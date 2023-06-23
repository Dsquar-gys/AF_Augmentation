using AF_Augmentation.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace AF_Augmentation.ViewModels
{
    public class WindowController : ObservableObject
    {
        public List<string> BaseFiles { get; set; }
        public List<string> AmbientFiles { get; set; }
        public string ResultPath { get; set; }

        public void SelectBaseFolder()
        {
            BaseFiles = Controller.SetBaseFolder();
            MainWindow.Instance.UpdateBaseStack(BaseFiles);
        }

        public void SelectAmbientFolder()
        {
            AmbientFiles = Controller.SetAmbientFolder();
            MainWindow.Instance.UpdateAmbientStack(AmbientFiles);
        }

        public void SelectResultFolder()
        {
            ResultPath = Controller.SetResultFolder();
            MainWindow.Instance.UpdateResultPath(ResultPath);
        }

        public void RunApplication() => Controller.Mix();
    }
}
