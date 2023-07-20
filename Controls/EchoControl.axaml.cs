using AudioEffects;
using AudioEffects.Effects;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using System;

namespace AF_Augmentation.Controls;

public class EchoControl : BaseOptionControl
{
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        DelayTextBox = e.NameScope.Find<TextBox>("DelayTextBox");
        DecaySlider = e.NameScope.Find<Slider>("DecaySlider");
        RepetitionsTextBox = e.NameScope.Find<TextBox>("RepetitionsTextBox");
    }

    public static readonly StyledProperty<TextBox> DelayTextBoxProperty = AvaloniaProperty.Register<EchoControl, TextBox>(
                nameof(DelayTextBox));
    public TextBox DelayTextBox
    {
        get => GetValue(DelayTextBoxProperty);
        set => SetValue(DelayTextBoxProperty, value);
    }

    public static readonly StyledProperty<Slider> DecaySliderProperty = AvaloniaProperty.Register<EchoControl, Slider>(
                nameof(DecaySlider));
    public Slider DecaySlider
    {
        get => GetValue(DecaySliderProperty);
        set => SetValue(DecaySliderProperty, value);
    }

    public static readonly StyledProperty<TextBox> RepetitionsTextBoxProperty = AvaloniaProperty.Register<EchoControl, TextBox>(
                nameof(RepetitionsTextBox));

    public TextBox RepetitionsTextBox
    {
        get => GetValue(RepetitionsTextBoxProperty);
        set => SetValue(RepetitionsTextBoxProperty, value);
    } 

    public override IEffect CreateEffect()
    {
        double delay;
        double decay = DecaySlider.Value;
        int repetitions;
        try
        {
            delay = DelayTextBox.Text is null ? 0 : Convert.ToDouble(DelayTextBox.Text.Replace('.', ','));
            repetitions = RepetitionsTextBox.Text is null ? 0 : (int)Convert.ToDouble(RepetitionsTextBox.Text.Replace('.', ','));
        }
        catch (Exception)
        {
            delay = 0;
            repetitions = 0;
        }
        
        return new Echo(delay, decay, repetitions);
    }

}