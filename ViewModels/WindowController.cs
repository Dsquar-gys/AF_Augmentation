using AF_Augmentation.Controls;
using AF_Augmentation.Models;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using AudioEffects;
using AudioEffects.Effects;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace AF_Augmentation.ViewModels
{
    public class EffectViewModel : ViewModelBase
    {

    }

    public class EffectViewModel1 : EffectViewModel
    {

    }

    public partial class WindowController : ObservableObject
    {
        public List<string> BaseFiles { get; set; }
        public List<string> AmbientFiles { get; set; }

        public ObservableCollection<EffectViewModel> ListOfEffects { get; } = new();

        [ObservableProperty]
        private string resultPath = "";

        public WindowController()
        {
            ListOfEffects.Add(new EffectViewModel1() );
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
        private void AddOption(string controlType)
        {
            if (BaseOptionControl.ControlSelector.ContainsKey(controlType))
                MainWindow.Instance.AddOption(BaseOptionControl.ControlSelector[controlType].Invoke());
        }
        [RelayCommand]
        private void DeleteOption(int index) => MainWindow.Instance.DeleteOption(index);
        [RelayCommand]
        private void SwitchRadio(BaseOptionControl control) => control.SwitchRadio();
        [RelayCommand]
        private void CommitChange(BaseOptionControl control)
        {
            control.Active = !control.Active;

            IEffect command = control.CreateEffect();

            if (!control.Active) // Register
            {
                if (control.AmbientToggle)
                    Controller.effectAmbient.Register(command);
                else Controller.effectBase.Register(command);
            }
            else // Revoke
            {
                if (control.AmbientToggle)
                    Controller.effectAmbient.Remove(command);
                else Controller.effectBase.Remove(command);
            }
        }

        #endregion

        private void UpdateApplyButtonActivity()
        {
            bool applyActivate = BaseFiles is null || AmbientFiles is null ||
                                 ResultPath is null || BaseFiles.Count == 0 ||
                                 AmbientFiles.Count == 0 || ResultPath == "" ?
                                 false : true;
            MainWindow.Instance.UpdateApplyButtonActivity(applyActivate);
        }

        

        public async Task RunApplicationAsync()
        {
            var tr = Thread.CurrentThread.ManagedThreadId;
            await Controller.MixAsync();
            MainWindow.Instance.Logger.Invoke("Mixing in progress...");
        }
    }
}
