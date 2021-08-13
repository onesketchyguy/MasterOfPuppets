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

        [SerializeField] private Transform player;

        [SerializeField] private UnityEngine.Events.UnityEvent onCaughtPlayer;

        [SerializeField] private Animator animator;
        [SerializeField] private string GrabAnimationName = "FistClosed";

        private Transform m_transform;

        private float horizontalInput;
        private float verticalInput;

        private bool chasingPlayer = true;

        private Vector3 startPosition;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            var dir = Vector3.right * 100;

            Gizmos.DrawLine(Vector3.up * attackPositionY - dir, Vector3.up * attackPositionY + dir);
        }

        private void OnEnable()
        {
            if (m_transform == null) startPosition = transform.position;
            else m_transform.position = startPosition;

            chasingPlayer = true;
        }

        private void Start()
        {
            m_transform = transform;
            m_transform.position = new Vector3(0, offsetY);
        }

        private void Update()
        {
            // Set the default inputs
            verticalInput = (attackPositionY + offsetY) - m_transform.position.y;
            horizontalInput = 0;

            animator.SetBool(GrabAnimationName, chasingPlayer == false);

            if (chasingPlayer)
            {
                horizontalInput = player.position.x - m_transform.position.x;

                // Overrite the Y input if the player is in range
                if (player.position.y < attackPositionY)
                    verticalInput = player.position.y - m_transform.position.y;
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