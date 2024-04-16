using System.Reactive;
using AF_Augmentation.Models;
using AF_Augmentation.Views;
using System.Threading;
using ReactiveUI;

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

        public ReactiveCommand<Unit, Unit> Positive => ReactiveCommand.Create(() => Answer(true));
        public ReactiveCommand<Unit, Unit> Negative => ReactiveCommand.Create(() => Answer(false));

        #endregion
    }
}
