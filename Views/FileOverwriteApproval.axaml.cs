using Avalonia.Controls;

namespace AF_Augmentation.Views;

public partial class FileOverwriteApproval : Window
{
    public FileOverwriteApproval()
    {
        InitializeComponent();
    }

    // Unable to close window
    protected override bool HandleClosing() => true;
}