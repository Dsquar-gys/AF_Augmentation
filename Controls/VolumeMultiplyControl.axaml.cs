using AudioEffects;
using Avalonia.Controls;
using Avalonia;
using Avalonia.Controls.Primitives;
using AudioEffects.Effects;

namespace AF_Augmentation.Controls;

public class VolumeMultiplyControl : BaseOptionControl
{
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        VolumeSlider = e.NameScope.Find<Slider>("VolumeSlider");
    }

    public static readonly StyledProperty<Slider> VolumeSliderProperty = AvaloniaProperty.Register<VolumeMultiplyControl, Slider>(
                nameof(VolumeSlider));
    public Slider VolumeSlider
    {
        get => GetValue(VolumeSliderProperty);
        set => SetValue(VolumeSliderProperty, value);
    }

    public override IEffect CreateEffect() => new VolumeMultiply(VolumeSlider.Value);
}