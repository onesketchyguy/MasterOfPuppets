using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PuppetMaster.EscapeSequence
{
    public class EscapeSequenceGameManager : MonoBehaviour
    {
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

        public void FinishLoad()
        {
            Debug.Log("Load");
            loadScene.allowSceneActivation = true;
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