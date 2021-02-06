using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Input;

namespace PuppetMaster
{
    /// <summary>
    /// The third person movement controller for the user.
    /// </summary>
    public class ThirdPersonController : MonoBehaviour, IMoveInputReciever
    {
        public float maxSpeed
        {
            get
            {
                return Speed * runSpeedModifier;
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
        public bool updateRotation = true;

        /// <summary>
        /// How fast to move by default.
        /// </summary>
        public float Speed = 5;

        /// <summary>
        /// How much to multiply the base Speed variable by when running.
        /// </summary>
        public float runSpeedModifier = 1.5f;

        /// <summary>
        /// How much slower to move when in the air.
        /// </summary>
        public float airSpeedMultiplier = 0.5f;

        /// <summary>
        /// How much slower to move when in the air.
        /// </summary>
        public float airControlMultiplier = 0.5f;

        /// <summary>
        /// How much force to use when jumping.
        /// </summary>
        public float VerticalForce = 8;

        /// <summary>
        /// How fast the user will reach the Speed variable, and slow down.
        /// </summary>
        public float accelerationSpeed = 24f;

        /// <summary>
        /// Weather or not ground is required to jump.
        /// </summary>
        public bool groundedRequired = true;

        /// <summary>
        /// The layers that the user can't jump on.
        /// </summary>
        public LayerMask notGround;

        /// <summary>
        /// The absolute bottom of the character.
        /// </summary>
        public float characterFeetPosition = 0.5f;

        //Send a line out from the character to check to see if ground exists.
        private bool grounded => Physics.Linecast(transform.position + Vector3.down * characterFeetPosition, transform.position + Vector3.down * (characterFeetPosition + 0.7f), ~notGround);

        private new Rigidbody rigidbody //The rigidbody for this object
        {
            get
            {   //Get the Rigidbody component, if one exists, otherwise add it.
                return GetComponent<Rigidbody>() ?? gameObject.AddComponent<Rigidbody>();
            }
        }

        public Vector3 CurrentVelocity => rigidbody.velocity; // The current amount of velocity for this body.

        public float HorizontalInput { get; set; }
        public float VerticalInput { get; set; }

        private Camera cameraManager;

        private void Start()
        {
            //Set the usingGlobalMovement variable.
            cameraManager = Camera.main;
        }

        private void FixedUpdate()
        {
            bool running = input.w > 0;
            float speed = running ? Speed * runSpeedModifier : Speed; //Define the speed of movement based on weather or not the user is running.

            UpdateGlobalPosition(speed);

            if (updateRotation) UpdateGlobalRotation(speed);

            rigidbody.angularVelocity = Vector3.zero;
        }

        private void UpdateGlobalPosition(float speed)
        {
            var i = input.normalized;

            var velocity = rigidbody.velocity;

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

            rigidbody.velocity = velocity;
            currentSpeed = velocity.magnitude;
        }

        private void UpdateGlobalRotation(float speed)
        {
            Vector3 groundInput = new Vector3(input.x, 0, input.z);

            Vector3 forwardInput = (cameraManager.transform.forward * groundInput.z);
            Vector3 strafeInput = (cameraManager.transform.right * groundInput.x);

            var _input = new Vector3(forwardInput.x + strafeInput.x, 0, forwardInput.z + strafeInput.z);

            speed = grounded ? speed : speed * airSpeedMultiplier;

            Vector3 pointA = transform.position + transform.forward * speed;

            Vector3 pointB = transform.position + (_input * (30));

            Vector3 faceDir = Vector3.MoveTowards(pointA, pointB, (Vector3.Distance(pointA, pointB) * speed) * Time.deltaTime);

            transform.LookAt(faceDir);
        }
    }
}