using Avalonia.Controls;
using System.Runtime.InteropServices;
using System;

namespace AF_Augmentation.Views;

public partial class FileOverwriteApproval : Window
{
    public bool closeable = false;
    public FileOverwriteApproval()
    {
        InitializeComponent();
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        e.Cancel = !closeable;
    }
}