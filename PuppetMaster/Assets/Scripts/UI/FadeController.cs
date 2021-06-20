using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CustomUI
{
    public class FadeController : MonoBehaviour
    {
        [SerializeField] private Graphic graphic = null;

        [SerializeField] private bool fadeOnStart = false;
        [SerializeField] private float fadeTime = 1.0f;

        [SerializeField] private Color color;

        private void OnValidate()
        {
            if (fadeTime < 0) fadeTime = 0;

            if (graphic == null)
                graphic = GetComponent<Graphic>();
            else
            {
                graphic.color = Color.clear;
            }
        }

        private void Start()
        {
            graphic.color = color;

            if (fadeOnStart) Fade();
        }

        public void Fade(bool fadeIn = false)
        {
            if (fadeIn)
            {
                StartCoroutine(FadeIn(fadeTime));
            }
            else
            {
                StartCoroutine(FadeOut(fadeTime));
            }
        }

        public IEnumerator FadeIn(float fadeTime)
        {
            yield return null;

            while (true)
            {
                graphic.color = Color.Lerp(graphic.color, color, fadeTime * Time.deltaTime);

                yield return null;

                if (graphic.color.a >= color.a - 20) break;
            }

            graphic.color = color;
        }

        public IEnumerator FadeOut(float fadeTime)
        {
            yield return null;

            while (true)
            {
                graphic.color = Color.Lerp(graphic.color, Color.clear, fadeTime * Time.deltaTime);

                yield return null;

                if (graphic.color.a <= 10) break;
            }

            graphic.color = Color.clear;
        }
    }
}