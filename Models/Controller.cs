using Avalonia.Controls;
using NAudio.Wave;
using OptionsHandler;
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

        public static EffectStream effectBase { get; }
        public static EffectStream effectAmbient { get; }

        static Controller()
        {
            effectBase = new EffectStream();
            effectAmbient = new EffectStream();
        }

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
                    using (var reader1 = new WaveFileReader(baseAudioPath))
                    using (var reader2 = new WaveFileReader(ambientAudioPath))
                    {
                        effectBase.SourceStream = reader1;
                        effectAmbient.SourceStream = reader2;

                        BlockAlignReductionStream processedBase = new BlockAlignReductionStream(effectBase);
                        BlockAlignReductionStream processedAmbient = new BlockAlignReductionStream(effectAmbient);

                        //effectAmbient.ClearEffects();
                        //effectAmbient.Register(new Echo(20000, 0.5f, 5));
                        //effectAmbient.Register(new VolumeMultiply(0.1f));
                        //effectAmbient.Register(new Echo(10000, 0.8f, 10));

                        // Mixing
                        var mixer = new MixingWaveProvider32(new[] { processedBase, processedAmbient });

                        // Output
                        WaveFileWriter.CreateWaveFile(ExtractResultPath(baseAudioPath, ambientAudioPath), mixer);
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

        public static void ChangeWaveFormat(string path)
        {
            using (var reader = new WaveFileReader(path))
            {
                var outFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, reader.WaveFormat.Channels);
                using (var resampler = new MediaFoundationResampler(reader, outFormat))
                {
                    WaveFileWriter.CreateWaveFile("test.wav", resampler);
                }
            }
        }
    }
}
