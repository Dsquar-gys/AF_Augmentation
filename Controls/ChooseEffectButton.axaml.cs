using Avalonia.Controls;
using Avalonia;
using Avalonia.Controls.Primitives;

namespace AF_Augmentation.Controls;

public class ChooseEffectButton : TemplatedControl
{
    public ChooseEffectButton() => Instance = this;

    public static readonly StyledProperty<string> EffectNameProperty = AvaloniaProperty.Register<ChooseEffectButton, string>(
                nameof(EffectName));
    public string EffectName
    {
        get => GetValue(EffectNameProperty);
        set => SetValue(EffectNameProperty, value);
    }

    public static readonly StyledProperty<ChooseEffectButton> InstanceProperty = AvaloniaProperty.Register<ChooseEffectButton, ChooseEffectButton>(
                nameof(Instance));
    public ChooseEffectButton Instance
    {
        get => GetValue(InstanceProperty);
        set => SetValue(InstanceProperty, value);
    }
}