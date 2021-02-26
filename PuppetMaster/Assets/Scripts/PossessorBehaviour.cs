using UnityEngine;
using Player.Input;
using Utility;

namespace PuppetMaster
{
    public class PossessorBehaviour : MonoBehaviour, IActionInputReceiver
    {
        [Tooltip("The physics layer to search through to find mortals.")]
        [SerializeField] private LayerMask possessableMask = 0;

        [Tooltip("The distance this creature can be from a mortal before being capable of taking possession of their soul.")]
        [SerializeField] private float overlapRadius = 3;

        [Space]
        [Tooltip("The gameobject that holds onto the visuals. ie the mesh, or renderer.")]
        [SerializeField] private GameObject display;

        [Tooltip("Effect to play on taking possession of a mortal.")]
        [SerializeField] private GameObject onPossessionEffect;

        [Tooltip("The collider to disable when possessing a mortal.")]
        [SerializeField] private Collider _collider;

        [Tooltip("The rigidbody to use when possessing a mortal.")]
        [SerializeField] private Rigidbody rigidBody;

        /// <summary>
        /// A cached transform.
        /// </summary>
        private Transform _transform;

        /// <summary>
        /// A cache of who is currently possessed.
        /// </summary>
        private Transform _possessed;

        /// <summary>
        /// Returns the current object being possessed, or null if none.
        /// </summary>
        /// <returns></returns>
        private CharacterInput GetPossessing()
        {
            var controlled = CharacterInput.playerControlled;

            if (controlled.gameObject == gameObject) return null;

            return controlled;
        }

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white * 0.25f;
            Gizmos.DrawSphere(transform.position, overlapRadius);
        }

#endif

        private void Start()
        {
            // Cache the transform.
            _transform = transform;
        }

        private void Update()
        {
            if (GetPossessing() == null && display.activeSelf == false)
            {
                // Set our visuals to active if we are not possessing anyone.
                SetVisable(true);
            }
            else if (GetPossessing() != null && display.activeSelf == true)
            {
                // Set our visuals to inactive if we are possessing some one and they are still on.
                SetVisable(false);
            }
            else if (GetPossessing() != null)
            {
                // Move to the actively controlled character
                _transform.position = _possessed.position;
            }
        }

        private void Possess(CharacterInput character)
        {
            // Hide ourselves
            SetVisable(false);

            // Set the object to recieve player input
            CharacterInput.playerControlled = character;
            _possessed = character.transform;

            // Create the possession effect
            ObjectPool.Get(onPossessionEffect, _possessed.position, Quaternion.identity);
        }

        /// <summary>
        /// Hides or shows this object
        /// </summary>
        /// <param name="visable"></param>
        private void SetVisable(bool visable)
        {
            _collider.enabled = visable;
            rigidBody.isKinematic = visable;
            display.SetActive(visable);
        }

        /*
         * INPUT AREA
         */

        public void OnFire1()
        {
            // Posess a body
            var collisions = Physics.OverlapSphere(_transform.position, overlapRadius, possessableMask);

            if (collisions != null)
            {
                // Get the closest possessable object
                Collider closest = null;
                var distToClosest = overlapRadius;

                foreach (var item in collisions)
                {
                    if (item == closest || item.transform == _transform) continue;

                    // Check if this object is closer than the stored closest object
                    var dist = Vector3.Distance(item.transform.position, _transform.position);
                    if (dist < distToClosest)
                    {
                        closest = item;
                        distToClosest = dist;
                    }
                }

                // Possess the closest character
                if (closest != null) Possess(closest.gameObject.GetComponent<CharacterInput>());
            }
        }

        public void OnFire1Up()
        {
        }

        public void OnFire2()
        {
        }

        public void OnFire2Up()
        {
        }
    }
}