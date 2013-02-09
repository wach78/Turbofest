using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Audio;
using OpenTK.Audio.OpenAL;

namespace OpenGL
{
    class Sound : IDisposable
    {
        private bool disposed = false;
        public enum FileType { Unknown=-1, WAV=0, Ogg=1 };

        private static int PlayingBuffer = -1;
        private static int PlayingSource = -1;
        private static AudioContext context = new AudioContext();
        
        static int buffer;
        static int Source;

        //private static List<KeyValuePair<string, int>> SoundList = new List<KeyValuePair<string, int>>(); // used to say what sounds we have
        private static Dictionary<string, int> SoundList = new Dictionary<string, int>(); // used to say what sounds we have
        //System.Threading.Thread tr;

        public Sound()
        {
            context.MakeCurrent(); // needed for playing sounds!!!
            Source = AL.GenSource();
            
            /*OpenTK.Vector3 v3 = new OpenTK.Vector3(1, 1, 1);
            AL.Source(AL.GenBuffer(), ALSource3f.Position, ref v3);*/

            //buffer = LoadWAV(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/fbk.wav");
            //buffer = LoadOgg(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Samples/free.ogg");
            //Play();
            /*tr = new System.Threading.Thread(new System.Threading.ThreadStart(Sound.Play));
            tr.Start();*/
        }

        ~Sound()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                SoundList.Clear();
                AL.DeleteSource(Source);
            }
            // free native resources if there are any.
            

