using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using AF_Augmentation.ViewModels;

namespace AF_Augmentation.Models
{
    public static class Controller
    {
        private static string sourceBase;
        private static string sourceAmbient;
        private static string resultDirectory;
        static List<string> basePaths = new List<string>();
        static List<string> ambientPaths = new List<string>();

        static Controller()
        {
            var source = Path.GetFullPath(Directory.GetCurrentDirectory() + "\\..\\..\\..\\Source");
            sourceBase = source + "\\Base";
            sourceAmbient = source + "\\Ambient";
            resultDirectory = source + "\\Result\\";
            basePaths = Directory.GetFiles(sourceBase).ToList();
            ambientPaths = Directory.GetFiles(sourceAmbient).ToList();
        }

        public static List<string> SetBaseFolder()
        {
            string? result = new OpenFolderDialog().ShowAsync(MainWindow.Instance).Result;
            if (result is not null)
                sourceBase = result;
            basePaths = Directory.GetFiles(sourceBase).ToList();
            return basePaths;
        }

        public static List<string> SetAmbientFolder()
        {
            string? result = new OpenFolderDialog().ShowAsync(MainWindow.Instance).Result;
            if (result is not null)
                sourceAmbient = result;
            ambientPaths = Directory.GetFiles(sourceAmbient).ToList();
            return ambientPaths;
        }

        public static void SetResultFolder()
        {
            throw new NotImplementedException();
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
            return resultDirectory + firstFile.Substring(sourceBase.Length + 1, firstFile.Length - sourceBase.Length - 5) +
                '_' + secondFile.Substring(sourceAmbient.Length + 1, secondFile.Length - sourceAmbient.Length - 5) + ".wav";
        }
    }
}
