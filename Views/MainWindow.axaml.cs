using AF_Augmentation.Controls;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace AF_Augmentation
{
    public partial class MainWindow : Window
    {
        #region Private Members

        private Control mOptionSelectorPopup;
        private Control mMainGrid;
        private Control mOptionSelectorButton;

        #endregion

        public static MainWindow Instance { get; private set; }
        public Action<string> Logger;
        public MainWindow()
        {
            Instance = this;
            InitializeComponent();

            mOptionSelectorPopup = this.FindControl<Control>("OptionSelectorPopup") ?? throw new Exception("Can't find Option Selector Popup by name");
            mMainGrid = this.FindControl<Control>("MainGrid") ?? throw new Exception("Can't find Main Grid by name");
            mOptionSelectorButton = this.FindControl<Control>("OptionSelectorButton") ?? throw new Exception("Can't find Option Selector Button by name");

            Logger += DisplayLog;
            // Create buttons for avaliable effects
            foreach (string key in BaseOptionControl.ControlSelector.Keys)
                OptionsList.Children.Add(new ChooseEffectButton { EffectName = key });
        }

        public void UpdateBaseStack(List<string> names)
        {
            BaseStackPanel.Children.Clear();
            foreach (string name in names)
                BaseStackPanel.Children.Add(new GridElementControl(name.Substring(name.LastIndexOf('\\') + 1)));
        }

        private void DisplayLog(string message) => Instance.Log.Content = message;
        //{
        //    while(true)
        //    {
        //        Instance.Log.Content = "";
        //        foreach (var sym in message)
        //        {
        //            Instance.Log.Content += sym.ToString();
        //        }
        //    }
        //}
        //private async Task DisplayLogAsync(string message) => await Task.Run(() => DisplayLog(message));

        public void UpdateAmbientStack(List<string> names)
        {
            AmbientStackPanel.Children.Clear();
            foreach (string name in names)
                AmbientStackPanel.Children.Add(new GridElementControl(name.Substring(name.LastIndexOf('\\') + 1)));
        }
        public void UpdateApplyButtonActivity(bool applyActivity) => ApplyButton.IsEnabled = applyActivity;
        public void AddOption(BaseOptionControl control)
        {
            OptionSelectorPopup.ToggleOpenClose();
            OptionsStackPanel.Children.Add(control);
        }

        public void DeleteOption(int index)
        {
            var controls = MainWindow.Instance.OptionsStackPanel.Children;
            controls.RemoveAt(index);

            BaseOptionControl reference;

            // Updating indexes of the rest controls
            for (int i = 0; i < controls.Count; i++)
            {
                reference = controls[i] as BaseOptionControl;
                reference.Index = i;
            }
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            var position = mOptionSelectorButton.TranslatePoint(new Point(), mMainGrid) ??
                throw new Exception("Can't get Translation Point from Option Selector Button");

            mOptionSelectorPopup.Margin = new Thickness(
                position.X,
                position.Y + mOptionSelectorButton.Bounds.Height,
                0,
                0);
        }
    }
}