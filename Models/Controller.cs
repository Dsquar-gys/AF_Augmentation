using Avalonia.Controls;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AF_Augmentation.Models
{
    public static class Controller
    {
        private static string sourceBase;
        private static string sourceAmbient;
        private static string resultDirectory;
        static List<string> basePaths;
        static List<string> ambientPaths;

        public static List<string> SetBaseFolder()
        {
            string? result = new OpenFolderDialog().ShowAsync(MainWindow.Instance).Result;
            if (result is not null)
                sourceBase = result;
            basePaths = Directory.EnumerateFiles(sourceBase, "*.wav").ToList();
            return basePaths;
        }

        public static List<string> SetAmbientFolder()
        {
            string? result = new OpenFolderDialog().ShowAsync(MainWindow.Instance).Result;
            if (result is not null)
                sourceAmbient = result;
            ambientPaths = Directory.GetFiles(sourceAmbient, "*.wav").ToList();
            return ambientPaths;
        }

        public static string SetResultFolder()
        {
            string? result = new OpenFolderDialog().ShowAsync(MainWindow.Instance).Result;
            if (result is not null)
                resultDirectory = result + '\\';
            return resultDirectory;
        }

        #region Async Setters

        public static async Task<List<string>> SetBaseFolderAsync() =>
            await Task.Run(() => SetBaseFolder());

        public static async Task<List<string>> SetAmbientFolderAsync() =>
            await Task.Run(() => SetAmbientFolder());

        #endregion

        public static void Mix()
        {
            ThreadPool.SetMaxThreads(ThreadPool.ThreadCount, 10);
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
            ThreadPool.SetMaxThreads(ThreadPool.ThreadCount, ThreadPool.ThreadCount);
        }

        public static async Task MixAsync() => await Task.Run(() => Mix());

        private static string ExtractResultPath(string firstFile, string secondFile)
        {
            return resultDirectory + firstFile.Substring(sourceBase.Length + 1, firstFile.Length - sourceBase.Length - 5) +
                '_' + secondFile.Substring(sourceAmbient.Length + 1, secondFile.Length - sourceAmbient.Length - 5) + ".wav";
        }
    }
}
