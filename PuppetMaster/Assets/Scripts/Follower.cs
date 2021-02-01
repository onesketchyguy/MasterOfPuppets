using UnityEngine;

namespace PuppetMaster
{
    public class Follower : MonoBehaviour
    {
        public Transform objectToFollow;
        private Transform _transform;

        private void Start()
        {
            _transform = transform;
        }

        private void Update()
        {
            _transform.position = Vector3.Slerp(_transform.position, objectToFollow.position, 20 * Time.deltaTime);
        }
    }
}