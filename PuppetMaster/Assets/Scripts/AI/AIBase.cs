using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace PuppetMaster.AI
{
    public enum AI_State
    {
        idle,
        scared,
        fighting
    }

    public class AIBase : MonoBehaviour
    {
        internal static ScoreManager scoreManager;

        public AI_State state;

        [SerializeField] private int scoreToGiveOnDeath = 100;

        [SerializeField] private CombatManager combatManager = null;
        [SerializeField] private CharacterInput inputManager = null;
        [SerializeField] private CharacterStats stats = null;
        [SerializeField] private NavMeshAgent agent = null;

        [Tooltip("How long to wait in seconds before moving again.")]
        [SerializeField] private float movementFrequency = 3;

        [Tooltip("How far away from a point this agent can stop.")]
        [SerializeField] private float stoppingDistance = 2;

        [SerializeField] private float walkRadius = 5;

        [SerializeField] private AudioClipPlayer audioSource = null;

        [SerializeField] private float visabilityRange = 10;

        private int updateFrame = 0;
        private const int UPDATE_FRAME_LIMIT = 3;

        /// <summary>
        /// Cached local transform
        /// </summary>
        private Transform _transform;

        private NavMeshPath movementPath;
        private int pathIndex = 0;

        private Transform target;
        private Vector3 targetPosition;
        private float timeInPosition;

        /// <summary>
        /// A flag to let us know if we are already calculating a path.
        /// </summary>
        private bool calulatingPath = false;

        /// <summary>
        /// The direction the character is moving.
        /// </summary>
        private Vector3 inputDirection;

        private void OnValidate()
        {
            if (agent == null)
                agent = GetComponent<NavMeshAgent>();

            if (inputManager == null)
                inputManager = GetComponent<CharacterInput>();

            if (stats == null)
                stats = GetComponent<CharacterStats>();
        }

        private void Awake()
        {
            if (scoreManager == null)
            {
                scoreManager = FindObjectOfType<ScoreManager>();
            }
        }

        private void Start()
        {
            // Chache the transform
            _transform = transform;

            // Reset the moveTo position to our point for later
            targetPosition = _transform.position;

            // Initialize the nav mesh path
            movementPath = new NavMeshPath();

            // Keep the AI a little bit random
            timeInPosition = Random.Range(0, movementFrequency);

            // By choosing a random fram to choose to update on each AI
            // won't (necessarilly) be updating on the same frame.
            updateFrame = Random.Range(0, UPDATE_FRAME_LIMIT);

            // Make sure we know when we've died
            stats.onDiedCallback += OnDeath;
        }

        // Update is called once per frame
        private void Update()
        {
            // Only update periodically
            // This helps to ensure a good frame rate since the AI aren't all
            // Updating every frame
            if (Time.frameCount % UPDATE_FRAME_LIMIT != updateFrame) return;

            if (inputManager.isPlayer == false)
            {
                // Do AI stuff
                HandleStateMachine();
                HandleMovement();

                if (agent.enabled == false) agent.enabled = true;
            }
            else
            {
                // Don't do AI stuff
                if (agent.enabled == true) agent.enabled = false;
            }
        }

        private void OnDeath()
        {
            // Do some died stuff

            if (inputManager.isPlayer == false)
                // Add score
                scoreManager.ModifyScore(scoreToGiveOnDeath);

            audioSource.PlayClip();
        }

        public void SwapState(AI_State state)
        {
            this.state = state;
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        private void HandleMovement()
        {
            // Stop the current input direction
            inputDirection *= 0.0f;

            // Try to calculate a new input direction
            if (movementPath.corners != null && movementPath.corners.Length > 0)
            {
                if (pathIndex >= movementPath.corners.Length || pathIndex < 0)
                {
                    // Clear the nav path
                    movementPath.ClearCorners();
                    pathIndex = 0;

                    return;
                }

                targetPosition = movementPath.corners[pathIndex];

                inputDirection = targetPosition - _transform.position;

                if (Vector3.Distance(_transform.position, targetPosition) <= stoppingDistance)
                {
                    pathIndex++;

                    if (pathIndex >= movementPath.corners.Length)
                    {
                        pathIndex = 0;
                        movementPath.ClearCorners();
                    }
                }
            }

            // Feed the input to the input manager.
            inputManager.HandleMovement(inputDirection);
        }

        private void HandleStateMachine()
        {
            switch (state)
            {
                case AI_State.idle:
                    // Wander around and such
                    HandleIdleState();

                    // If we are just chilling, let's get some action
                    if (combatManager.isArmed && state != AI_State.fighting)
                        SwapState(AI_State.fighting);

                    break;

                case AI_State.scared:
                    // Flee
                    HandleScaredState();

                    break;

                case AI_State.fighting:
                    // Fight
                    HandleFightingState();

                    break;

                default:
                    SwapState(AI_State.idle);
                    break;
            }
        }

        private void HandleIdleState()
        {
            // Find a wander point and move there

            if (Vector3.Distance(_transform.position, targetPosition) <= stoppingDistance)
            {
                if (timeInPosition > movementFrequency)
                {
                    // Find a new wonder pos

                    // pick a random position within range that is on the navmesh
                    Vector3 moveTo;

                    // Commit to a loop where we look for a point on the navmesh
                    do
                    {
                        var wanderDirection = Random.insideUnitSphere * walkRadius;

                        wanderDirection += _transform.position;
                        wanderDirection.y = _transform.position.y;

                        moveTo = GetNavMeshPosition(wanderDirection);
                    }
                    while (moveTo == Vector3.zero);

                    // Generate a path to the point
                    CalculatePath(moveTo);
                }
                else
                {
                    // Increment the amount of time we have spent in this position.
                    timeInPosition += Time.deltaTime;
                }
            }
        }

        private void HandleScaredState()
        {
            // Find a point farthest away from the attacker and run there

            if (target == null)
            {
                SwapState(AI_State.idle);
            }
            else
            {
                var runDist = Random.Range(10, 30);

                if (Vector3.Distance(_transform.position, target.position) < runDist)
                {
                    var direction = Random.insideUnitSphere * runDist;
                    direction.y = _transform.position.y;
                    direction += _transform.position;

                    var moveTo = GetNavMeshPosition(direction);

                    // Generate a path to the point
                    CalculatePath(moveTo);
                }
            }
        }

        private void HandleFightingState()
        {
            // Stand within attacking distance and attack the target
            if (target == null)
            {
                SwapState(AI_State.idle);
            }
            else
            {
                // Check if we can even see the target
                var targetVisable = CanSeeObject(target);

                if (targetVisable)
                {
                    // If the target is visable we should attack them
                    inputManager.Fire1_performed();
                }
                else
                {
                    // If the target is not visable we should move
                    var point = target.position;

                    // Clamp the position to our range
                    point.x = Mathf.Clamp(_transform.position.x, point.x - visabilityRange, point.x + visabilityRange);
                    point.z = Mathf.Clamp(_transform.position.z, point.z - visabilityRange, point.z + visabilityRange);

                    var moveTo = GetNavMeshPosition(point);

                    // Generate a path to the point
                    CalculatePath(moveTo);
                }
            }
        }

        public bool CanSeeObject(Transform obj)
        {
            // Check if the object is even within visability range
            if (Vector3.Distance(_transform.position, obj.position) > visabilityRange)
            {
                return false;
            }
            else
            {
                /*
                 * NOTE: This does not support field of view!
                 * This only supports seeing what is DIRECTLY in front of this object
                 */

                // Launch a ray out and check if it hits the target object
                RaycastHit hit;

                // Does the ray intersect any objects
                if (Physics.Raycast(_transform.position,
                    _transform.TransformDirection(Vector3.forward),
                    out hit, visabilityRange, obj.gameObject.layer))
                {
                    Debug.DrawRay(_transform.position,
                        _transform.TransformDirection(Vector3.forward) * hit.distance,
                        Color.yellow);
                    return true;
                }
                else
                {
                    Debug.DrawRay(transform.position,
                        _transform.TransformDirection(Vector3.forward) * 1000,
                        Color.white);
                    return false;
                }
            }
        }

        private Vector3 GetNavMeshPosition(Vector3 position)
        {
            NavMeshHit hit;
            NavMesh.SamplePosition(position, out hit, walkRadius, 1);

            return hit.position;
        }

        /// <summary>
        /// Creates a path for the character to follow.
        /// </summary>
        /// <param name="point"></param>
        private void CalculatePath(Vector3 point)
        {
            if (movementPath.corners == null || movementPath.corners.Length == 0 ||
                pathIndex == movementPath.corners.Length - 1)
            {
                StartCoroutine(CalculatePathAsync(point));
            }
        }

        /// <summary>
        /// Creates a path for the character to follow.
        /// </summary>
        /// <param name="point"></param>
        private IEnumerator CalculatePathAsync(Vector3 point)
        {
            if (calulatingPath) yield break;
            calulatingPath = true;

            //Debug.Log("Recalulating path...");

            yield return null;

            // Store the calculated path and move following that path
            if (agent.CalculatePath(point, movementPath))
            {
                timeInPosition = 0;
                pathIndex = 0;

                //Debug.Log("Successfully generated path.");
            }
            else
            {
                Debug.LogWarning($"{gameObject.name}: Could not create path to: {point}");
            }

            calulatingPath = false;
        }

#if UNITY_EDITOR // Don't include this in the build

        /// <summary>
        /// Draws some debug information for this object.
        /// </summary>
        private void OnDrawGizmos()
        {
            if (Application.isPlaying && inputManager.isPlayer == false)
            {
                Gizmos.color = Color.blue;

                Gizmos.DrawLine(_transform.position, targetPosition);
                Gizmos.DrawWireSphere(targetPosition, 1);
            }
        }

#endif
    }
}