            PlayingBuffer = -1;
            PlayingSource = -1;
            buffer = -1;
            Source = -1;
            disposed = true;
        }

        /// <summary>
        /// Adds the specefied filename to a list with Name as key and a aduio buffer as value
        /// </summary>
        /// <param name="ft">Identifies what type of sound file it is</param>
        /// <param name="filename"></param>
        /// <param name="Name"></param>
        public static void CreateSound(FileType ft, string filename, string Name)
        { 
            //make this add to a static list what sound name and sound buffer.
            if (ft == FileType.WAV)
            {
                //SoundList.Add(new KeyValuePair<string, int>(Name, Sound.LoadWAV(filename)));
                if (!SoundList.ContainsKey(Name))
                {
                    SoundList.Add(Name, Sound.LoadWAV(filename));
                }
                
            }
            else if (ft == FileType.Ogg)
            {
                 if (!SoundList.ContainsKey(Name))
                {
                    SoundList.Add(Name, Sound.LoadOgg(filename));
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
        public static int LoadWAV(string filename)
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
                throw new Exception("input file not RIFF");
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
            // Format end
            // DATA start
            subChunk2ID = new string(br.ReadChars(4));
            subChunk2Size = br.ReadInt32(); //BitConverter.ToInt32(br.ReadBytes(4).Reverse().ToArray(), 0);
            data = new byte[br.BaseStream.Length];
            data = br.ReadBytes((int)br.BaseStream.Length);
            // DATA end
            br.Close();
            //IntPtr dataPointer = System.Runtime.InteropServices.Marshal.AllocHGlobal(data.Length);//new IntPtr();
            //System.Runtime.InteropServices.Marshal.Copy(data,0,dataPointer, data.Length);
            //GCHandle pinnedArray = GCHandle.Alloc(byteArray, GCHandleType.Pinned);
            //IntPtr pointer = pinnedArray.AddrOfPinnedObject();
            //IntPtr dataPointer = System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(data, 0);

            
            
            int ab = AL.GenBuffer();
            AL.BufferData(ab, ALFormat.Stereo16, data, data.Length, SampleRate);
            
            //AL.Source(source, ALSourcei.Buffer, ab);
            /*//AL.SourceQueueBuffer(source, ab);
            AL.Source(source, ALSourcef.Gain, 1.0f);
            AL.Source(source, ALSourcef.Pitch, 1.0f);
             */
            return ab;
        }


        class OggHeader
        {
            public string CapturePattern;
            public short Version; // filesize
            public short HeaderType;
            public long GranulePosition;
            public int BitstreamSerialNumber;
            public int PageSequenceNumber;
            public int Checksum;
            public short PageSegment;
            public byte[] SegmentTable;
        }
       

        /// <summary>
        /// Loads a Ogg file, not functioning
        /// </summary>
        /// <param name="filename">path and filename</param>
        /// <returns>int AudioBuffer</returns>
        public static int LoadOgg(string filename)
        {
            /* OpenAL needs a binaryreadyer to get data from the sound-file. */
            Stream output = new MemoryStream(); // use this to make the PCM
            BinaryReader br = new BinaryReader(File.OpenRead(filename));
            //OGG_Header
            string CapturePattern;
            short Version; // filesize
            short HeaderType;
            long GranulePosition;
            int BitstreamSerialNumber;
            int PageSequenceNumber;
            int Checksum;
            short PageSegment;
            byte[] SegmentTable;

            byte[] data;

            /* OGG
             * OGG-page header is 27+ Bytes
            */

            CapturePattern = new string(br.ReadChars(4));

            if (CapturePattern != "OggS") // WAV-file header 1
            {
                throw new Exception("input file not Ogg");
            }
            Version = br.ReadByte();
            HeaderType = br.ReadByte();
            GranulePosition = br.ReadInt64();
            BitstreamSerialNumber = br.ReadInt32();
            PageSequenceNumber = br.ReadInt32();
            Checksum = br.ReadInt32();
            PageSegment = br.ReadByte();
            SegmentTable = br.ReadBytes(5);


            data = new byte[br.BaseStream.Length];
            data = br.ReadBytes((int)br.BaseStream.Length);
            // DATA end
            br.Close();

            int ab = AL.GenBuffer();
            //AL.BufferData(ab, ALFormat.Stereo16, data, data.Length, SampleRate);

            return ab;
        }

        /// <summary>
        /// Sets up a buffer from the Name and plays it
        /// </summary>
        /// <param name="Name">Name of the buffer</param>
        public void Play(string Name)
        {
            if (SoundList.ContainsKey(Name))
            {
                context.MakeCurrent();
                int state;
                Sound.PlayingBuffer = buffer = SoundList[Name];
                Sound.PlayingSource = Source;

                AL.Source(PlayingSource, ALSourcei.Buffer, SoundList[Name]);
                AL.Source(PlayingSource, ALSourcef.Gain, 1.0f);
                AL.Source(PlayingSource, ALSourcef.Pitch, 1.0f);
                AL.SourcePlay(PlayingSource);

                Console.WriteLine(AL.GetError());
                AL.GetSource(PlayingSource, ALGetSourcei.SourceState, out state);
                Console.WriteLine("Playing Source " + state); // 4116 dec = 1014 hex
                
                do
                {
                    System.Threading.Thread.Sleep(100);
                    Console.Write(".");
                    // Get Source State
                    AL.GetSource(PlayingSource, ALGetSourcei.SourceState, out state);
                } while ((ALSourceState)state == ALSourceState.Playing);


                AL.SourceStop(PlayingSource);
                Sound.PlayingBuffer = -1;
                Sound.PlayingSource = -1;
            }    
        }


        public void PlayThread()
        {
            context.MakeCurrent();
            Console.WriteLine(AL.GetError());
            int state;
            int oldPlayingBuffer = PlayingBuffer;
            int oldPlayingSource = PlayingSource;
            

            while (true)
            {
                AL.GetSource(PlayingSource, ALGetSourcei.SourceState, out state);
                Console.WriteLine("Playing Source, state is " + (ALSourceState)state);

                if (buffer != -1 && PlayingBuffer == -1)
                {
                    PlayingBuffer = buffer;
                }
                if (Source != -1 && PlayingSource== -1)
                {
                    PlayingSource = Source;
                }
                if (PlayingBuffer >= 0 && PlayingSource >= 0 && PlayingBuffer != oldPlayingBuffer)
                {

                    //if ((ALSourceState)state == ALSourceState.Stopped)
                    //{
                        Play();
                        oldPlayingBuffer = PlayingBuffer;
                    //}
                }
                
                System.Threading.Thread.Sleep(100);
            }

            
        }

        /// <summary>
        /// Playes a preset buffer
        /// </summary>
        public void Play()
        {
            
            context.MakeCurrent();
            int state;
            
            Sound.PlayingBuffer = buffer;
            Sound.PlayingSource = Source;

            AL.Source(PlayingSource, ALSourcei.Buffer, PlayingBuffer);
            Console.WriteLine(AL.GetError());
            //AL.SourceQueueBuffer(source, ab);

            AL.Source(PlayingSource, ALSourcef.Gain, 1.0f);
            AL.Source(PlayingSource, ALSourcef.Pitch, 1.0f);
            AL.SourcePlay(PlayingSource);
            Console.WriteLine(AL.GetError());
            Console.WriteLine("Playing Source " + Source);
            AL.GetSource(PlayingSource, ALGetSourcei.SourceState, out state);
            // 4116 = ALSourceState.Stopped
            
            Console.WriteLine("Playing Source, state is " + (ALSourceState)state);
            //Console.WriteLine(AL.GetError());
            
            do
            {
                System.Threading.Thread.Sleep(100);
                //Console.Write(".");
                // Get Source State
                AL.GetSource(PlayingSource, ALGetSourcei.SourceState, out state);
                //Console.WriteLine("Playing Source, state is " + (ALSourceState)state);
                if (PlayingSource == -1)
                {
                    break;
                }
            } while ((ALSourceState)state == ALSourceState.Playing);

            //AL.SourceStop(PlayingSource); //the sound should already be stopped no need
            Sound.PlayingBuffer = -1;
            //Sound.PlayingSource = -1;
        }

        public void SetAudioBuffer(int buffer)
        { 
        
        }

        public static void SetAudioBuffer(string Name)
        {
            try
            {
                buffer = SoundList[Name];
            }
            catch (Exception)
            {
                
                throw;
            }
            
            //AL.Source(Source, ALSourcei.Buffer, SoundList[Name]);
        }

        public void SetSource(int source)
        {

        }

        public void Stop()
        {
            int state;
            AL.GetSource(PlayingSource, ALGetSourcei.SourceState, out state);
            if (ALSourceState.Playing == (ALSourceState)state)
            {
                AL.SourceStop(Sound.PlayingSource);
            }
            // setting this before the sound loop is done is bad!
            //Sound.PlayingBuffer = -1;
            //Sound.PlayingSource = -1;
        }
    }
}
