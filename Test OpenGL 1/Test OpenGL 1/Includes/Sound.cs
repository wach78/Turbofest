using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using System.Threading;
using csogg;
using csvorbis;
using System.Diagnostics;

namespace OpenGL
{
    class StreamFile : IDisposable
    {
        private bool disposed;
        //private bool Streaming;

        public StreamFile(string FileName)
        {
            
        }

        #region Dispose
        ~StreamFile()
        {
            System.Diagnostics.Debug.WriteLine("Sound Destructor / Finalizer");
            Dispose(false);
        }

        public void Dispose()
        {
            System.Diagnostics.Debug.WriteLine("Sound Dispose");
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            //System.Diagnostics.Debug.WriteLine("Dispose called with "+ (disposed?"Disposing":"Not Disposing"));
            if (!this.disposed)
            {
                if (disposing)
                {
                    // free managed resources
                    
                }
                // free native resources if there are any.
                disposed = true;
                System.Diagnostics.Debug.WriteLine(this.GetType().ToString() + " disposed.");
            }
        }
        #endregion

    }

    class SoundType : IDisposable
    {
        private bool disposed;
        private bool Streaming;
        private string File;
        private int BufferID;
        private VorbisFile FileToBuffer;
        private Info FileInfoVO;
        private ALFormat alf;

        public SoundType(string FileName, bool IsToBeStreaming)
        {
            if (!System.IO.File.Exists(FileName))
            {
                throw new Exception("Missing sound file!");
            }
            File = FileName;
            Streaming = IsToBeStreaming;
            if (!Streaming)
            {
                BufferID = AL.GenBuffer();
            }
            else
            {
                BufferID = -1;
            }
            // This makes the program take some time to load...
            FileToBuffer = new VorbisFile(File);
            FileInfoVO = FileToBuffer.getInfo(-1);
            alf = 0;
            /*int asd = FileToBuffer.bitrate(-1);
            int asd2 = FileToBuffer.bitrate_instant();*/
            if (FileInfoVO.channels == 1) // mono
            {
                alf = ALFormat.Mono16;
            }
            else if (FileInfoVO.channels == 2) // sterio
            {
                alf = ALFormat.Stereo16;
            }
            else if (alf == 0)
            {
                throw new Exception("Wrong number of channels in sound file.");
            }
        }


        #region Dispose
        ~SoundType()
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
            if (!this.disposed)
            {
                if (disposing)
                {
                    // free managed resources
                    if (BufferID  >= 0) //???
                    {
                        AL.DeleteBuffer(BufferID);    
                    }
                    FileToBuffer = null;
                    FileInfoVO.clear();
                    FileInfoVO = null;
                }
                // free native resources if there are any.
                disposed = true;
                System.Diagnostics.Debug.WriteLine(this.GetType().ToString() + " disposed.");
            }
        }
        #endregion

        public bool ToBeStreamed
        {
            get { return Streaming; }
            set { }
        }

        public string Filename
        {
            get { return File; }
            set { }
        }

        public int Buffer
        {
            get { return BufferID; }
            set { }
        }

        public VorbisFile FileData
        {
            get { return FileToBuffer; }
            set { }
        }

