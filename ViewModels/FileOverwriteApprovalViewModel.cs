using AF_Augmentation.Models;
using AF_Augmentation.Views;
using CommunityToolkit.Mvvm.Input;
using System.Threading;

namespace AF_Augmentation.ViewModels
{
    public partial class FileOverwriteApprovalViewModel : ViewModelBase
    {
        #region Private Members

        private readonly Thread callbackThread;
        private FileOverwriteApproval approvalWindow;

        private void Answer(bool answer)
        {
            approvalWindow.closeable = true;
            approvalWindow.Close();
            Controller.overwriteApproval = answer;
            callbackThread.Interrupt();
        }

        #endregion
        #region Constructor

        public FileOverwriteApprovalViewModel(Thread thread)
        {
            callbackThread = thread;
            approvalWindow = new FileOverwriteApproval
            {
                DataContext = this
            };
            approvalWindow.Show();
        }

        #endregion
        #region Relay Commands

        [RelayCommand]
        private void Positive() => Answer(true);
        [RelayCommand]
        private void Negative() => Answer(false);

        #endregion
    }
}
