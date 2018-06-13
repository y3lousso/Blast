using NAudio.Wave;
using System;
using System.Collections.Generic;
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

            string path = "music/escape.mp3";
            float sampleRate = GetMp3SampleRate(path);
            float[] music = GetRawMp3Frames(path);


            /* sound signature */
            List<List<bool>> signature = SoundSignatureGenerator.GetSignature(path, 175);            

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


        public static float[] GetRawMp3Frames(string filename)
        {
            float[] floatBuffer;
            using (MediaFoundationReader media = new MediaFoundationReader(filename))
            {

                int byteBuffer32Length = (int) media.Length;
                int floatBufferLength = byteBuffer32Length / sizeof(float);

                IWaveProvider stream32 = new Wave16ToFloatProvider(media);
                WaveBuffer waveBuffer = new WaveBuffer(byteBuffer32Length);
                stream32.Read(waveBuffer, 0, byteBuffer32Length);
                floatBuffer = new float[floatBufferLength];

                for (int i = 0; i < floatBufferLength; i++)
                {
                    floatBuffer[i] = waveBuffer.FloatBuffer[i];
                }
            }
            return floatBuffer;
        }



    }
}

