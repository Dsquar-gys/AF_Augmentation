using AF_Augmentation.Controls;
using AF_Augmentation.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ReactiveUI;

namespace AF_Augmentation.ViewModels
{
    public partial class WindowController : ReactiveObject
    {
        #region Private fields
        
        private string? _resultPath = "";
        private bool _shaded;
        private string _activeText = "";
        private bool _applyActivity;

        #endregion
        
        #region Properties
        
        public ObservableCollection<GridElementControl> BaseFiles { get; } = new();
        public ObservableCollection<GridElementControl> AmbientFiles { get; } = new();

        public string? ResultPath
        {
            get => _resultPath;
            set => this.RaiseAndSetIfChanged(ref _resultPath, value);
        }
        
        public bool Shaded
        {
            get => _shaded;
            set => this.RaiseAndSetIfChanged(ref _shaded, value);
        }
        
        public string ActiveText
        {
            get => _activeText;
            set => this.RaiseAndSetIfChanged(ref _activeText, value);
        }
        
        public bool ApplyActivity
        {
            get => _applyActivity;
            set => this.RaiseAndSetIfChanged(ref _applyActivity, value);
        }
        
        #endregion
        
        #region Window Controller Properties

        public static WindowController? Instance { get; set; }
        private static Dictionary<ChooseEffectButton, Func<EffectViewModel>> ControlSelector { get; }
        private ObservableCollection<EffectViewModel> ListOfEffects { get; } = new();

        #endregion
        
        #region Constructors

        static WindowController()
        {
            var parentType = typeof(EffectViewModel);
            var heirsList = Assembly.GetAssembly(parentType)
                                                  .GetTypes()
                                                  .Where(type => type.IsSubclassOf(parentType));

            ControlSelector = new();

            foreach (var heir in heirsList)
                ControlSelector.Add(new ChooseEffectButton { EffectName = heir.Name.Replace("ControlViewModel", "") },
                                    () => heir.GetConstructor(Type.EmptyTypes).Invoke(null) as EffectViewModel);
        }
        public WindowController() => Instance = this;

        #endregion
        
        #region Relay Commands

        public ReactiveCommand<Unit, Unit> SelectBaseFolderAsync => ReactiveCommand.CreateFromTask(async () =>
        {
            Shaded = true;
            BaseFiles.Clear();
            foreach (string name in await Controller.SetBaseFolderAsync())
                BaseFiles.Add(new GridElementControl(name.Substring(name.LastIndexOf('\\') + 1)));
            Shaded = false;

            await DisplayLogAsync("Base folder selected...");
            UpdateApplyButtonActivity();
        });

        public ReactiveCommand<Unit, Unit> SelectAmbientFolderAsync => ReactiveCommand.CreateFromTask(async () =>
        {
            Shaded = true;
            AmbientFiles.Clear();
            foreach (string name in await Controller.SetAmbientFolderAsync())
                AmbientFiles.Add(new GridElementControl(name.Substring(name.LastIndexOf('\\') + 1)));
            Shaded = false;

            await DisplayLogAsync("Ambient folder selected...");
            UpdateApplyButtonActivity();
        });

        public ReactiveCommand<Unit, Unit> SelectResultFolderAsync => ReactiveCommand.CreateFromTask(async () =>
        {
            ResultPath = await Controller.SetResultFolderAsync();
            await DisplayLogAsync("Result folder selected...");
            UpdateApplyButtonActivity();
        });

        public ReactiveCommand<ChooseEffectButton, Unit> AddOption => ReactiveCommand.Create<ChooseEffectButton>(
            sender =>
            {
                ListOfEffects.Add(ControlSelector[sender].Invoke());
                MainWindow.Instance.OptionSelectorPopup.ToggleOpenClose();
            });
        
        #endregion
        
        #region Public Methods

        public async Task DeleteOptionAsync(int index) =>
            await Task.Run(() => DeleteOption(index));

        #endregion
        
        #region Private Methods

        private void DisplayLog(string text)
        {
            ActiveText = "";
            foreach (var letter in text)
            {
                ActiveText += letter;
                Thread.Sleep(2);
            }
        }
        
        private async Task DisplayLogAsync(string text) => await Task.Run(() => DisplayLog(text));
        
        private void UpdateApplyButtonActivity() => ApplyActivity = BaseFiles is null || AmbientFiles is null ||
            ResultPath is null || BaseFiles.Count == 0 ||
            AmbientFiles.Count == 0 || ResultPath == "" ?
            false : true;

        private void DeleteOption(int index)
        {
            ListOfEffects.RemoveAt(index);

            //Updating indexes of the rest controls
            for (int i = 0; i < ListOfEffects.Count; i++)
                ListOfEffects[i].Index = i;
        }

        #endregion

        public async Task RunApplicationAsync()
        {

            await DisplayLogAsync("Mixing in progress...");
            Shaded = true;
            await Controller.MixAsync();
            Shaded = false;
            await DisplayLogAsync("Mixing is done!");
        }
    }
}
