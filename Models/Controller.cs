using AF_Augmentation.ViewModels;
using AudioEffects;
using Avalonia.Controls;
using Avalonia.Threading;
using NAudio.Wave;
using System;
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

        public static bool overwriteApproval { get; set; }
        public static EffectStream effectBase { get; }
        public static EffectStream effectAmbient { get; }

        static Controller()
        {
            effectBase = new EffectStream();
            effectAmbient = new EffectStream();

            ambientPaths = new List<string>();
            basePaths = new List<string>();
        }

        public static List<string> SetBaseFolder()
        {
            string? result = new OpenFolderDialog().ShowAsync(MainWindow.Instance).Result;
            if (result is not null)
            {
                sourceBase = result;
                basePaths = Directory.EnumerateFiles(sourceBase, "*.wav").ToList();
            }
            return basePaths;
        }

        public static List<string> SetAmbientFolder()
        {
            string? result = new OpenFolderDialog().ShowAsync(MainWindow.Instance).Result;
            if (result is not null)
            {
                sourceAmbient = result;
                ambientPaths = Directory.GetFiles(sourceAmbient, "*.wav").ToList();
            }
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

        public static async Task<string> SetResultFolderAsync() =>
            await Task.Run(() => SetResultFolder());

        #endregion

        public static void Mix()
        {
            overwriteApproval = false;
            ThreadPool.SetMaxThreads(ThreadPool.ThreadCount, 10);

            foreach (var baseAudioPath in basePaths)
                foreach (var ambientAudioPath in ambientPaths)
                {
                    using (var reader1 = new WaveFileReader(baseAudioPath))
                    using (var reader2 = new WaveFileReader(ambientAudioPath))
                    {
                        effectBase.SetSource(reader1);
                        effectAmbient.SetSource(reader2);

                        BlockAlignReductionStream processedBase = new BlockAlignReductionStream(effectBase);
                        BlockAlignReductionStream processedAmbient = new BlockAlignReductionStream(effectAmbient);

                        // Mixing
                        var mixer = CreateMixer(processedBase, processedAmbient);
                        // Output
                        var newFile = ExtractResultPath(baseAudioPath, ambientAudioPath);
                        // If the file already exists...
                        if (Directory.GetFiles(resultDirectory).Contains(newFile) && !overwriteApproval)
                        {
                            // Remember the current thread
                            var approvalThread = Thread.CurrentThread;

                            // Through the UI thread the approval window appears
                            Dispatcher.UIThread.InvokeAsync(() => { new FileOverwriteApprovalViewModel(approvalThread); });

                            // Trying to make current thread sleeping for a while
                            try
                            {
                                Thread.Sleep(Timeout.Infinite);
                            }
                            // If operator approve overwriting, nothing happens, otherwise method returns
                            catch (ThreadInterruptedException e)
                            {
                                if (!overwriteApproval)
                                    return;
                            }
                        }
                        WaveFileWriter.CreateWaveFile(newFile, mixer);
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
        public static MixingWaveProvider32 CreateMixer(BlockAlignReductionStream first, BlockAlignReductionStream second)
        {
            var outFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, first.WaveFormat.Channels);
            using (var resampler = new MediaFoundationResampler(first, outFormat))
            using (var resampler2 = new MediaFoundationResampler(second, outFormat))
            {
                return new MixingWaveProvider32(new[] { resampler, resampler2 });
            }
        }
    }
}
