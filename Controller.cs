using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AF_Augmentation
{
    internal static class Controller
    {
        private static string source = "..\\..\\..\\Source";
        static List<string> basePaths = new List<string>();
        static List<string> ambientPaths = new List<string>();

        static Controller()
        {
            basePaths = Directory.GetFiles(source + "\\Base").ToList();
            ambientPaths = Directory.GetFiles(source + "\\Ambient").ToList();
        }

        public static void Mix()
        {
            foreach (var baseAudioPath in basePaths)
                foreach (var ambientAudioPath in ambientPaths)
                {
                    using (var reader1 = new AudioFileReader(baseAudioPath))
                    using (var reader2 = new AudioFileReader(ambientAudioPath))
                    {
                        var mixer = new MixingSampleProvider(new[] { reader1, reader2 });
                        WaveFileWriter.CreateWaveFile16(ExtractResultPath(reader1.FileName, reader2.FileName), mixer);
                    }
                }
        }

        private static string ExtractResultPath(string firstFile, string secondFile)
        {
            return source + "\\Result\\" + firstFile.Substring(20, firstFile.Length - 24) + '_' + secondFile.Substring(24, secondFile.Length - 28) + ".wav";
        }
    }
}
