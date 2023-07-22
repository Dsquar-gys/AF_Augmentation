using AF_Augmentation.Controls;
using AF_Augmentation.Models;
using AudioEffects;
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
        public List<string>? BaseFiles { get; set; }
        public List<string>? AmbientFiles { get; set; }
        public static WindowController? Instance { get; set; }
        public static Dictionary<ChooseEffectButton, Func<EffectViewModel>> ControlSelector { get; }
        public ObservableCollection<EffectViewModel> ListOfEffects { get; } = new();

        [ObservableProperty]
        private string? resultPath = "";

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
        public WindowController()
        {
            Instance = this;
        }

        #region Relay Commands

        [RelayCommand]
        private async Task SelectBaseFolderAsync()
        {
            BaseFiles = await Controller.SetBaseFolderAsync();
            MainWindow.Instance.UpdateBaseStack(BaseFiles);
            MainWindow.Instance.Logger.Invoke("Base folder selected...");
            UpdateApplyButtonActivity();
        }
        [RelayCommand]
        private async Task SelectAmbientFolderAsync()
        {
            AmbientFiles = await Controller.SetAmbientFolderAsync();
            MainWindow.Instance.UpdateAmbientStack(AmbientFiles);
            MainWindow.Instance.Logger.Invoke("Ambient folder selected...");
            UpdateApplyButtonActivity();
        }
        [RelayCommand]
        private void SelectResultFolder()
        {
            ResultPath = Controller.SetResultFolder();
            MainWindow.Instance.Logger.Invoke("Result folder selected...");
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
        //[RelayCommand]
        //private void CommitChange(BaseOptionControl control)
        //{
        //    control.Active = !control.Active;

        //    IEffect command = control.CreateEffect();

        //    if (!control.Active) // Register
        //    {
        //        if (control.AmbientToggle)
        //            Controller.effectAmbient.Register(command);
        //        else Controller.effectBase.Register(command);
        //    }
        //    else // Revoke
        //    {
        //        if (control.AmbientToggle)
        //            Controller.effectAmbient.Remove(command);
        //        else Controller.effectBase.Remove(command);
        //    }
        //}

        #endregion

        private void UpdateApplyButtonActivity()
        {
            bool applyActivate = BaseFiles is null || AmbientFiles is null ||
                                 resultPath is null || BaseFiles.Count == 0 ||
                                 AmbientFiles.Count == 0 || resultPath == "" ?
                                 false : true;
            MainWindow.Instance.UpdateApplyButtonActivity(applyActivate);
        }

        public async Task RunApplicationAsync()
        {
            await Controller.MixAsync();
            MainWindow.Instance.Logger.Invoke("Mixing in progress...");
        }
    }
}
