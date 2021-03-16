using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuppetMaster
{
    public class ObstacleManager : MonoBehaviour
    {
        [SerializeField] private GameObject obstacleWarningPrefab;
        [SerializeField] private RectTransform warningParent;

        [SerializeField] private float obstacleSpawnMinTime = 0.1f;
        [SerializeField] private float obstacleSpawnMaxTime = 1.0f;
        [SerializeField] private GameObject obstaclePrefab;

        [SerializeField] private int obstacleCount = 5;

        [SerializeField] private float warningOffset = 3.5f;

        private float spawnTime;

        private GameObject[] obstacles;
        private Renderer[] obstacleRenderers;
        private GameObject[] obstacleWarnings;

        private void OnValidate()
        {
            if (obstacleSpawnMinTime < 0)
            {
                obstacleSpawnMinTime = 0;
            }

            if (obstacleSpawnMaxTime < obstacleSpawnMinTime)
            {
                obstacleSpawnMaxTime = obstacleSpawnMinTime;
            }
        }

        private void OnEnable()
        {
            spawnTime = 0;
        }

        private void OnDisable()
        {
            if (obstacles == null || obstacles.Length == 0) return;

            for (int i = 0; i < obstacles.Length; i++)
            {
                if (obstacles[i] == null || obstacles[i].activeSelf == true) continue;

                obstacles[i].SetActive(false);
            }
        }

        private void Start()
        {
            StartCoroutine(PopulateObstacleArray());
        }

        private void Update()
        {
            if (spawnTime > 0)
            {
                spawnTime -= Time.deltaTime;
            }
            else
            {
                spawnTime = Random.Range(obstacleSpawnMinTime, obstacleSpawnMaxTime);

                // Spawn a obstacle
                SpawnObstacle();
            }

            if (obstacles == null || obstacles.Length == 0) return;

            if (Time.frameCount % 5 == 0)
            {
                for (int i = 0; i < obstacleCount; i++)
                {
                    if (obstacles[i].activeSelf == false)
                    {
                        if (obstacleWarnings[i].activeSelf == true)
                        {
                            obstacleWarnings[i].SetActive(false);
                        }

                        continue;
                    }

                    obstacleWarnings[i].SetActive(true);

                    obstacleWarnings[i].transform.position = Camera.main.WorldToScreenPoint(
                        obstacles[i].transform.position - Vector3.up * warningOffset);
                }
            }
        }

        private void SpawnObstacle()
        {
            if (obstacles == null || obstacles.Length == 0) return;

            for (int i = 0; i < obstacleCount; i++)
            {
                if (obstacles[i] == null || obstacles[i].activeSelf == true) continue;

                obstacles[i].SetActive(true);

                break;
            }
        }

        private IEnumerator PopulateObstacleArray()
        {
            yield return new WaitForEndOfFrame();

            obstacles = new GameObject[obstacleCount];
            obstacleWarnings = new GameObject[obstacleCount];
            obstacleRenderers = new Renderer[obstacleCount];

            for (int i = 0; i < obstacles.Length; i++)
            {
                obstacles[i] = Instantiate(obstaclePrefab, transform);
                obstacles[i].SetActive(false);

                obstacleRenderers[i] = obstacles[i].GetComponent<Renderer>();

                obstacleWarnings[i] = Instantiate(obstacleWarningPrefab, warningParent);
                obstacleWarnings[i].SetActive(false);

                if (i % 2 == 0)
                {
                    // Spawn 4 objects per frame
                    yield return new WaitForEndOfFrame();
                }
            }
        }
    }
}