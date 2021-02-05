using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace PuppetMaster
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private Text scoreDisplay;
        [SerializeField] private string scorePrefix = "";
        [SerializeField] private string scoreSuffix = "";

        private int score;

        private int changeAmount;
        private long displayScore;

        private void Start()
        {
            ResetScore();
        }

        public void ResetScore()
        {
            ModifyScore(-score);
        }

        public void ModifyScore(int changeAmount)
        {
            this.changeAmount += changeAmount;

            StopAllCoroutines();
            StartCoroutine(ShowScore());
        }

        private IEnumerator ShowScore()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();

                // Show score
                var add = Mathf.CeilToInt(Mathf.Clamp(displayScore + changeAmount, -10, 10));

                changeAmount -= add;
                displayScore += add;

                scoreDisplay.text = $"{scorePrefix}{displayScore}{scoreSuffix}";

                if (changeAmount <= 0) break;
            }
        }
    }
}