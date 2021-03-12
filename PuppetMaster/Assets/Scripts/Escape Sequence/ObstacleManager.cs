using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuppetMaster
{
    public class ObstacleManager : MonoBehaviour
    {
        [SerializeField] private float obstacleSpawnMinTime = 0.1f;
        [SerializeField] private float obstacleSpawnMaxTime = 1.0f;
        [SerializeField] private GameObject obstaclePrefab;

        [SerializeField] private int obstacleCount = 5;

        private float spawnTime;

        private GameObject[] obstacles;

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
        }

        private void SpawnObstacle()
        {
            if (obstacles == null || obstacles.Length == 0) return;

            for (int i = 0; i < obstacles.Length; i++)
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

            for (int i = 0; i < obstacles.Length; i++)
            {
                obstacles[i] = Instantiate(obstaclePrefab, transform);
                obstacles[i].SetActive(false);

                if (i % 2 == 0)
                {
                    // Spawn 2 objects per frame
                    yield return new WaitForEndOfFrame();
                }
            }
        }
    }
}