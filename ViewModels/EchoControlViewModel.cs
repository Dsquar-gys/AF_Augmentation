using AudioEffects;
using AudioEffects.Effects;
using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace AF_Augmentation.ViewModels
{
    public partial class EchoControlViewModel : EffectViewModel
    {
        #region Override Members

        public override IEffect CreateEffect()
        {
            double delayValue;
            int repetitionsValue;
            try
            {
                delayValue = Delay is null ? 0 : Convert.ToDouble(Delay.Replace('.', ','));
                repetitionsValue = Repetitions is null ? 0 : (int)Convert.ToDouble(Repetitions.Replace('.', ','));
            }
            catch (Exception)
            {
                WindowController.Instance.ActiveText = "Wrong delay or repetitions number";
                delayValue = 0;
                repetitionsValue = 0;
            }

            return new Echo(delayValue, Decay, repetitionsValue);
        }

        #endregion
        #region Internal Parameters

        [ObservableProperty]
        private string? delay;

        [ObservableProperty]
        private double decay;

        [ObservableProperty]
        private string? repetitions;

        #endregion
    }
}
