using AudioEffects;
using AudioEffects.Effects;
using ReactiveUI;

namespace AF_Augmentation.ViewModels
{
    public partial class VolumeControlViewModel : EffectViewModel
    {
        private float _multiplyBy;

        public float MultiplyBy
        {
            get => _multiplyBy;
            set => this.RaiseAndSetIfChanged(ref _multiplyBy, value);
        }
        
        #region Override Members

        protected override IEffect CreateEffect() => new VolumeMultiply(MultiplyBy);

        #endregion
    }
}
