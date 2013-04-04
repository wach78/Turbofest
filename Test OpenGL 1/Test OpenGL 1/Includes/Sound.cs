using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using System.Threading;

namespace OpenGL
{
    class Sound : IDisposable
    {
        /// <summary>
        /// What type of file is to be loaded.
        /// </summary>
        public enum FileType { Unknown = -1, WAV = 0, Ogg = 1 };
        private bool disposed;
        /// <summary>
        /// This is the list with sounds and what buffer is to be used
        /// </summary>
        private  Dictionary<string, int> SoundList;
        /// <summary>
        /// The sound thread that we will use to play sounds
        /// </summary>
        private System.Threading.Thread tr;
        private AudioContext context;
        private int SoundSource;
        private bool RunSoundThread;

        bool isPlaying;
        string NowPlayingName;
        int NowPlayingBuffer;
        string NextPlayingName;
        int NextPlayingBuffer;
        int numPLayedBuffer;

        public Sound(bool StartThread)
        {
            disposed = false;
            SoundList = new Dictionary<string, int>();
            context = new AudioContext();
            context.MakeCurrent();
            SoundSource = AL.GenSource();
            RunSoundThread = true;

            isPlaying = false;
            NowPlayingName = string.Empty;
            NowPlayingBuffer = -1;
            NextPlayingName = string.Empty;
            NextPlayingBuffer = -1;
            numPLayedBuffer = 0; //??
            /*OpenTK.Vector3 v3 = new OpenTK.Vector3(1, 1, 1);
            AL.Source(AL.GenBuffer(), ALSource3f.Position, ref v3);*/

            tr = new System.Threading.Thread(new ThreadStart(PlayThread));
            if (StartThread)
            {
                RunThread();
            }
        }

        #region Dispose
        ~Sound()
        {
            Console.WriteLine("Sound Destructor / Finalizer");
            Dispose(false);
        }

        public void Dispose()
        {
            Console.WriteLine("Sound Dispose");
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            //Console.WriteLine("Dispose called with "+ (disposed?"Disposing":"Not Disposing"));
            if (disposing)
            {
                // free managed resources
                if (tr.IsAlive)
                {
                    RunSoundThread = false;
                }
                tr = null; // is bad if it's not stopped?
                if (SoundSource != -1)
                {
                    AL.SourceStop(SoundSource);
                    AL.DeleteSource(SoundSource);
                }
                foreach (var item in SoundList)
                {
                    AL.DeleteBuffer(item.Value);
                }
                SoundList.Clear();
                SoundList = null;
            }
            // free native resources if there are any.
            disposed = true;
            Console.WriteLine(this.GetType().ToString() + " disposed.");
        }
        #endregion

        private void PlayThread()
        {
            int sourceState;
            context.MakeCurrent();
            Console.WriteLine("Sound thread started.");
            while (RunSoundThread)
            {
                //Check if there is a sound to play
                if (NowPlayingBuffer == -1 && NextPlayingBuffer != -1)
                {
                    NowPlayingBuffer = NextPlayingBuffer;
                    NowPlayingName = NextPlayingName;
                    NextPlayingBuffer = -1;
                    AL.Source(SoundSource, ALSourcei.Buffer, NowPlayingBuffer);
                    //AL.Source(SoundSource, ALSourcef.Gain, 1.0f);
                    //AL.Source(SoundSource, ALSourcef.Pitch, 1.0f);
                    AL.SourcePlay(SoundSource);
                    isPlaying = true;
                }


                //Check if there is playing a sound
                if (SoundSource != -1)
                {
                    do
                    {
                        if (!RunSoundThread || !isPlaying)
                        {
                            break;
                        }
                        /*if (!isPlaying)
                        {
                            isPlaying = true;    
                        }*/
                        
                        System.Threading.Thread.Sleep(100); // let other threads run.
                        //Console.Write(".");
                        // Get Source State
                        AL.GetSource(SoundSource, ALGetSourcei.SourceState, out sourceState);
                    } while ((ALSourceState)sourceState == ALSourceState.Playing);
                    AL.SourceStop(SoundSource);
                }
                NowPlayingBuffer = -1;
                NowPlayingName = string.Empty;
                isPlaying = false;
                Thread.Sleep(10); // just to let other threads run...
            }
            Console.WriteLine("Sound thread stopped.");
        }

