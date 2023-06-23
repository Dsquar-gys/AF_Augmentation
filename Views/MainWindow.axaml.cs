using AF_Augmentation.ViewModels;
using Avalonia.Controls;
using System.Collections.Generic;

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
    }
}