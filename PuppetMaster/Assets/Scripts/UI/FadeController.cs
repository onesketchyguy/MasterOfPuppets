using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UIEffects
{
    public class FadeController : MonoBehaviour
    {
        [SerializeField] private Gradient fadeGradient = null;
        [SerializeField] private Graphic graphic = null;

        [SerializeField] private bool fadeOnStart = false;
        [SerializeField] private float fadeTime = 1.0f;

        private void OnValidate()
        {
            if (fadeTime < 0) fadeTime = 0;

            if (graphic == null)
                graphic = GetComponent<Graphic>();
            else
            {
                graphic.color = fadeGradient.Evaluate(1);
            }
        }

        private void Start()
        {
            if (fadeOnStart) Fade();
        }

        public void Fade(bool fadeIn = false)
        {
            if (fadeIn)
            {
                StartCoroutine(FadeIn());
            }
            else
            {
                StartCoroutine(FadeOut());
            }
        }

        public IEnumerator FadeIn()
        {
            yield return null;

            float value = 1;
            float time = 0;

            while (true)
            {
                graphic.color = fadeGradient.Evaluate(value);

                value *= time;
                time = Mathf.Clamp(time + Time.deltaTime, 0, fadeTime);

                yield return null;

                if (value <= 0) break;
            }
        }

        public IEnumerator FadeOut()
        {
            yield return null;

            float value = 1;
            float time = fadeTime;

            while (true)
            {
                graphic.color = fadeGradient.Evaluate(value);

                value *= time;
                time = Mathf.Clamp(time - Time.deltaTime, 0, fadeTime);

                yield return null;

                if (value <= 0) break;
            }
        }
    }
}