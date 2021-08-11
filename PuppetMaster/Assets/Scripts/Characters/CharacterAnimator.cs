using Player.Input;
using RootMotion.FinalIK;
using UnityEngine;

namespace PuppetMaster
{
    // FIXME: NEEDS CODE REVIEW
    public class CharacterAnimator : MonoBehaviour, ILookInputReceiver
    {
        [Header("Component Setup")]
        [SerializeField] private Rigidbody rigidBody = null;

        [SerializeField] private RuntimeAnimatorController controller;

        // NOTE: I don't think I even need this to be an array of animators, needs research.
        private Animator[] animators = null;

        [SerializeField] private new Renderer renderer = null;

        [SerializeField] private CombatManager combatManager = null;
        [SerializeField] private AimController aimController = null;

        [Header("Field setup")]
        [SerializeField] private int aimLayerIndex = 1;

        [SerializeField] private string strafeMotionField = "MotionX";

        [SerializeField] private string forwardMotionField = "MotionY";

        [SerializeField] private Transform lookObject;
        public Vector3 lookDirection { get; set; }

        private Transform _transform;

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
            return _transform.InverseTransformDirection(velocity);
        }

        private void Start()
        {
            if (animators == null)
            {
                Debug.LogWarning($"{gameObject.name} Animator not setup. Trying to get the component from children...");

                animators = GetComponentsInChildren<Animator>();

                if (animators == null)
                {
                    Debug.LogError($"{gameObject.name} cannot use {nameof(CharacterAnimator)}, reason: Animator not setup on children.");
                    Destroy(this); // Removes this component
                }
            }

            foreach (var animator in animators)
            {
                animator.runtimeAnimatorController = controller;
            }

            if (rigidBody == null)
            {
                Debug.LogError($"{gameObject.name} cannot use {nameof(CharacterAnimator)}, reason: Rigidbody not setup.");
                Destroy(this); // Removes this component
            }

            _transform = transform;
        }

        private void Update()
        {
            if (isSeen)
            {
                UpdateMovement();
                UpdateLookIK();
            }
        }

        private void UpdateMovement()
        {
            var motion = GetMotion();

            foreach (var animator in animators)
            {
                animator.SetFloat(strafeMotionField, -motion.x);
                animator.SetFloat(forwardMotionField, motion.z);
            }
        }

        private void UpdateLookIK()
        {
            if (lookDirection == Vector3.zero)
            {
                lookDirection = _transform.forward;
            }

            // FIXME: this isn't working for fuck knows the reason
            //lookObject.position = _transform.position + // Add our position to offset the object from us
            //    (lookDirection * 10) + // Add our actual aim at position
            //    Vector3.up * 0.5f; // Add a slight incline up as to try to aim at the target. FIXME: auto aim

            float armedWeight = combatManager.isArmed ? 1 : 0;
            foreach (var animator in animators)
                animator.SetLayerWeight(aimLayerIndex, armedWeight);

            aimController.weight = armedWeight;
        }
    }
}