using AF_Augmentation.Models;
using AF_Augmentation.Views;
using System.Threading;

namespace AF_Augmentation.ViewModels
{
    public class FileOverwriteApprovalViewModel : ViewModelBase
    {
        private readonly Thread callbackThread;
        FileOverwriteApproval approvalWindow;
        public FileOverwriteApprovalViewModel(Thread thread)
        {
            callbackThread = thread;
            approvalWindow = new FileOverwriteApproval
            {
                DataContext = this
            };
            approvalWindow.Show();
        }
        public void Positive()
        {
            approvalWindow.closeable = true;
            Answer(true);
            callbackThread.Interrupt();
        }
        public void Negative()
        {
            approvalWindow.closeable = true;
            Answer(false);
            callbackThread.Interrupt();
        }
        private void Answer(bool answer)
        {
            approvalWindow.Close();
            Controller.overwriteApproval = answer;
        }
    }
}
