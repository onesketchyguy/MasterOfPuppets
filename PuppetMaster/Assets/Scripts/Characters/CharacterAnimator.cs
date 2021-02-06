using UnityEngine;

namespace PuppetMaster
{
    public class CharacterAnimator : MonoBehaviour
    {
        [Header("Component Setup")]
        [SerializeField] private Rigidbody rigidBody = null;

        [SerializeField] private Animator animator = null;
        [SerializeField] private new Renderer renderer = null;

        [Header("Field setup")]
        [SerializeField] private string horizontalMotionField = "MotionX";

        [SerializeField] private string verticalMotionField = "MotionY";

        private bool isSeen
        {
            get
            {
                if (renderer == null) return true; // If no renderer is setup just assume like we are being seen

                return renderer.isVisible;
            }
        }

        private Vector3 GetMotion()
        {
            // return the direction the animator is animating in.
            var velocity = rigidBody.velocity;

            // Return the motion in local space
            return animator.transform.TransformVector(velocity);
        }

        private void Start()
        {
            if (animator == null)
            {
                Debug.LogWarning($"{gameObject.name} Animator not setup. Trying to get the component from children...");

                animator = GetComponentInChildren<Animator>();

                if (animator == null)
                {
                    Debug.LogError($"{gameObject.name} cannot use {nameof(CharacterAnimator)}, reason: Animator not setup.");
                    Destroy(this); // Removes this component
                }
            }

            if (rigidBody == null)
            {
                Debug.LogError($"{gameObject.name} cannot use {nameof(CharacterAnimator)}, reason: Rigidbody not setup.");
                Destroy(this); // Removes this component
            }
        }

        private void Update()
        {
            if (isSeen)
            {
                var motion = GetMotion();

                animator.SetFloat(horizontalMotionField, motion.x);
                animator.SetFloat(verticalMotionField, motion.z);
            }
        }
    }
}