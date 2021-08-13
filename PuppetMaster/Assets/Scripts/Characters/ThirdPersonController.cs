using UnityEngine;
using Player.Input;

namespace PuppetMaster
{
    /// <summary>
    /// Top down movement controller for the user.
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
        /// Look direction input.
        /// </summary>
        public Vector3 lookInput { get; set; }

        /// <summary>
        /// Horizontal move input
        /// </summary>
        public float HorizontalInput { get; set; }

        /// <summary>
        /// Vertical, or forward and back, move input.
        /// </summary>
        public float VerticalInput { get; set; }

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

        /// <summary>
        /// Whether or not the character is grounded.
        /// </summary>
        private bool grounded;

        /// <summary>
        /// A cached value for the transform.
        /// </summary>
        private Transform m_transform;

        /// <summary>
        /// The rigidbody for this object.
        /// </summary>
        private Rigidbody rigidBody;

        public Vector3 CurrentVelocity => rigidBody.velocity; // The current amount of velocity for this body.

        [Tooltip("Blocks velocity going over '0'. NOTE: Does not block gravity.")]
        [SerializeField] private bool freezeY = true;

        private Transform mainCamera;

        private void Start()
        {
            // Get the Rigidbody component
            rigidBody = GetComponent<Rigidbody>();

            // Cache the transform
            m_transform = transform;

            // Store the camera's transform
            mainCamera = Camera.main.transform;
        }

        private void FixedUpdate()
        {
            // Send a line out from the character to check to see if ground exists.
            grounded = Physics.Linecast(m_transform.position + Vector3.down * characterFeetPosition, m_transform.position + Vector3.down * (characterFeetPosition + 0.7f), ~notGround);

            // Define the speed of movement based on weather or not the user is running.
            bool running = input.w > 0;
            float spd = running ? speed * runSpeedModifier : speed;

            // Update the position of this character
            UpdateGlobalPosition(spd);

            // Update the rotation of this character
            if (updateRotation) UpdateLookRotation(lookInput);
            rigidBody.angularVelocity = Vector3.zero;
        }

        public void SetFreezeY(bool freeze)
        {
            freezeY = freeze;
        }

        private void UpdateGlobalPosition(float speed)
        {
            // Store our velocity for manipulation
            var velocity = rigidBody.velocity;

            // If freeze y prevent any y axis velocity gain.
            if (velocity.y > 0 && freezeY) velocity.y = 0;

            // Define the speed based off the air speed multiplier.
            speed = grounded ? speed : speed * airSpeedMultiplier;

            // Define some input variables
            var forwardInput = (mainCamera.forward * input.z * speed);
            var strafeInput = (mainCamera.right * input.x * speed);

            float xInput = forwardInput.x + strafeInput.x;
            float zInput = forwardInput.z + strafeInput.z;

            // Define how quickly to move between variables
            var lerpSpeed = grounded ? accelerationSpeed : accelerationSpeed * airControlMultiplier;

            // Move the velocity in the desired direction
            velocity.x = Mathf.MoveTowards(velocity.x, xInput, lerpSpeed * Time.deltaTime);
            velocity.z = Mathf.MoveTowards(velocity.z, zInput, lerpSpeed * Time.deltaTime);

            // Handle jumping
            if (input.y > 0 && freezeY == false)
            {
                // Check if we need the ground to jump
                if (groundedRequired)
                {
                    // Jump if grounded
                    if (grounded)
                    {
                        velocity.y = input.y * VerticalForce;
                    }
                }
                else
                {
                    // Just jump
                    velocity.y = input.y * VerticalForce;
                }
            }

            // Apply the new velocity values
            rigidBody.velocity = velocity;
            currentSpeed = velocity.magnitude;
        }

        /// <summary>
        /// Looks using the RigidBody.
        /// </summary>
        /// <param name="lookInput"></param>
        private void UpdateLookRotation(Vector3 lookInput)
        {
            //// Find the position in local space
            //var targetDir = lookAt + m_transform.position;
            //var localTarget = m_transform.InverseTransformPoint(targetDir);
            //
            //// Find the angle based off the local space
            //var angle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

            // Move towards that angle
            var eulerAngleVelocity = rigidBody.rotation.eulerAngles + new Vector3(0, lookInput.x, 0);
            var newRotation = Quaternion.Euler(eulerAngleVelocity);
            rigidBody.MoveRotation(newRotation);
        }
    }
}