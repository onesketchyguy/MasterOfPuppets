using UnityEngine;

namespace PuppetMaster
{
    // FIXME: Needs code review
    public class Follower : MonoBehaviour
    {
        [Tooltip("Leave empty to just follow the player.")]
        [SerializeField] private Transform _objectToFollow;

        [SerializeField] private bool copyRotation = true;

        private bool followPlayer = false;

        private Transform GetFollowObject()
        {
            if (_objectToFollow == null || _objectToFollow.gameObject.activeSelf == false ||
                (_objectToFollow != null && followPlayer == true &&
                _objectToFollow != CharacterInput.playerControlled.transform))
            {
                _objectToFollow = CharacterInput.playerControlled.transform;
            }

            return _objectToFollow;
        }

        private Transform m_transform;

        [Range(0, 1f)]
        [Tooltip("Amount of speed weight to give based on distance. 0 is no speed increase 1 is full distance compensation.")]
        [SerializeField] private float distanceWeight = 1;

        [SerializeField] private float baseSpeed = 5;

        private void Start()
        {
            m_transform = transform;
            followPlayer = _objectToFollow == null;
        }

        private void FixedUpdate()
        {
            if (GetFollowObject() == null) return;

            var dist = (Vector3.Distance(m_transform.position, GetFollowObject().position)) * distanceWeight;
            m_transform.position = Vector3.Lerp(m_transform.position, GetFollowObject().position, (dist + baseSpeed) * Time.deltaTime);

            if (copyRotation == true)
                m_transform.rotation = Quaternion.Lerp(m_transform.rotation, GetFollowObject().rotation, (dist + baseSpeed) * Time.deltaTime);
        }
    }
}