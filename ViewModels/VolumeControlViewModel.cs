using AudioEffects;
using AudioEffects.Effects;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AF_Augmentation.ViewModels
{
    public partial class VolumeControlViewModel : EffectViewModel
    {
        #region Override Members

        public override IEffect CreateEffect() => new VolumeMultiply(MultiplyBy);

        #endregion
        #region Internal Parameters

        [ObservableProperty]
        private float multiplyBy = 0;

        #endregion
    }
}