        public void RunThread()
        {
            if (!tr.IsAlive)
            {
                tr.Name = "SoundThreadGLWindow";
                tr.Start();
            }
            
        }

        public void StopThread()
        {
            RunSoundThread = false;
        }

        public void StopSound()
        {
            isPlaying = false;
        }

        /// <summary>
        /// Adds the specefied filename to a list with Name as key and a aduio buffer as value
        /// </summary>
        /// <param name="ft">Identifies what type of sound file it is</param>
        /// <param name="filename"></param>
        /// <param name="Name"></param>
        public void CreateSound(FileType ft, string filename, string Name)
        { 
            //make this add to a static list what sound name and sound buffer.
            if (ft == FileType.WAV)
            {
                // We might want to throw a exception here if there are more with the same name trying to get added.
                if (!SoundList.ContainsKey(Name))
                {
                    lock (SoundList)
                    {
                        SoundList.Add(Name, LoadWAV(filename));
                    }
                }
                
            }
            else if (ft == FileType.Ogg)
            {
                // We might want to throw a exception here if there are more with the same name trying to get added.
                if (!SoundList.ContainsKey(Name))
                {
                    lock (SoundList)
                    {
                        //SoundList.Add(Name, LoadOGG(filename));
                        throw new Exception("Not implemented!");
                    }
                }
                
            }
            else
            {
                throw new Exception("Not a valid soundfile.");
            }
        }

