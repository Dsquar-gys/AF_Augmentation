using AF_Augmentation.ViewModels;
using AudioEffects;
using Avalonia.Controls;
using Avalonia.Threading;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
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
        #region Private Members

        private static string sourceBase;
        private static string sourceAmbient;
        private static string resultDirectory;
        private static List<string> basePaths;
        private static List<string> ambientPaths;

        #endregion
        #region Public Properties

        public static bool overwriteApproval { get; set; }
        public static EffectStream effectBase { get; }
        public static EffectStream effectAmbient { get; }

        #endregion
        #region Constructor

        static Controller()
        {
            effectBase = new EffectStream();
            effectAmbient = new EffectStream();

            ambientPaths = new List<string>();
            basePaths = new List<string>();
        }

        #endregion
        #region Async Methods

        public static async Task<List<string>> SetBaseFolderAsync() =>
            await Task.Run(() => SetBaseFolder());

        public static async Task<List<string>> SetAmbientFolderAsync() =>
            await Task.Run(() => SetAmbientFolder());

        public static async Task<string> SetResultFolderAsync() =>
            await Task.Run(() => SetResultFolder());

        public static async Task MixAsync() => await Task.Run(() => Mix());

        #endregion
        #region Private Methods

        private static List<string> SetBaseFolder()
        {
            string? result = new OpenFolderDialog().ShowAsync(MainWindow.Instance).Result;
            if (result is not null)
            {
                sourceBase = result;
                basePaths = Directory.EnumerateFiles(sourceBase, "*.wav").ToList();
            }
            return basePaths;
        }
        private static List<string> SetAmbientFolder()
        {
            string? result = new OpenFolderDialog().ShowAsync(MainWindow.Instance).Result;
            if (result is not null)
            {
                sourceAmbient = result;
                ambientPaths = Directory.GetFiles(sourceAmbient, "*.wav").ToList();
            }
            return ambientPaths;
        }
        private static string SetResultFolder()
        {
            string? result = new OpenFolderDialog().ShowAsync(MainWindow.Instance).Result;
            if (result is not null)
                resultDirectory = result + '\\';
            return resultDirectory;
        }
        private static void Mix()
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

                        // Mixing
                        var mixer = CreateMixer(effectBase, effectAmbient);

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
                        WaveFileWriter.CreateWaveFile16(newFile, mixer);
                    }
                }
            ThreadPool.SetMaxThreads(ThreadPool.ThreadCount, ThreadPool.ThreadCount);
        }
        private static MixingSampleProvider CreateMixer(ISampleProvider first, ISampleProvider second)
        {
            var outFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, first.WaveFormat.Channels);

            var x = new MixingSampleProvider(outFormat);
            x.AddMixerInput(first);
            x.AddMixerInput(second);
            return x;
        }
        private static string ExtractResultPath(string firstFile, string secondFile)
        {
            return resultDirectory + firstFile.Substring(sourceBase.Length + 1, firstFile.Length - sourceBase.Length - 5) +
                '_' + secondFile.Substring(sourceAmbient.Length + 1, secondFile.Length - sourceAmbient.Length - 5) + ".wav";
        }

        #endregion
    }
}
