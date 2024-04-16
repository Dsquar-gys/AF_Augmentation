using System.Reactive;
using AF_Augmentation.Models;
using AudioEffects;
using System.Threading.Tasks;
using ReactiveUI;

namespace AF_Augmentation.ViewModels
{
    public abstract class EffectViewModel : ViewModelBase
    {
        #region Abstract Members

        protected abstract IEffect CreateEffect();

        #endregion
        
        #region Private fields
        
        private bool _baseToggle;
        private bool _ambientToggle;
        private bool _active;
        private EffectViewModel _instance;
        private int _index;
        
        #endregion
        
        #region Properties


        public bool BaseToggle
        {
            get => _baseToggle;
            set => this.RaiseAndSetIfChanged(ref _baseToggle, value);
        }
        
        public bool AmbientToggle
        {
            get => _ambientToggle;
            set => this.RaiseAndSetIfChanged(ref _ambientToggle, value);
        }
        
        public bool Active
        {
            get => _active;
            set => this.RaiseAndSetIfChanged(ref _active, value);
        }
        
        public EffectViewModel Instance
        {
            get => _instance;
            set => this.RaiseAndSetIfChanged(ref _instance, value);
        }
        
        public int Index
        {
            get => _index;
            set => this.RaiseAndSetIfChanged(ref _index, value);
        }

        #endregion
        
        #region Commands

        public ReactiveCommand<int, Unit> DeleteOptionCommand => ReactiveCommand.CreateFromTask<int>(async index =>
            await WindowController.Instance.DeleteOptionAsync(index));

        public ReactiveCommand<Unit, Unit> CommitChangeCommand => ReactiveCommand.Create(() =>
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
        });
        
        #endregion
        
        #region Constructor

        protected EffectViewModel()
        {
            var controlsAmount = MainWindow.Instance.OptionsGrid.ItemCount;
            _index = controlsAmount > 0 ? controlsAmount : 0;
            _instance = this;
            _active = true;
            _baseToggle = false;
            _ambientToggle = true;
        }

        #endregion
    }
}
