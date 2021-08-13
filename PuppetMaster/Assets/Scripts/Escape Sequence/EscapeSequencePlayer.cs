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

        [SerializeField] private EscapeSequenceGameManager gameManager;
        [SerializeField] private float speed = 5;
        [SerializeField] private float escapeSpeed = 10;

        [SerializeField] private UnityEngine.Events.UnityEvent onHit;
        [SerializeField] private UnityEngine.Events.UnityEvent onReachedEnd;

        [Range(0, 100)]
        [SerializeField] private float accelleration = 30f;

        [Range(0, 100)]
        [SerializeField] private float deccelleration = 30f;

        [SerializeField] private Rigidbody rigidBody;

        [SerializeField] private float targetY = 8;
        private float _targetY;

        private Transform m_transform;

        [Tooltip("Minimum distance to target before loading.")]
        [SerializeField] private float minDist = 0.01f;

        private Vector3 startPosition;

        private float dist;

        private float escapeProgress;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(Vector3.zero + Vector3.up * targetY, 0.1f);
        }

        private void OnEnable()
        {
            if (m_transform == null) startPosition = transform.position;
            else
            {
                escapeProgress = 0;
                m_transform.position = startPosition;
            }
        }

        private void Start()
        {
            m_transform = transform;
        }

        private void Update()
        {
            if (gameManager.progress >= 0.9f)
            {
                escapeProgress = Mathf.MoveTowards(escapeProgress, 1, escapeSpeed * Time.deltaTime);
            }

            _targetY = targetY * escapeProgress;

            dist = Vector2.Distance(m_transform.position, new Vector2(m_transform.position.x, targetY));

            if (dist <= minDist)
            {
                // Finished level
                // gameManager.FinishLoad();
                onReachedEnd?.Invoke();
            }

            VerticalInput = Mathf.Clamp(_targetY - m_transform.position.y, -1, 1);
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

            velocity.y = Mathf.MoveTowards(velocity.y, VerticalInput, speed * Time.deltaTime);

            rigidBody.velocity = velocity;
        }

        // Called by EscapeSequenceObstacle on hit
        public void OnHit() => onHit?.Invoke();
    }
}