using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UIEffects;

namespace PuppetMaster.EscapeSequence
{
    public class EscapeSequenceGameManager : MonoBehaviour
    {
        public FadeController fadeController;

        public GameObject player;
        public GameObject hand;

        public bool gameOver = false;
        private bool restarting = false;

        public int levelToLoadIndex;

        public float progress;

        private AsyncOperation loadScene;

        public static Queue<AsyncOperation> operations = new Queue<AsyncOperation>();

        private void Start()
        {
            loadScene = SceneManager.LoadSceneAsync(levelToLoadIndex, LoadSceneMode.Single);
            loadScene.allowSceneActivation = false;

            operations.Enqueue(loadScene);
            StartCoroutine(LoadScene());
        }

        private void Update()
        {
            if (gameOver && !restarting)
            {
                StartCoroutine(HandleGameOverState());
            }
        }

        public void FinishLoad()
        {
            loadScene.allowSceneActivation = true;
        }

        public void SetGameOver(bool value)
        {
            gameOver = value;
        }

        private IEnumerator HandleGameOverState()
        {
            if (restarting) yield break;
            restarting = true;

            yield return fadeController.FadeOut();

            player.SetActive(false);
            hand.SetActive(false);

            yield return new WaitForEndOfFrame();

            player.SetActive(true);
            hand.SetActive(true);

            yield return fadeController.FadeIn();

            gameOver = false;
            restarting = false;
        }

        private IEnumerator LoadScene()
        {
            yield return null;

            float startCount = operations.Count;

            while (operations.Count > 0)
            {
                var task = operations.Dequeue();

                while (task.isDone == false)
                {
                    progress = task.progress / startCount;

                    yield return null;
                }

                yield return null;
            }
        }
    }
}