using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuppetMaster.EscapeSequence
{
    public class EscapeSequenceHand : MonoBehaviour
    {
        [SerializeField] private string playerTag = "Player";

        [SerializeField] private float offsetY = -5;

        [SerializeField] private float attackPositionY = -1;

        [SerializeField] private float speedX = 5f;

        [SerializeField] private Rigidbody rigidBody;

        [SerializeField] private UnityEngine.Events.UnityEvent onCaughtPlayer;

        private Transform player;

        private Transform _transform;

        private float horizontalInput;
        private float verticalInput;

        private bool chasingPlayer = true;

        private Vector3 startPosition;

        private void TrySetPlayer()
        {
            var go = GameObject.FindGameObjectWithTag(playerTag);

            if (go != null) player = go.transform;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            var dir = Vector3.right * 100;

            Gizmos.DrawLine(Vector3.up * attackPositionY - dir, Vector3.up * attackPositionY + dir);
        }

        private void OnEnable()
        {
            if (_transform == null) startPosition = transform.position;
            else _transform.position = startPosition;
        }

        private void Start()
        {
            _transform = transform;
            _transform.position = new Vector3(0, offsetY);
        }

        private void Update()
        {
            if (player == null)
            {
                TrySetPlayer();

                if (player != null)
                {
                    chasingPlayer = true;
                }
            }

            // Set the default inputs
            verticalInput = (attackPositionY + offsetY) - _transform.position.y;
            horizontalInput = 0;

            if (chasingPlayer)
            {
                horizontalInput = player.position.x - _transform.position.x;

                // Overrite the Y input if the player is in range
                if (player.position.y < attackPositionY)
                    verticalInput = player.position.y - _transform.position.y;
            }
        }

        private void FixedUpdate()
        {
            rigidBody.velocity = new Vector3(horizontalInput * speedX, verticalInput * speedX);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag(playerTag))
            {
                // Take player back to hell
                onCaughtPlayer?.Invoke();

                chasingPlayer = false;
            }
        }
    }
}