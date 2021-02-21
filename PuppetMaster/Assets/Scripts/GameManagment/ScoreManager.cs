using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PuppetMaster
{
    public class ScoreManager : MonoBehaviour
    {
        [Tooltip("Text object to use to display the score.")]
        [SerializeField] private Text scoreDisplay = null;

        [Tooltip("Text to appear before the score itself.")]
        [SerializeField] private string scorePrefix = "";

        [Tooltip("Text to appear after the score itself.")]
        [SerializeField] private string scoreSuffix = "";

        /// <summary>
        /// The current value being used to modify the score.
        /// </summary>
        private int changeAmount;

        /// <summary>
        /// The current value being displayed.
        /// </summary>
        private long displayScore;

        private void OnEnable()
        {
            ResetScore();
        }

        /// <summary>
        /// Sets the displayed score to zero.
        /// </summary>
        public void ResetScore()
        {
            // Reset the score value
            displayScore = 0;
            changeAmount = 0;

            // Start displaying the score to the user.
            StopAllCoroutines();
            StartCoroutine(ShowScore());
        }

        /// <summary>
        /// Returns the current score value.
        /// </summary>
        /// <returns></returns>
        public long GetScore()
        {
            return displayScore + changeAmount;
        }

        /// <summary>
        /// Modifies the score value with a given modifier.
        /// </summary>
        /// <param name="changeAmount"></param>
        public void ModifyScore(int changeAmount)
        {
            // Add the change to the change amount value.
            this.changeAmount += changeAmount;

            // Start displaying the score to the user.
            StopAllCoroutines();
            StartCoroutine(ShowScore());
        }

        /// <summary>
        /// Starts the process of showing the score.
        /// </summary>
        /// <returns></returns>
        private IEnumerator ShowScore()
        {
            while (true)
            {
                // Yield a frame as though this were running on the main thread.
                yield return new WaitForEndOfFrame();

                // Find out how much to move this frame.
                var moveAmount = Mathf.CeilToInt(changeAmount * 0.1f);

                // Reflect the movement.
                changeAmount -= moveAmount;
                displayScore += moveAmount;

                // Display the new score value.
                scoreDisplay.text = $"{scorePrefix}{displayScore}{scoreSuffix}";

                // If the change amount has emptied then we finished.
                if (changeAmount == 0) break;
            }
        }
    }
}