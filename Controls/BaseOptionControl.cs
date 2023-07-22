using AudioEffects;
using Avalonia;
using Avalonia.Controls.Primitives;
using System;
using System.Collections.Generic;

namespace AF_Augmentation.Controls
{
    public abstract class BaseOptionControl : TemplatedControl
    {
        public static Dictionary<string, Func<BaseOptionControl>> ControlSelector;
        static BaseOptionControl()
        {
            // Must contain every effect control
            ControlSelector = new Dictionary<string, Func<BaseOptionControl>>
            {
                { "Echo", () => new EchoControl() },
                { "Volume Multiply", () => new VolumeMultiplyControl() }
            };
        }
        // +
        public BaseOptionControl()
        {
            //var controlsAmount = MainWindow.Instance.OptionsStackPanel.Children.Count;
            //Index = controlsAmount > 0 ? controlsAmount : 0;
            Instance = this;
            Active = true;

            BaseToggle = false;
            AmbientToggle = true;
        }
        // +
        public static readonly StyledProperty<int> IndexProperty = AvaloniaProperty.Register<BaseOptionControl, int>(
                nameof(Index));
        public int Index
        {
            get => GetValue(IndexProperty);
            set => SetValue(IndexProperty, value);
        }
        // +
        // For Switching Radio Base/Ambient | To refer proper control to a method from xaml
        public static readonly StyledProperty<BaseOptionControl> InstanceProperty = AvaloniaProperty.Register<BaseOptionControl, BaseOptionControl>(
                nameof(Instance));
        public BaseOptionControl Instance
        {
            get => GetValue(InstanceProperty);
            set => SetValue(InstanceProperty, value);
        }
        // +
        // Controls activity property
        public static readonly StyledProperty<bool> ActiveProperty = AvaloniaProperty.Register<BaseOptionControl, bool>(
                nameof(Active));
        public bool Active
        {
            get => GetValue(ActiveProperty);
            set => SetValue(ActiveProperty, value);
        }
        // +
        // For Base radio
        public static readonly StyledProperty<bool> BaseToggleProperty = AvaloniaProperty.Register<BaseOptionControl, bool>(
                nameof(BaseToggle));
        public bool BaseToggle
        {
            get => GetValue(BaseToggleProperty);
            set => SetValue(BaseToggleProperty, value);
        }
        // +
        // For Ambient radio
        public static readonly StyledProperty<bool> AmbientToggleProperty = AvaloniaProperty.Register<BaseOptionControl, bool>(
                nameof(AmbientToggle));
        public bool AmbientToggle
        {
            get => GetValue(AmbientToggleProperty);
            set => SetValue(AmbientToggleProperty, value);
        }
        // +
        public void SwitchRadio()
        {
            BaseToggle ^= true;
            AmbientToggle ^= true;
        }
        // +
        public abstract IEffect CreateEffect();
    }
}
