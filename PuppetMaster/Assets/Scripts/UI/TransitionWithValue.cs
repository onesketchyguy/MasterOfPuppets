using UnityEngine;
using UnityEngine.UI;

namespace UIEffects
{
    public class TransitionWithValue : MonoBehaviour
    {
        public Gradient healthGradient;
        public Graphic graphic;

        public Slider slider;

        private float GetValue()
        {
            return slider.normalizedValue;
        }

        private void OnValidate()
        {
            if (graphic != null && slider != null)
                UpdateValue();
        }

        private void Update()
        {
            UpdateValue();
        }

        private void UpdateValue()
        {
            graphic.color = healthGradient.Evaluate(GetValue());
        }
    }
}