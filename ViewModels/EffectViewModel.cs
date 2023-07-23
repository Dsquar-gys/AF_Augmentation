using AF_Augmentation.Models;
using AudioEffects;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AF_Augmentation.ViewModels
{
    public abstract partial class EffectViewModel : ViewModelBase
    {
        #region Abstract Members

        public abstract IEffect CreateEffect();

        #endregion
        #region Observable Properties

        [ObservableProperty]
        private bool baseToggle;

        [ObservableProperty]
        private bool ambientToggle;

        [ObservableProperty]
        private bool active;

        [ObservableProperty]
        private EffectViewModel instance;

        [ObservableProperty]
        private int index;

        #endregion
        #region Relay Commands

        [RelayCommand]
        private void DeleteOption(int index) => WindowController.Instance.DeleteOption(index);

        [RelayCommand]
        private void CommitChange()
        {
            Active ^= true;

            IEffect command = CreateEffect();

            if (!Active) // Register
            {
                if (AmbientToggle)
                    Controller.effectAmbient.Register(command);
                else Controller.effectBase.Register(command);
            }
            else // Revoke
            {
                if (AmbientToggle)
                    Controller.effectAmbient.Remove(command);
                else Controller.effectBase.Remove(command);
            }
        }

        #endregion
        #region Constructor

        public EffectViewModel()
        {
            var controlsAmount = MainWindow.Instance.OptionsGrid.ItemCount;
            Index = controlsAmount > 0 ? controlsAmount : 0;
            Instance = this;
            Active = true;
            BaseToggle = false;
            AmbientToggle = true;
        }

        #endregion
    }
}
