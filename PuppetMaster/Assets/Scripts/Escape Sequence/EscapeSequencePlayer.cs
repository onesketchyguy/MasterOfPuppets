using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Input;

namespace PuppetMaster.EscapeSequence
{
    public class EscapeSequencePlayer : MonoBehaviour, IMoveInputReceiver
    {
        public float HorizontalInput { get; set; }
        public float VerticalInput { get; set; }

        [SerializeField] private float speed = 5;

        [Range(0, 100)]
        [SerializeField] private float accelleration = 30f;

        [Range(0, 100)]
        [SerializeField] private float deccelleration = 30f;

        [SerializeField] private Rigidbody rigidBody;

        [SerializeField] private float targetY = 8;

        private Transform _transform;

        private void Start()
        {
            _transform = transform;
        }

        private void Update()
        {
            VerticalInput = Mathf.Clamp(targetY - _transform.position.y, -1, 1);
        }

        private void FixedUpdate()
        {
            var velocity = rigidBody.velocity;

            var move = HorizontalInput * speed;
            bool moving = move != 0;

            if (moving)
            {
                velocity.x = Mathf.MoveTowards(velocity.x, move,
                    accelleration * Time.deltaTime);
            }
            else
            {
                velocity.x = Mathf.MoveTowards(velocity.x, move,
                    deccelleration * Time.deltaTime);
            }

            velocity.y = Mathf.MoveTowards(velocity.y, VerticalInput, accelleration * Time.deltaTime);

            rigidBody.velocity = velocity;
        }
    }
}