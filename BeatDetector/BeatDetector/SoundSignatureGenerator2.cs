using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CenterSpace.NMath.Core;
using NAudio.Wave;

namespace BeatDetector
{
    public class SoundSignatureGenerator2
    {
        public static List<List<bool>> GetSignature(string musicPath, float beat, float globalThreshold)
        {
            // Extract data and sample rate from audio file
            float sampleRate = SoundSignatureGenerator2.GetMp3SampleRate(musicPath);
            float[] music = SoundSignatureGenerator2.GetRawMp3Frames(musicPath);

            // Generate signature

            return CreateSignature(music, beat, sampleRate, globalThreshold);


        }


        /** Create the music signature : a list of 2D points (list[0] is abscissa, list[1] is ordinate)
         */
        private static List<List<bool>> CreateSignature(float[] signal, float beat, float sampleFrequency, float globalThreshold)
        {
            // Variables
            int nFilter = 1;
            int n = signal.Length;
            float time = n / sampleFrequency;
            float barTime = 60f / beat;
            int nbBars = (int) (time / barTime);
            int nbSamplesPerBar = n / nbBars;
            float[] valuesT = new float[nbBars];

            float[] dSignal = new float[n-1];
            for (int i = 0; i < dSignal.Length; i++)
            {
                dSignal[i] = Math.Abs(signal[i + 1] - signal[i]);
            }

            for (int i = 0; i < nbBars; i++)
            {
                valuesT[i] = ((float) i * time) / ((float) (nbBars - 1));
            }

            float[] means = new float[nbBars];
            for (int i = 0; i < nbBars; i++)
            {
                float[] sub = new float[nbSamplesPerBar/nFilter];
                for (int j = 0; j < sub.Length; j++)
                {
                    sub[j] = Math.Abs(dSignal[j * nFilter + i * nbSamplesPerBar]);
                }
                double mean = 0;
                //for (int j = 0; j < nbSamplesPerBar; j++)
                //{
                //    mean += Math.Abs(signal[j + i * nbSamplesPerBar]);
                //}
                means[i] = Mean(sub);
            }

            float threshold = Mean(means); //STD(peaks);

            List<List<bool>> signature = new List<List<bool>>();
            for (int i = 0; i < nbBars; i++)
            {
                List<bool> l = new List<bool>();
                l.Add(means[i] > threshold);
                signature.Add(l);               
            }
                
            return signature;

        }

        /*************************
         ******* FILES ***********
         *************************/

        /** Return the sample rate (Hz)
         */
        private static float GetMp3SampleRate(string filename)
        {
            float sampleRate;
            using (MediaFoundationReader media = new MediaFoundationReader(filename))
            {
                sampleRate = media.WaveFormat.SampleRate;
            }

            return sampleRate;
        }

        /** Return an array with all the music frames
         */
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
                floatBuffer = new float[_floatBuffer_length / 2];

                for (int i = 0; i < _floatBuffer_length / 2; i++)
                {
                    floatBuffer[i] = _waveBuffer.FloatBuffer[i];
                }
            }

            return floatBuffer;
        }

        /*************************
         ******* Utils ***********
         *************************/

        /** Return the maximum of each column of the band
         * The first column of the return matrix contains the indices (int),
         * the second the values of the maximum
         */
        private static float[][] getLocalesMaximum(FloatComplex[][] band)
        {
            int n = band[0].Length;
            int sizeBand = band.Length;

            float[][] localesMax = new float[2][];
            localesMax[0] = new float[n];
            localesMax[1] = new float[n];

            for (int i = 0; i < n; i++)
            {
                int k = 0;
                int indMax = 0;
                float max = -1;
                while (k < sizeBand)
                {
                    if (Abs(band[k][i]) > max)
                    {
                        indMax = k;
                        max = Abs(band[k][i]);
                    }

                    k++;
                }

                localesMax[0][i] = indMax;
                localesMax[1][i] = max;
            }

            return localesMax;
        }

        /** Return the indice of the first number x in data such as x<value
         */
        private static int MinIndice(float[] data, float value)
        {
            int n = data.Length;
            int i = 0;
            int indMin = 0;
            bool isFound = false;

            while (i < n && !isFound)
            {
                if (data[i] > value)
                {
                    indMin = i;
                    isFound = true;
                }

                i++;
            }

            return indMin;
        }

        /** Mean
         */
        private static float Mean(float[] data)
        {
            return data.Sum() / data.Length;
        }

        /** Standard deviation 1D
         */
        private static float STD(float[] data)
        {
            double std = 0;
            if (data.Length > 0)
            {
                double avg = Mean(data);
                double sum = data.Sum(d => Math.Pow(d - avg, 2));
                std = Math.Sqrt((sum) / (data.Count() - 1));
            }

            return (float) std;

        }

        /** Complex abs : sqrt(x^2 + y^2)
         */
        private static float Abs(FloatComplex complex)
        {
            return (float) Math.Sqrt(Math.Pow(complex.Real, 2) + Math.Pow(complex.Imag, 2));
        }


    }
}