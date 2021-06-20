using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CustomUI;

namespace PuppetMaster.EscapeSequence
{
    public class EscapeSequenceGameManager : MonoBehaviour
    {
        public FadeController fadeController;

        [SerializeField] private ObstacleManager obstacleManager;

        public GameObject player;
        public GameObject hand;

        private bool gameOver = false;

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

            obstacleManager.StartSpawning();
        }

        public void FinishLoad()
        {
            loadScene.allowSceneActivation = true;
        }

        public void TriggerGameOver()
        {
            if (!gameOver)
                StartCoroutine(HandleGameOverState());
        }

        private IEnumerator HandleGameOverState()
        {
            if (gameOver) yield break;
            gameOver = true;

            yield return fadeController.FadeIn(5f);

            obstacleManager.StopSpawning();

            player.SetActive(false);
            hand.SetActive(false);

            yield return new WaitForSeconds(1);

            player.SetActive(true);
            hand.SetActive(true);

            yield return fadeController.FadeOut(50f);

            obstacleManager.StartSpawning();

            gameOver = false;
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