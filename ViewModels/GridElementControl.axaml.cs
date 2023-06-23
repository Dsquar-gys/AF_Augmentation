using Avalonia;
using Avalonia.Controls.Primitives;

namespace AF_Augmentation.ViewModels;

public class GridElementControl : TemplatedControl
{
    public GridElementControl() : this("Example.wav")
    { }
    public GridElementControl(string fileName) => FileName = fileName;

    public static readonly StyledProperty<string> FileNameProperty = AvaloniaProperty.Register<GridElementControl, string>(
            nameof(FileName));

    public string FileName
    {
        get => GetValue(FileNameProperty);
        set => SetValue(FileNameProperty, value);
    }
}