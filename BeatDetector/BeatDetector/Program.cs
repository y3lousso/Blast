using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BeatDetector
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            BPMTFF bpmtff = new BPMTFF();
            SoundSignature soundSignature = new SoundSignature();
            DWTBeatDetector dwtBeatDetector = new DWTBeatDetector();

            //string path = "music/insane.mp3";
            string path = "C:\\Mockup\\8INF955_Projet\\BeatDetector\\BeatDetector\\music\\insane.mp3";
            string output = "C:\\Mockup\\8INF955_Projet\\BeatDetector\\BeatDetector\\signatures\\insane.txt";
            
            float sampleRate = GetMp3SampleRate(path);
            float[] music = music2(path);

            /* sound signature */
            SoundSignatureFileManager.SaveSoundSignature(output, SoundSignatureGenerator.GetSignature(path, 160));
            //List<List<bool>> signature = SoundSignatureGenerator.GetSignature(path, 175);   
            //SoundSignatureFileManager.SaveSoundSignature("music/text.txt", signature);
            //List<List<bool>> signature2 = SoundSignatureFileManager.LoadSoundSignature("music/text.txt");
            //Console.WriteLine(signature2 == signature);

            /* Beat detector
            int windowTime = 4;
            int nbFrames = (int)(music.Length / windowTime / sampleRate)
            float[] beat = new float[nbFrames];
            for (int i = 0; i < nbFrames-1; i++)
            {
                float[] music2 = new float[(int)(windowTime * sampleRate)];
                for (int j = 0; j < music2.Length; j++)
                {
                    music2[j] = music[(int) (j + i * windowTime * sampleRate)];
                }
                beat[i] = dwtBeatDetector.Beat(music2);
                Console.WriteLine(beat[i]);
            }
            */


            /* BPMTFF
            float[] music = GetRawMp3Frames(path);
            float[] sample = bpmtff.CreateS[iampling(music);


            float[][] fftsignal = bpmtff.FilterBank(sample);
            float[][] fftsignal2 = bpmtff.Hwindow(fftsignal);
            float[][] fftsignal3 = bpmtff.DiffRect(fftsignal2);
            //float[] bpms = bpmtff.TimeComb(fftsignal3);


            */


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 form = new Form1();

            //form.plotGraph2(signature);
            Application.Run(form);
            
            
        }



        public static float GetMp3SampleRate(string filename)
        {
            float sampleRate;
            using (MediaFoundationReader media = new MediaFoundationReader(filename))
            {
                sampleRate = media.WaveFormat.SampleRate;
            }
            return sampleRate;
        }


        private static float[] GetRawMp3Frames(string filename)
        {

            float[] floatBuffer;
            using (MediaFoundationReader media = new MediaFoundationReader(filename))
            {
                int _byteBuffer32_length = (int)media.Length * 2;
                int _floatBuffer_length = _byteBuffer32_length / sizeof(float);

                IWaveProvider stream32 = new Wave16ToFloatProvider(media);
                WaveBuffer _waveBuffer = new WaveBuffer(_byteBuffer32_length);
                stream32.Read(_waveBuffer, 0, (int)_byteBuffer32_length);
                floatBuffer = new float[_floatBuffer_length/2];

                for (int i = 0; i < _floatBuffer_length/2; i++)
                {
                    floatBuffer[i] = (_waveBuffer.FloatBuffer[i]);
                }

                //float max = floatBuffer.Max();
                //for (int i = 0; i < floatBuffer.Length; i++)
                //    floatBuffer[i] = floatBuffer[i] / max;
            }

            return floatBuffer;
        }

        private static float[] music2(string filename)
        {
            float[] floatBuffer;
            using (MemoryStream output = new MemoryStream())
            {
                Mp3FileReader reader = new Mp3FileReader(filename);
                Mp3Frame frame;
                while ((frame = reader.ReadNextFrame()) != null)
                {
                    output.Write(frame.RawData, 0, frame.RawData.Length);
                }

                byte[] outputList = output.ToArray();
                floatBuffer = new float[output.Length];

                for (int i = 0; i < output.Length; i++)
                {
                    floatBuffer[i] = outputList[i];
                }
            }

            return floatBuffer;
        }
    }
}

