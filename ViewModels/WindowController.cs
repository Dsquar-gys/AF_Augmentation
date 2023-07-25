using AF_Augmentation.Controls;
using AF_Augmentation.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AF_Augmentation.ViewModels
{
    public partial class WindowController : ObservableObject
    {
        #region Observable Properties

        [ObservableProperty]
        private ObservableCollection<GridElementControl> baseFiles = new();
        [ObservableProperty]
        private ObservableCollection<GridElementControl> ambientFiles = new();
        [ObservableProperty]
        private string? resultPath = "";
        [ObservableProperty]
        private bool shaded = false;
        [ObservableProperty]
        private string activeText = "";
        [ObservableProperty]
        private bool applyActivity = false;

        #endregion
        #region Window Controller Properties

        public static WindowController? Instance { get; set; }
        public static Dictionary<ChooseEffectButton, Func<EffectViewModel>> ControlSelector { get; }
        private ObservableCollection<EffectViewModel> ListOfEffects { get; } = new();

        #endregion
        #region Constructors

        static WindowController()
        {
            Type parentType = typeof(EffectViewModel);
            IEnumerable<Type> heirsList = Assembly.GetAssembly(parentType)
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

        [RelayCommand]
        private async Task SelectBaseFolderAsync()
        {
            Shaded = true;
            BaseFiles.Clear();
            foreach (string name in await Controller.SetBaseFolderAsync())
                BaseFiles.Add(new GridElementControl(name.Substring(name.LastIndexOf('\\') + 1)));
            Shaded = false;

            ActiveText = "Base folder selected...";
            UpdateApplyButtonActivity();
        }
        [RelayCommand]
        private async Task SelectAmbientFolderAsync()
        {
            Shaded = true;
            AmbientFiles.Clear();
            foreach (string name in await Controller.SetAmbientFolderAsync())
                AmbientFiles.Add(new GridElementControl(name.Substring(name.LastIndexOf('\\') + 1)));
            Shaded = false;

            ActiveText = "Ambient folder selected...";
            UpdateApplyButtonActivity();
        }
        [RelayCommand]
        private async Task SelectResultFolder()
        {
            ResultPath = await Controller.SetResultFolderAsync();
            ActiveText = "Result folder selected...";
            UpdateApplyButtonActivity();
        }
        [RelayCommand]
        private void AddOption(ChooseEffectButton sender)
        {
            ListOfEffects.Add(ControlSelector[sender].Invoke());
            MainWindow.Instance.OptionSelectorPopup.ToggleOpenClose();
        }

        public void DeleteOption(int index)
        {
            ListOfEffects.RemoveAt(index);

            //Updating indexes of the rest controls
            for (int i = 0; i < ListOfEffects.Count; i++)
                ListOfEffects[i].Index = i;
        }

        #endregion
        #region Private Methods

        private void UpdateApplyButtonActivity() => ApplyActivity = BaseFiles is null || AmbientFiles is null ||
            resultPath is null || BaseFiles.Count == 0 ||
            AmbientFiles.Count == 0 || resultPath == "" ?
            false : true;

        #endregion

        public async Task RunApplicationAsync()
        {
            ActiveText = "Mixing in progress...";
            Shaded = true;
            await Controller.MixAsync();
            Shaded = false;
            ActiveText = "Mixing is done!";
        }
    }
}
