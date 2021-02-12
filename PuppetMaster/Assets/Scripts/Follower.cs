using UnityEngine;

namespace PuppetMaster
{
    public class Follower : MonoBehaviour
    {
        [Tooltip("Leave empty to just follow the player.")]
        [SerializeField] private Transform objectToFollow;

        private Transform GetFollowObject()
        {
            if (objectToFollow == null)
            {
                return CharacterInput.playerControlled.transform;
            }

            return objectToFollow;
        }

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
            var dist = (Vector3.Distance(_transform.position, GetFollowObject().position)) * distanceWeight;
            _transform.position = Vector3.Lerp(_transform.position, GetFollowObject().position, (dist + baseSpeed) * Time.deltaTime);
        }
    }
}