        public Info FileInfo
        {
            get { return FileInfoVO; }
            set { }
        }
        public ALFormat StereoMono
        {
            get { return alf; }
            set { }
        }
    }
}

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
        private Dictionary<string, SoundType> SoundList;
        /// <summary>
        /// The sound thread that we will use to play sounds
        /// </summary>
        private System.Threading.Thread tr;
        private AudioContext context;
        private int SoundSource;
        private int SoundStreamSource;
        private bool RunSoundThread;
        private int[] SoundBuffers;

        bool isPlaying;
        string NowPlayingName;
        //int NowPlayingBuffer;
        string NextPlayingName;
        //int NextPlayingBuffer;
        //int numPLayedBuffer;

        public Sound(bool StartThread)
        {
            disposed = false;
            SoundList = new Dictionary<string, SoundType>();
            context = new AudioContext(); // default audio device
            lock (context)
            {
                context.MakeCurrent();
            }
            //XRamExtension XRam = new XRamExtension();
            
            
            /*SoundBuffers = AL.GenBuffers(4); // number of buffers
            SoundSource = AL.GenSource();
            SoundStreamSource = AL.GenSource();*/
            RunSoundThread = true;

            isPlaying = false;
            NowPlayingName = string.Empty;
            //NowPlayingBuffer = -1;
            NextPlayingName = string.Empty;
            //NextPlayingBuffer = -1;
            //numPLayedBuffer = 0; //??
            /*OpenTK.Vector3 v3 = new OpenTK.Vector3(1, 1, 1);
            AL.Source(AL.GenBuffer(), ALSource3f.Position, ref v3);*/

            //Debug.WriteLine(AL.Get(ALGetString.Version));
            

            tr = new System.Threading.Thread(new ThreadStart(PlayThread));
            if (StartThread)
            {
                RunThread();
            }
        }

        #region Dispose
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
            if (!this.disposed)
            {
                if (disposing)
                {
                    // free managed resources
                    if (tr.IsAlive)
                    {
                        Console.WriteLine("sound thread is running!!!!!");
                        RunSoundThread = false;
                        StopSound();
                        Thread.Sleep(100);
                    }
                    tr = null; // is bad if it's not stopped?
                    context.MakeCurrent();
                    if (SoundSource != -1)
                    {
                        AL.SourceStop(SoundSource);
                        AL.DeleteSource(SoundSource);
                        SoundSource = -1;
                    }

                    if (SoundStreamSource != -1)
                    {
                        AL.SourceStop(SoundStreamSource);
                        AL.SourceUnqueueBuffers(SoundStreamSource,4);
                        AL.DeleteSource(SoundStreamSource);
                        AL.DeleteBuffers(SoundBuffers);
                        SoundStreamSource = -1;
                    }
                    foreach (var item in SoundList)
                    {
                        item.Value.Dispose();
                    }
                    SoundList.Clear();
                    SoundList = null;
                    context.Dispose();
                    context = null;
                }
                // free native resources if there are any.
                disposed = true;
                System.Diagnostics.Debug.WriteLine(this.GetType().ToString() + " disposed.");
            }
        }
        #endregion

        private void PlayThread()
        {
            int sourceState;
            SoundType st;
            byte[] BufferData = new byte[4096*10];
            byte[] BufferData2 = new byte[4096];
            lock (context)
            {
                context.MakeCurrent();
                SoundBuffers = AL.GenBuffers(4); // number of buffers
                SoundSource = AL.GenSource();
                SoundStreamSource = AL.GenSource();
                
                System.Diagnostics.Debug.WriteLine("Sound thread started.");
                while (RunSoundThread)
                {
                    //Check if there is a sound to play
                    if (NowPlayingName == string.Empty && NextPlayingName != string.Empty)
                    {
                        //NowPlayingBuffer = NextPlayingBuffer;
                        NowPlayingName = NextPlayingName;

                        if (SoundList.ContainsKey(NowPlayingName))
                        {
                            st = SoundList[NowPlayingName];
                            //NextPlayingBuffer = -1;
                            NextPlayingName = string.Empty;

                            if (st.ToBeStreamed)
                            {
                                // open file to read to buffers
                                /*VorbisFile FileToBuffer = new VorbisFile(st.Filename);
                                Info FileInfo = FileToBuffer.getInfo(-1);
                                ALFormat alf = 0;*/
                                int fileRead, readSize;
                                /*if (FileInfo.channels == 1) // mono
                                {
                                    alf = ALFormat.Mono16;
                                }
                                else if (FileInfo.channels == 2) // stereo
                                {
                                    alf = ALFormat.Stereo16;
                                }
                                else if (alf == 0)
                                {
                                    throw new Exception("Wrong number of channels in sound file.");
                                }*/
                                //readSize = fileRead = 0; // buggs out

                                for (int i = 0; i < SoundBuffers.Length; i++)
                                {
                                    if (NowPlayingName == "Sune") 
                                        Console.Write("");
                                    fileRead = st.FileData.read(BufferData, BufferData.Length, 0, 2, 1, null);
                                    AL.BufferData(SoundBuffers[i], st.StereoMono, BufferData, fileRead, st.FileInfo.rate);
                                    //readSize += fileRead;
                                    Array.Clear(BufferData, 0, BufferData.Length);
                                }


                                AL.SourceQueueBuffers(SoundStreamSource, SoundBuffers.Length, SoundBuffers);
                                AL.SourcePlay(SoundStreamSource);
                                isPlaying = true;

                               

                                int ProcessedBuffers = 0;
                                /*int QueuedBuffers = 0;*/
                                int[] section = new int[10];
                                int BufferRef;
                                bool EOF = false;

                                do
                                {
                                    if (!RunSoundThread || !isPlaying)
                                    {
                                        System.Diagnostics.Debug.WriteLine("Ending loop with boolean.");
                                        break;
                                    }

                                    Thread.Sleep(10);

                                    AL.GetSource(SoundStreamSource, ALGetSourcei.BuffersProcessed, out ProcessedBuffers);
                                    while (ProcessedBuffers > 0 && isPlaying)
                                    {
                                        readSize = fileRead = 0;
                                        BufferRef = AL.SourceUnqueueBuffer(SoundStreamSource);
                                        if (BufferRef != 0 && !EOF && isPlaying)
                                        {
                                            while (readSize < (BufferData.Length * 0.75) && isPlaying) // this is not at all safe!!!
                                            {
                                                fileRead = st.FileData.read(BufferData2, BufferData2.Length, 0, 2, 1, null);
                                                //System.Diagnostics.Debug.WriteLine("Bytes read: " + fileRead);
                                                if (!isPlaying)
                                                {
                                                    AL.SourceStop(SoundStreamSource);
                                                    break;
                                                }
                                                if (fileRead > 0)
                                                {
                                                    Buffer.BlockCopy(BufferData2, 0, BufferData, readSize, fileRead);
                                                    readSize += fileRead;
                                                    Array.Clear(BufferData2, 0, BufferData2.Length);
                                                }
                                                else if (fileRead < 0)
                                                {
                                                    EOF = true;
                                                    break;
                                                }
                                                else
                                                {
                                                    EOF = true;
                                                    System.Diagnostics.Debug.WriteLine("File is at the end no more data. Size read: " + readSize.ToString());
                                                    break;
                                                }
                                            }

                                            if (isPlaying && readSize > 0)
                                            {
                                                AL.BufferData(BufferRef, st.StereoMono, BufferData, /*BufferData.Length*/readSize, st.FileInfo.rate);
                                                /*System.Diagnostics.Debug.WriteLine("Buffering data: " + AL.GetErrorString(AL.GetError()));
                                                System.Diagnostics.Debug.WriteLine("ALC: " + context.CurrentError.ToString());*/
                                                AL.SourceQueueBuffer(SoundStreamSource, BufferRef);
                                                //System.Diagnostics.Debug.WriteLine("source queuing: " + AL.GetErrorString(AL.GetError()));
                                            }
                                            Array.Clear(BufferData, 0, BufferData.Length);
                                            Array.Clear(BufferData2, 0, BufferData2.Length);
                                        }
                                        --ProcessedBuffers;
                                    }

                                    // Get Source State
                                    AL.GetSource(SoundStreamSource, ALGetSourcei.SourceState, out sourceState);
                                } while ((ALSourceState)sourceState == ALSourceState.Playing && isPlaying);
                                System.Diagnostics.Debug.WriteLine("Not playing any more.");
                                AL.SourceStop(SoundStreamSource);
                                AL.Source(SoundStreamSource, ALSourcei.Buffer, 0);
                                AL.SourceUnqueueBuffers(SoundStreamSource, SoundBuffers.Length);
                                if (st.FileData.seekable())
                                { // this might not always be true
                                    st.FileData.raw_seek(0);
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine("ej sök");
                                }
                                //seems like we can get error on dates where we have the same type of event...
                                
                                /*FileInfo.clear();
                                FileInfo = null;
                                FileToBuffer = null;*/
                            } // end is to be streamd
                            else
                            {
                                AL.Source(SoundSource, ALSourcei.Buffer, st.Buffer);
                                AL.SourcePlay(SoundSource);
                                isPlaying = true;
                                do
                                {
                                    if (!RunSoundThread || !isPlaying)
                                    {
                                        break;
                                    }

                                    System.Threading.Thread.Sleep(100); // let other threads run.

                                    // Get Source State
                                    AL.GetSource(SoundSource, ALGetSourcei.SourceState, out sourceState);
                                } while ((ALSourceState)sourceState == ALSourceState.Playing);
                                AL.SourceStop(SoundSource);
                            } // end else
                        }// end playing content that is in list

                        /*AL.SourceStop(SoundSource);
                        AL.SourceStop(SoundStreamSource);*/
                        NowPlayingName = string.Empty;
                        st = null;
                        isPlaying = false;
                    }

                    Thread.Sleep(10); // just to let other threads run...
                }
            }
            System.Diagnostics.Debug.WriteLine("Sound thread stopped.");
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
            System.Diagnostics.Debug.WriteLine("Stoping sound method");
            isPlaying = false;
            AL.SourceStop(SoundSource); // to be safe
            AL.SourceStop(SoundStreamSource); // to be safe
            NowPlayingName = string.Empty;
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
                        SoundList.Add(Name, new SoundType(filename, false));
                        if (!SoundList[Name].ToBeStreamed)
                        {
                            LoadWAV(SoundList[Name]);
                        }
                        //SoundList.Add(Name, LoadWAV(filename));
                    }
                }
                
            }
            else if (ft == FileType.Ogg)
            {
                // With this we can still play but we will not play the ogg music...
                if (!File.Exists("csogg.dll") || !File.Exists("csvorbis.dll"))
                {
                    return; // or throw a error
                }
                // We might want to throw a exception here if there are more with the same name trying to get added.
                if (!SoundList.ContainsKey(Name))
                {
                    lock (SoundList)
                    {
                        /*if (Name == "rms" || Name == "FBK")
                        {*/

                            SoundList.Add(Name, new SoundType(filename, /*(Name == "rms" ?*/ true /*: false)*/));
                            /*if (!SoundList[Name].ToBeStreamed)
                            {
                                LoadOGG(SoundList[Name]);
                            }
                        }*/
                        //SoundList.Add(Name, LoadOGG(filename));
                        //throw new Exception("Not implemented!");
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
        public void LoadWAV(SoundType sound)
        {
            //AudioContext context = new AudioContext(); // needed for playing sounds!!!
            /* OpenAL needs a binaryreadyer to get data from the sound-file.
             * 
             */
            BinaryReader br = new BinaryReader(File.OpenRead(sound.Filename));
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
            int ab = sound.Buffer;

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
            //return ab;
        }

        public void LoadOGG(SoundType sound)
        {
            // Dirty way of checking that we have ogg-dlls in exe path....
            if (!File.Exists("csogg.dll") || !File.Exists("csvorbis.dll"))
            {
                return; // return 0;
            }

            int ab = sound.Buffer;

//#define ogg // or activate this, but this is just for this place/file... :D
#if noogg
#warning "Returning 0 on Ogg file, you need to define \"ogg\" in build constants, in project build property."
            return ; // quicker upstart
#endif

            VorbisFile asd = new VorbisFile(sound.Filename);
            System.Diagnostics.Debug.WriteLine(sound.Filename);
            Info info = asd.getInfo(-1);
            //long samples = asd.pcm_total(-1);
            //int streams = asd.streams();
            //byte[] data3 = new byte[samples*streams];
            byte[] data2 = new byte[4096];
            Stream output2 = new MemoryStream();
            int readBytes = 0;

            while ((readBytes = asd.read(data2, data2.Length, 0 /*Bigendian*/, /*(info.rate < 44100 ? 2 : 2)*/ 2 /*1=byte, 2=16bit*/, 1/*signd*/, null)) > 0)
            {
                output2.Write(data2, 0, readBytes);
            }
            data2 = null;
            data2 = new byte[output2.Length];
            output2.Seek(0, SeekOrigin.Begin);
            output2.Read(data2, 0, data2.Length);
            output2.Close();
            output2.Dispose();
            //
            #region csogg and stuff, commented
            // borrow from csogg and csvorbis...
            /*using (var input = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                System.Diagnostics.Debug.WriteLine(filename);
                bool skipWavHeader = true;
                int HEADER_SIZE = 36;

                int convsize = 4096 * 2;
                byte[] convbuffer = new byte[convsize]; // take 8k out of the data segment, not the stack

                Stream output = new MemoryStream();
                if (!skipWavHeader)
                    output.Seek(HEADER_SIZE, SeekOrigin.Begin); // reserve place for WAV header

                SyncState oy = new SyncState(); // sync and verify incoming physical bitstream
                StreamState os = new StreamState(); // take physical pages, weld into a logical stream of packets
                Page og = new Page(); // one Ogg bitstream page.  Vorbis packets are inside
                Packet op = new Packet(); // one raw packet of data for decode

                Info vi = new Info();  // struct that stores all the static vorbis bitstream settings
                Comment vc = new Comment(); // struct that stores all the bitstream user comments
                DspState vd = new DspState(); // central working state for the packet->PCM decoder
                Block vb = new Block(vd); // local working space for packet->PCM decode

                byte[] buffer;
                int bytes = 0;

                // Decode setup

                oy.init(); // Now we can read pages

                while (true)
                { // we repeat if the bitstream is chained
                    int eos = 0;

                    // grab some data at the head of the stream.  We want the first page
                    // (which is guaranteed to be small and only contain the Vorbis
                    // stream initial header) We need the first page to get the stream
                    // serialno.

                    // submit a 4k block to libvorbis' Ogg layer
                    int index = oy.buffer(4096);
                    buffer = oy.data;
                    try
                    {
                        bytes = input.Read(buffer, index, 4096);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e);
                    }
                    oy.wrote(bytes);

                    // Get the first page.
                    if (oy.pageout(og) != 1)
                    {
                        // have we simply run out of data?  If so, we're done.
                        if (bytes < 4096) break;

                        // error case.  Must not be Vorbis data
                        System.Diagnostics.Debug.WriteLine("Input does not appear to be an Ogg bitstream.");
                    }

                    // Get the serial number and set up the rest of decode.
                    // serialno first; use it to set up a logical stream
                    os.init(og.serialno());

                    // extract the initial header from the first page and verify that the
                    // Ogg bitstream is in fact Vorbis data

                    // I handle the initial header first instead of just having the code
                    // read all three Vorbis headers at once because reading the initial
                    // header is an easy way to identify a Vorbis bitstream and it's
                    // useful to see that functionality seperated out.

                    vi.init();
                    vc.init();
                    if (os.pagein(og) < 0)
                    {
                        // error; stream version mismatch perhaps
                        System.Diagnostics.Debug.WriteLine("Error reading first page of Ogg bitstream data.");
                    }

                    if (os.packetout(op) != 1)
                    {
                        // no page? must not be vorbis
                        System.Diagnostics.Debug.WriteLine("Error reading initial header packet.");
                    }

                    if (vi.synthesis_headerin(vc, op) < 0)
                    {
                        // error case; not a vorbis header
                        System.Diagnostics.Debug.WriteLine("This Ogg bitstream does not contain Vorbis audio data.");
                    }

                    // At this point, we're sure we're Vorbis.  We've set up the logical
                    // (Ogg) bitstream decoder.  Get the comment and codebook headers and
                    // set up the Vorbis decoder

                    // The next two packets in order are the comment and codebook headers.
                    // They're likely large and may span multiple pages.  Thus we reead
                    // and submit data until we get our two pacakets, watching that no
                    // pages are missing.  If a page is missing, error out; losing a
                    // header page is the only place where missing data is fatal. 

                    int i = 0;

                    while (i < 2)
                    {
                        while (i < 2)
                        {

                            int result = oy.pageout(og);
                            if (result == 0) break; // Need more data
                            // Don't complain about missing or corrupt data yet.  We'll
                            // catch it at the packet output phase

                            if (result == 1)
                            {
                                os.pagein(og); // we can ignore any errors here
                                // as they'll also become apparent
                                // at packetout
                                while (i < 2)
                                {
                                    result = os.packetout(op);
                                    if (result == 0) break;
                                    if (result == -1)
                                    {
                                        // Uh oh; data at some point was corrupted or missing!
                                        // We can't tolerate that in a header.  Die.
                                        System.Diagnostics.Debug.WriteLine("Corrupt secondary header.  Exiting.");
                                    }
                                    vi.synthesis_headerin(vc, op);
                                    i++;
                                }
                            }
                        }
                        // no harm in not checking before adding more
                        index = oy.buffer(4096);
                        buffer = oy.data;
                        try
                        {
                            bytes = input.Read(buffer, index, 4096);
                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine(e);
                        }
                        if (bytes == 0 && i < 2)
                        {
                            System.Diagnostics.Debug.WriteLine("End of file before finding all Vorbis headers!");
                        }
                        oy.wrote(bytes);
                    }

                    // Throw the comments plus a few lines about the bitstream we're decoding
                    {
                        byte[][] ptr = vc.user_comments;
                        for (int j = 0; j < vc.user_comments.Length; j++)
                        {
                            if (ptr[j] == null) break;
                            System.Diagnostics.Debug.WriteLine(vc.getComment(j));
                        }
                        System.Diagnostics.Debug.WriteLine("\nBitstream is " + vi.channels + " channel, " + vi.rate + "Hz");
                        System.Diagnostics.Debug.WriteLine("Encoded by: " + vc.getVendor() + "\n");
                    } // comment this on release...
                    
                    convsize = 4096 / vi.channels;

                    // OK, got and parsed all three headers. Initialize the Vorbis
                    //  packet->PCM decoder.
                    vd.synthesis_init(vi); // central decode state
                    vb.init(vd);           // local state for most of the decode

                    // so multiple block decodes can
                    // proceed in parallel.  We could init
                    // multiple vorbis_block structures
                    // for vd here

                    float[][][] _pcm = new float[1][][];
                    int[] _index = new int[vi.channels];
                    // The rest is just a straight decode loop until end of stream
                    while (eos == 0)
                    {
                        while (eos == 0)
                        {

                            int result = oy.pageout(og);
                            if (result == 0) break; // need more data
                            if (result == -1)
                            { // missing or corrupt data at this page position
                                System.Diagnostics.Debug.WriteLine("Corrupt or missing data in bitstream; continuing...");
                            }
                            else
                            {
                                os.pagein(og); // can safely ignore errors at
                                // this point
                                while (true)
                                {
                                    result = os.packetout(op);

                                    if (result == 0) break; // need more data
                                    if (result == -1)
                                    { // missing or corrupt data at this page position
                                        // no reason to complain; already complained above
                                    }
                                    else
                                    {
                                        // we have a packet.  Decode it
                                        int samples;
                                        if (vb.synthesis(op) == 0)
                                        { // test for success!
                                            vd.synthesis_blockin(vb);
                                        }

                                        // **pcm is a multichannel float vector.  In stereo, for
                                        // example, pcm[0] is left, and pcm[1] is right.  samples is
                                        // the size of each channel.  Convert the float values
                                        // (-1.<=range<=1.) to whatever PCM format and write it out

                                        while ((samples = vd.synthesis_pcmout(_pcm, _index)) > 0)
                                        {
                                            float[][] pcm = _pcm[0];
                                            bool clipflag = false;
                                            int bout = (samples < convsize ? samples : convsize);

                                            // convert floats to 16 bit signed ints (host order) and
                                            // interleave
                                            for (i = 0; i < vi.channels; i++)
                                            {
                                                int ptr = i * 2;
                                                //int ptr=i;
                                                int mono = _index[i];
                                                for (int j = 0; j < bout; j++)
                                                {
                                                    int val = (int)(pcm[i][mono + j] * 32767.0);
                                                    //        short val=(short)(pcm[i][mono+j]*32767.);
                                                    //        int val=(int)Math.round(pcm[i][mono+j]*32767.);
                                                    // might as well guard against clipping
                                                    if (val > 32767)
                                                    {
                                                        val = 32767;
                                                        clipflag = true;
                                                    }
                                                    if (val < -32768)
                                                    {
                                                        val = -32768;
                                                        clipflag = true;
                                                    }
                                                    if (val < 0) val = val | 0x8000;
                                                    convbuffer[ptr] = (byte)(val);
                                                    convbuffer[ptr + 1] = (byte)((uint)val >> 8);
                                                    ptr += 2 * (vi.channels);
                                                }
                                            }

                                            if (clipflag)
                                            {
                                                //s_err.WriteLine("Clipping in frame "+vd.sequence);
                                            }

                                            output.Write(convbuffer, 0, 2 * vi.channels * bout);

                                            vd.synthesis_read(bout); // tell libvorbis how
                                            // many samples we
                                            // actually consumed
                                        }
                                    }
                                }
                                if (og.eos() != 0) eos = 1;
                            }
                        }
                        if (eos == 0)
                        {
                            index = oy.buffer(4096);
                            buffer = oy.data;
                            try
                            {
                                bytes = input.Read(buffer, index, 4096);
                            }
                            catch (Exception e)
                            {
                                System.Diagnostics.Debug.WriteLine(e);
                            }
                            oy.wrote(bytes);
                            if (bytes == 0) eos = 1;
                        }
                    }

                    // clean up this logical bitstream; before exit we see if we're
                    // followed by another [chained]

                    os.clear();

                    // ogg_page and ogg_packet structs always point to storage in
                    // libvorbis.  They're never freed or manipulated directly

                    vb.clear();
                    vd.clear();
                    vi.clear();  // must be called last
                }

                // OK, clean up the framer
                oy.clear();
                System.Diagnostics.Debug.WriteLine("Done.");

                output.Seek(0, SeekOrigin.Begin);
                if (!skipWavHeader)
                {
                    WriteHeader(output, (int)(output.Length - HEADER_SIZE), vi.rate, (ushort)16, (ushort)vi.channels);
                    output.Seek(0, SeekOrigin.Begin);
                }
                */
            #endregion
            //
            ALFormat alf = 0;
            if (/*vi*/info.channels == 1) // mono
            {
                alf = ALFormat.Mono16;
            }
            else if (/*vi*/info.channels == 2) // sterio
            {
                alf = ALFormat.Stereo16;
            }
            else if (alf == 0)
            {
                throw new Exception("Wrong number of channels in sound file.");
            }

            /*BinaryReader bw = new BinaryReader(output);
            byte[] data = bw.ReadBytes((int)output.Length);
            bw.Close();
            bw.Dispose();*/
            /*byte[] data = new byte[(int)output.Length];
            output.Read(data, 0, data.Length);
            output.Close();
            output.Dispose();*/
            AL.BufferData(ab, alf, data2, data2.Length, /*vi.rate*/ info.rate);
            //data = null;
            //}

            //return ab;
        }

        public void LoadStreamOGG(SoundType sound)
        { 
            
        }

        private void WriteHeader(Stream stream, int length, int audioSampleRate, ushort audioBitsPerSample, ushort audioChannels)
        {
            int HEADER_SIZE = 36;
            BinaryWriter bw = new BinaryWriter(stream);

            bw.Write(new char[4] { 'R', 'I', 'F', 'F' });
            int fileSize = HEADER_SIZE + length;
            bw.Write(fileSize);
            bw.Write(new char[8] { 'W', 'A', 'V', 'E', 'f', 'm', 't', ' ' });
            bw.Write((int)16);
            bw.Write((short)1);
            bw.Write((short)audioChannels);
            bw.Write(audioSampleRate);
            bw.Write((int)(audioSampleRate * ((audioBitsPerSample * audioChannels) / 8)));
            bw.Write((short)((audioBitsPerSample * audioChannels) / 8));
            bw.Write((short)audioBitsPerSample);

            bw.Write(new char[4] { 'd', 'a', 't', 'a' });
            bw.Write(length);
            bw.Close();
        }

        /// <summary>
        /// Sets up a buffer from the Name and plays it
        /// </summary>
        /// <param name="Name">Name of the buffer</param>
        public void Play(string Name)
        {
            if (SoundList.ContainsKey(Name))
            {
                NextPlayingName = Name;
                //NextPlayingBuffer = SoundList[Name].Buffer; // fix me!!!
            }
        }

        public string PlayingName()
        {
            return NowPlayingName;
        }

        /*
        public int PlayingBuffer()
        {
            return NowPlayingBuffer;
        }*/
    } // end class
}
