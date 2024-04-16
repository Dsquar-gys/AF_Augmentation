using AudioEffects;
using AudioEffects.Effects;
using System;
using ReactiveUI;

namespace AF_Augmentation.ViewModels
{
    public partial class EchoControlViewModel : EffectViewModel
    {
        private string? _delay;
        private double _decay;
        private string? _repetitions;

        public string? Delay
        {
            get => _delay;
            set => this.RaiseAndSetIfChanged(ref _delay, value);
        }
        
        public double Decay
        {
            get => _decay;
            set => this.RaiseAndSetIfChanged(ref _decay, value);
        }
        
        public string? Repetitions
        {
            get => _repetitions;
            set => this.RaiseAndSetIfChanged(ref _repetitions, value);
        }
        
        #region Override Members

        protected override IEffect CreateEffect()
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
    }
}
