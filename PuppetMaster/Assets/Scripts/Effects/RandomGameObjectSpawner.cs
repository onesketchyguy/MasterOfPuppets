using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuppetMaster.Effects
{
    public class RandomGameObjectSpawner : MonoBehaviour
    {
        [SerializeField] private bool spawnOnStart = true;

        [SerializeField] private GameObject[] objects;

        [SerializeField] private int numberToSpawn = 1;

        [SerializeField] private Vector3 offsetFromSpawner;

        [SerializeField] private bool randomizeRotation = false;

        [SerializeField] private bool setParentToThis = false;

        private void OnValidate()
        {
            if (numberToSpawn <= 0) numberToSpawn = Mathf.Abs(numberToSpawn);
        }

        private void Start()
        {
            if (spawnOnStart) SpawnObjects();
        }

        public void SpawnObjects()
        {
            for (int i = 0; i < numberToSpawn; i++)
            {
                Instantiate(objects[Random.Range(0, objects.Length)], transform.position + offsetFromSpawner,
                   randomizeRotation ? Quaternion.Euler(Random.insideUnitSphere) : Quaternion.identity, setParentToThis ? transform : null);
            }
        }
    }
}