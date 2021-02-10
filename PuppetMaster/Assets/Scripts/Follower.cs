using UnityEngine;

namespace PuppetMaster
{
    public class Follower : MonoBehaviour
    {
        [SerializeField] private Transform objectToFollow;
        private Transform _transform;

        [Range(0, 1f)]
        [Tooltip("Amount of speed weight to give based on distance. 0 is no speed increase 1 is full distance compensation.")]
        [SerializeField] private float distanceWeight = 1;

        [SerializeField] private float baseSpeed = 5;

        private void Start()
        {
            _transform = transform;
        }

        private void FixedUpdate()
        {
            var dist = (Vector3.Distance(_transform.position, objectToFollow.position)) * distanceWeight;
            _transform.position = Vector3.Slerp(_transform.position, objectToFollow.position, (dist + baseSpeed) * Time.deltaTime);
        }
    }
}