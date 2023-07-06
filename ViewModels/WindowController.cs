using AF_Augmentation.Controls;
using AF_Augmentation.Models;
using CommunityToolkit.Mvvm.ComponentModel;
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
        public void CommitChange(OptionsElementControl control) => control.Active = !control.Active;

        public async Task RunApplicationAsync() => await Controller.MixAsync();
    }
}
