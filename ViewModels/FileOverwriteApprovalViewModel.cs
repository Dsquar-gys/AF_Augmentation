﻿using AF_Augmentation.Models;
using AF_Augmentation.Views;
using System.Threading;

namespace AF_Augmentation.ViewModels
{
    public class FileOverwriteApprovalViewModel : ViewModelBase
    {
        private Thread callbackThread;
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
            Answer(true);
            callbackThread.Interrupt();
        }
        public void Negative()
        {
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