using UnityEngine;
using Player.Input;

namespace PuppetMaster
{
    /// <summary>
    /// The third person movement controller for the user.
    /// </summary>
    public class ThirdPersonController : MonoBehaviour, IMoveInputReceiver, ILookInputReceiver
    {
        public float maxSpeed
        {
            get
            {
                return speed * runSpeedModifier;
            }
        }

        public float currentSpeed { get; set; }

        /// <summary>
        /// Where the user will feed in the movement.
        /// </summary>
        private Vector4 input
        {
            get
            {
                // FIXME: Add jump support, and run support
                return new Vector4(HorizontalInput, 0, VerticalInput, 0);
            }
        }

        [Header("Movement variables")]
        [SerializeField] private bool updateRotation = true;

        [Tooltip("Base movement speed.")]
        public float speed = 5;

        [Tooltip("How much to multiply the base Speed variable by when running.")]
        public float runSpeedModifier = 1.5f;

        [Tooltip("How fast the user will reach the Speed variable, and slow down.")]
        public float accelerationSpeed = 24f;

        [Tooltip("Speed to turn the character.")]
        [SerializeField] private float turnSpeed = 30;

        [Tooltip("How much slower to move when in the air.")]
        public float airSpeedMultiplier = 0.5f;

        [Tooltip("How much slower to move when in the air.")]
        public float airControlMultiplier = 0.5f;

        [Tooltip("How much force to use when jumping.")]
        public float VerticalForce = 8;

        [Tooltip("Whether or not ground is required to jump.")]
        [SerializeField] private bool groundedRequired = true;

        [Tooltip("The layers that the user can't jump on.")]
        [SerializeField] private LayerMask notGround = ~0;

        [Tooltip("The absolute bottom of the character.")]
        [SerializeField] private float characterFeetPosition = 0.5f;

        private bool grounded;

        private Transform _transform;

        public Vector3 lookDirection { get; set; }

        /// <summary>
        /// The rigidbody for this object.
        /// </summary>
        private Rigidbody rigidBody;

        public Vector3 CurrentVelocity => rigidBody.velocity; // The current amount of velocity for this body.

        public float HorizontalInput { get; set; }
        public float VerticalInput { get; set; }

        [SerializeField] private bool freezeY = true;

        private Camera cameraManager;

        private void Start()
        {
            // Get the Rigidbody component
            rigidBody = GetComponent<Rigidbody>();

            // Cache the transform
            _transform = transform;

            //Set the usingGlobalMovement variable.
            cameraManager = Camera.main;
        }

        private void FixedUpdate()
        {
            // Send a line out from the character to check to see if ground exists.
            grounded = Physics.Linecast(transform.position + Vector3.down * characterFeetPosition, transform.position + Vector3.down * (characterFeetPosition + 0.7f), ~notGround);

            // Define the speed of movement based on weather or not the user is running.
            bool running = input.w > 0;
            float spd = running ? speed * runSpeedModifier : speed;

            // Update the position of this character
            UpdateGlobalPosition(spd);

            // Update the rotation of this character
            if (updateRotation) UpdateLookRotation(lookDirection);
            rigidBody.angularVelocity = Vector3.zero;
        }

        public void SetFreezeY(bool freeze)
        {
            freezeY = freeze;
        }

        private void UpdateGlobalPosition(float speed)
        {
            var i = input.normalized;

            var velocity = rigidBody.velocity;

            if (velocity.y > 0 && freezeY)
                velocity.y = 0;

            speed = grounded ? speed : speed * airSpeedMultiplier;

            var forwardInput = (cameraManager.transform.forward * i.z * speed);
            var strafeInput = (cameraManager.transform.right * i.x * speed);

            float xInput = forwardInput.x + strafeInput.x;
            float zInput = forwardInput.z + strafeInput.z;

            var lerpSpeed = grounded ? accelerationSpeed : accelerationSpeed * airControlMultiplier;

            velocity.x = Mathf.MoveTowards(velocity.x, xInput, lerpSpeed * Time.deltaTime);
            velocity.z = Mathf.MoveTowards(velocity.z, zInput, lerpSpeed * Time.deltaTime);

            if (input.y > 0)
            {
                if (groundedRequired)
                {
                    if (grounded)
                    {
                        velocity.y = input.y * VerticalForce;
                    }
                }
                else
                {
                    velocity.y = input.y * VerticalForce;
                }
            }

            rigidBody.velocity = velocity;
            currentSpeed = velocity.magnitude;
        }

        private void UpdateLookRotation(Vector3 lookAt)
        {
            var targetDir = lookAt + _transform.position;
            var localTarget = _transform.InverseTransformPoint(targetDir);

            var angle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

            var eulerAngleVelocity = new Vector3(0, angle, 0);
            var deltaRotation = Quaternion.Euler(eulerAngleVelocity * turnSpeed * Time.fixedDeltaTime);
            rigidBody.MoveRotation(rigidBody.rotation * deltaRotation);
        }
    }
}