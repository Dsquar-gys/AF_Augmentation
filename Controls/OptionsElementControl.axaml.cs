using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace AF_Augmentation.Controls
{
    public class OptionsElementControl : TemplatedControl
    {
        public OptionsElementControl()
        {
            var controlsAmount = MainWindow.Instance.OptionsStackPanel.Children.Count;
            Index = controlsAmount > 0 ? controlsAmount : 0;
            Instance = this;
            Active = true;

            BaseToggle = false;
            AmbientToggle = true;
        }

        // ComboBox as a selector
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e) // After the control is completely created
        {
            EffectSelector = e.NameScope.Find<ComboBox>("EffectSelector");
        }
        public static readonly StyledProperty<ComboBox> EffectSelectorProperty = AvaloniaProperty.Register<OptionsElementControl, ComboBox>(
                nameof(EffectSelector));
        public ComboBox EffectSelector
        {
            get => GetValue(EffectSelectorProperty);
            private set => SetValue(EffectSelectorProperty, value);
        }

        // Index of the instance on the stack(grid)
        public static readonly StyledProperty<int> IndexProperty = AvaloniaProperty.Register<OptionsElementControl, int>(
                nameof(Index));
        public int Index
        {
            get => GetValue(IndexProperty);
            set => SetValue(IndexProperty, value);
        }

        public static readonly StyledProperty<OptionsElementControl> InstanceProperty = AvaloniaProperty.Register<OptionsElementControl, OptionsElementControl>(
                nameof(Instance));
        public OptionsElementControl Instance
        {
            get => GetValue(InstanceProperty);
            set => SetValue(InstanceProperty, value);
        }

        // Controls activity property
        public static readonly StyledProperty<bool> ActiveProperty = AvaloniaProperty.Register<OptionsElementControl, bool>(
                nameof(Active));
        public bool Active
        {
            get => GetValue(ActiveProperty);
            set => SetValue(ActiveProperty, value);
        }

        // For Base radio
        public static readonly StyledProperty<bool> BaseToggleProperty = AvaloniaProperty.Register<OptionsElementControl, bool>(
                nameof(BaseToggle));
        public bool BaseToggle
        {
            get => GetValue(BaseToggleProperty);
            set => SetValue(BaseToggleProperty, value);
        }

        // For Ambient radio
        public static readonly StyledProperty<bool> AmbientToggleProperty = AvaloniaProperty.Register<OptionsElementControl, bool>(
                nameof(AmbientToggle));
        public bool AmbientToggle
        {
            get => GetValue(AmbientToggleProperty);
            set => SetValue(AmbientToggleProperty, value);
        }

        public void SwitchRadio()
        {
            BaseToggle = AmbientToggle;
            AmbientToggle = !AmbientToggle;
        }
    }
}