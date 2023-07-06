using AF_Augmentation.Controls;
using Avalonia.Controls;
using System.Collections.Generic;
using System.Linq;

namespace AF_Augmentation
{
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }

        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
        }

        public void UpdateBaseStack(List<string> names)
        {
            BaseStackPanel.Children.Clear();
            foreach (string name in names)
                BaseStackPanel.Children.Add(new GridElementControl(name.Substring(name.LastIndexOf('\\') + 1)));
        }

        public void UpdateAmbientStack(List<string> names)
        {
            AmbientStackPanel.Children.Clear();
            foreach (string name in names)
                AmbientStackPanel.Children.Add(new GridElementControl(name.Substring(name.LastIndexOf('\\') + 1)));
        }

        public void UpdateResultPath(string path)
        {
            ResultFolderPath.FileName = path;
            ApplyButton.IsEnabled = true;
        }

        public void AddOption() => OptionsStackPanel.Children.Add(new OptionsElementControl());
        public void DeleteOption(int index)
        {
            var controls = MainWindow.Instance.OptionsStackPanel.Children;
            controls.RemoveAt(index);

            OptionsElementControl reference;

            // Updating indexes of the rest controls
            for (int i = 0; i < controls.Count; i++)
            {
                reference = controls[i] as OptionsElementControl;
                reference.Index = i;
            }
        }
    }
}