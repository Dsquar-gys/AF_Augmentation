using AF_Augmentation.Controls;
using AF_Augmentation.Models;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using OptionsHandler;
using OptionsHandler.Effects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AF_Augmentation.ViewModels
{
    public class WindowController : ObservableObject
    {
        public List<string> BaseFiles { get; set; }
        public List<string> AmbientFiles { get; set; }
        public string ResultPath { get; set; }

        public async Task SelectBaseFolderAsync()
        {
            BaseFiles = await Controller.SetBaseFolderAsync();
            MainWindow.Instance.UpdateBaseStack(BaseFiles);
        }

        public async Task SelectAmbientFolderAsync()
        {
            AmbientFiles = await Controller.SetAmbientFolderAsync();
            MainWindow.Instance.UpdateAmbientStack(AmbientFiles);
        }

        public void SelectResultFolder()
        {
            ResultPath = Controller.SetResultFolder();
            MainWindow.Instance.UpdateResultPath(ResultPath);
        }

        public void AddOption() => MainWindow.Instance.AddOption();
        public void DeleteOption(int index) => MainWindow.Instance.DeleteOption(index);
        public void SwitchRadio(OptionsElementControl control) => control.SwitchRadio();
        public void CommitChange(OptionsElementControl control)
        {
            control.Active = !control.Active;

            IEffect command;
            var s = control.EffectSelector.SelectedItem as ComboBoxItem;
            switch (s.Content)
            {
                case "Volume Up":
                    command = new VolumeMultiply(1.5f);
                    break;
                case "Volume Down":
                    command = new VolumeMultiply(0.5f);
                    break;
                case "Echo":
                    command = new Echo(20000, 0.5f, 5);
                    break;
                default: throw new System.Exception("WindowController_CommitChange_CommandError");

            }

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

        public async Task RunApplicationAsync() => await Controller.MixAsync();
    }
}
