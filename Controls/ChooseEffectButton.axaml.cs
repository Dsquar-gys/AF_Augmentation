using Avalonia.Controls;
using Avalonia;
using Avalonia.Controls.Primitives;

namespace AF_Augmentation.Controls;

public class ChooseEffectButton : TemplatedControl
{
    public static readonly StyledProperty<string> EffectNameProperty = AvaloniaProperty.Register<ChooseEffectButton, string>(
                nameof(EffectName));
    public string EffectName
    {
        get => GetValue(EffectNameProperty);
        set => SetValue(EffectNameProperty, value);
    }
}