        /// <summary>
        /// Loads a RIFF WAV
        /// </summary>
        /// <param name="filename">path and filename</param>
        /// <returns>int AudioBuffer</returns>
        public int LoadWAV(string filename)
        {
            //AudioContext context = new AudioContext(); // needed for playing sounds!!!
            /* OpenAL needs a binaryreadyer to get data from the sound-file.
             * 
             */
            BinaryReader br = new BinaryReader(File.OpenRead(filename));
            //RIFF_Header
            string ChunkID;
            int ChunkSize; // filesize
            string Format;
            //WAVE_Format
            string subChunk1ID; // fmt 
            int subChunk1Size; // fmt Size
            short audioFormat;
            short numChannels;
            int SampleRate;
            int ByteRate;
            short ByteAlign;
            short BitsPerSample;

            /*int mBytesPerPacket;
            int mFramesPerPacket;
            int mChannelsPerFrame;
            int mBitsPerChannel;*/
            //WAVE_Data
            string subChunk2ID;
            int subChunk2Size;
            byte[] data;

            // OpenAL Buffer
            int ab = AL.GenBuffer();

            /* WAVE
             * RIFF-header is 4x4 Bytes
             * Format is 2x4, 2x2, 2x4, 2x2 Bytes
             * DATA have headers 2x4 Bytes with rest of data as sound info
            */

            ChunkID = new string(br.ReadChars(4));

            if (ChunkID != "RIFF") // WAV-file header 1
            {
                throw new Exception("input file not RIFF");
            }
            ChunkSize = br.ReadInt32(); //BitConverter.ToInt32(br.ReadBytes(4).Reverse().ToArray(), 0); // rest of caf file header, casts error!?
            Format = new string(br.ReadChars(4));
            if (Format != "WAVE") // WAV-file header 1
            {
                throw new Exception("input file not WAVE");
            }
            // HEADER end
            // Format start
            subChunk1ID = new string(br.ReadChars(4));
            subChunk1Size = br.ReadInt32(); //BitConverter.ToInt32(br.ReadBytes(4).ToArray(), 0); 
            audioFormat = br.ReadInt16(); //BitConverter.ToInt16(br.ReadBytes(2).Reverse().ToArray(), 0); 
            numChannels = br.ReadInt16(); //BitConverter.ToInt16(br.ReadBytes(2).Reverse().ToArray(), 0);
            SampleRate = br.ReadInt32(); //BitConverter.ToInt32(br.ReadBytes(4).Reverse().ToArray(), 0);
            ByteRate = br.ReadInt32(); //BitConverter.ToInt32(br.ReadBytes(4).Reverse().ToArray(), 0);
            ByteAlign = br.ReadInt16(); //BitConverter.ToInt16(br.ReadBytes(2).Reverse().ToArray(), 0);
            BitsPerSample = br.ReadInt16(); //BitConverter.ToInt16(br.ReadBytes(2).Reverse().ToArray(), 0);
            ALFormat alf = 0;
            if (numChannels == 1) // mono
            {
                if (BitsPerSample == 8)
                {
                    alf = ALFormat.Mono8;
                }
                else // say that it is 16 even if not, bad way...
                {
                    alf = ALFormat.Mono16;
                }
            }
            else if (numChannels == 2) // sterio
            {
                if (BitsPerSample == 8)
                {
                    alf = ALFormat.Stereo8;
                }
                else // say that it is 16 even if not, bad way...
                {
                    alf = ALFormat.Stereo16;
                }
            }
            if (alf == 0)
            {
                throw new Exception("Wrong number of channels in sound file.");
            }
            // Format end
            // DATA start
            subChunk2ID = new string(br.ReadChars(4));
            subChunk2Size = br.ReadInt32(); //BitConverter.ToInt32(br.ReadBytes(4).Reverse().ToArray(), 0);
            data = new byte[br.BaseStream.Length];
            data = br.ReadBytes((int)br.BaseStream.Length);
            // DATA end
            
            //IntPtr dataPointer = System.Runtime.InteropServices.Marshal.AllocHGlobal(data.Length);//new IntPtr();
            //System.Runtime.InteropServices.Marshal.Copy(data,0,dataPointer, data.Length);
            //GCHandle pinnedArray = GCHandle.Alloc(byteArray, GCHandleType.Pinned);
            //IntPtr pointer = pinnedArray.AddrOfPinnedObject();
            //IntPtr dataPointer = System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(data, 0);

            AL.BufferData(ab, alf, data, data.Length, SampleRate);
            br.Close();
            //Add this if you don't want to have large memory allocation, as the allocation sticks to the program until gc can free it
            /*data = new byte[1]; // force release of data
            GC.Collect(); // well well well, bleh! this releases the memory only if it is top on heap else nothing happens...
            */
            return ab;
        }
        /*
        public int LoadOGG(string filename)
        {
            //NVorbis.OpenTKSupport.OggStream asd = new NVorbis.OpenTKSupport.OggStream(filename, 1);
            BinaryReader br = new BinaryReader(File.OpenRead(filename));
            NVorbis.VorbisReader reader = new NVorbis.VorbisReader(filename);
            //NVorbis.OpenTKSupport.OggStreamer asd2 = new NVorbis.OpenTKSupport.OggStreamer();
            
            int ab = AL.GenBuffer();
            
            ALFormat alf = 0;
            if (reader.Channels == 1) // mono
            {
                alf = ALFormat.Mono16;
            }
            else if (reader.Channels == 2) // sterio
            {
                alf = ALFormat.Stereo16;
            }
            else if (alf == 0)
            {
                throw new Exception("Wrong number of channels in sound file.");
            }

            List<float> Rdata = new List<float>();
            float[] data = new float[44100];
            int readSamples;
            //data = br.ReadBytes((int)br.BaseStream.Length);
            do
            {
                readSamples = reader.ReadSamples(data, Rdata.Count, 44100);
                Rdata.AddRange(data);
            } while (readSamples > 0);

            AL.BufferData(ab, alf, data, data.Length, reader.SampleRate);

            reader.Dispose();
            br.Close();
            br.Dispose();
            data = null;
            
            return ab;
        }*/


        /// <summary>
        /// Sets up a buffer from the Name and plays it
        /// </summary>
        /// <param name="Name">Name of the buffer</param>
        public void Play(string Name)
        {
            if (SoundList.ContainsKey(Name))
            {
                NextPlayingName = Name;
                NextPlayingBuffer = SoundList[Name];
            }
        }

        public string PlayingName()
        {
            return NowPlayingName;
        }

        public int PlayingBuffer()
        {
            return NowPlayingBuffer;
        }
    } // end class
}
