using Avalonia;
using Avalonia.Controls.Primitives;

namespace AF_Augmentation.ViewModels;

public class GridElementControl : TemplatedControl
{
    public static readonly StyledProperty<string> FileNameProperty = AvaloniaProperty.Register<GridElementControl, string>(
            nameof(FileName), "Example.wav");

    public string FileName
    {
        get => GetValue(FileNameProperty);
        set => SetValue(FileNameProperty, value);
    }
}