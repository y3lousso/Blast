using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BeatDetector
{
    public class SoundSignatureFileManager
    {
        public static void SaveSoundSignature(string filepath, List<List<bool>> signature)
        {

            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
            FileStream fs = File.Create(filepath);
            fs.Close();

            using (var tw = new StreamWriter(filepath, true))
            {
                for (int i = 0; i < signature.Count; i++)
                {
                    tw.WriteLine(string.Join(",", signature[i]));
                }
            }
        }

        public static List<List<bool>> LoadSoundSignature(string filepath)
        {
            List<List<bool>> signature = new List<List<bool>>();
            if (!File.Exists(filepath))
            {
                throw new IOException("The file " + filepath + " doesn't exist !");
            }

            string[] file = File.ReadAllLines(filepath);
            foreach (string s in file)
            {
                    signature.Add(s.Split(',').Select(bool.Parse).ToList());
            }

            return signature;
        }
    }
}