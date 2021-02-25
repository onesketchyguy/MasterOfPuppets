using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace PuppetMaster.AI
{
    /// <summary>
    /// The states of an AI character.
    /// </summary>
    public enum AI_State
    {
        /// <summary>
        /// Wander, stand still, etc.
        /// </summary>
        idle,

        /// <summary>
        /// Flee, hide, etc.
        /// </summary>
        scared,

        /// <summary>
        /// Attack target, find cover, etc.
        /// </summary>
        fighting
    }

    public class AIBase : MonoBehaviour
    {
        internal static ScoreManager scoreManager;
        private const int UPDATE_FRAME_LIMIT = 3;

        /// <summary>
        /// The current state of this AI unit.
        /// </summary>
        public AI_State state { get; private set; }

        [Header("Required components")]
        [Tooltip("Combat component.")]
        [SerializeField] private CombatManager combatManager = null;

        [Tooltip("Input component.")]
        [SerializeField] private CharacterInput inputManager = null;

        [Tooltip("Health and such manager.")]
        [SerializeField] private CharacterStats stats = null;

        [Tooltip("Navigation manager.")]
        [SerializeField] private NavMeshAgent agent = null;

        [Header("Detection and combat")]
        [Tooltip("Distance that this character can see too.")]
        [SerializeField] private float visabilityRange = 10;

        [Tooltip("How long to wait in seconds before attacking again.")]
        [SerializeField] private float attackFrequency = 0.5f;

        [Header("Movement")]
        [Tooltip("How long to wait in seconds before moving again.")]
        [SerializeField] private float movementFrequency = 3;

        [Tooltip("How far away from a point this agent can stop.")]
        [SerializeField] private float stoppingDistance = 2;

        [Tooltip("Range to walk within.")]
        [SerializeField] private float walkRadius = 5;

        [Header("On death")]
        [Tooltip("Score to give when this character dies. (Assuming the player is not possessing this.)")]
        [SerializeField] private int scoreToGiveOnDeath = 100;

        [Tooltip("Listeners to notify when this character dies.")]
        [SerializeField] private UnityEngine.Events.UnityEvent onDeathEvent;

        /// <summary>
        /// Frame to wait until before updating.
        /// </summary>
        private int updateFrame = 0;

        /// <summary>
        /// Cached local transform
        /// </summary>
        private Transform _transform;

        /// <summary>
        /// A navmesh path to follow allong. The corners are specifically what we need.
        /// </summary>
        private NavMeshPath movementPath;

        /// <summary>
        /// The current index of the movementPath.corners the movement is trying to move to.
        /// </summary>
        private int pathIndex = 0;

        /// <summary>
        /// The target of this character. This could be an attacker, or someone we are attacking.
        /// </summary>
        private Transform target;

        /// <summary>
        /// Position to move to.
        /// </summary>
        private Vector3 targetPosition;

        /// <summary>
        /// The amount of time we've spent in this position.
        /// </summary>
        private float timeInPosition;

        /// <summary>
        /// The amount of time since the last attack.
        /// </summary>
        private float timeSinceAttack;

        /// <summary>
        /// A flag to let us know if we are already calculating a path.
        /// </summary>
        private bool calulatingPath = false;

        /// <summary>
        /// The direction the character is moving.
        /// </summary>
        private Vector3 inputDirection;

#if UNITY_EDITOR // Don't include this stuff in the build

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

        /// <summary>
        /// Runs in the editor on validation
        /// </summary>
        private void OnValidate()
        {
            if (agent == null)
                agent = GetComponent<NavMeshAgent>();

            if (inputManager == null)
                inputManager = GetComponent<CharacterInput>();

            if (stats == null)
                stats = GetComponent<CharacterStats>();
        }

#endif

        private void Awake()
        {
            // Try to set the scoreManager if one has not been set.
            if (scoreManager == null) scoreManager = FindObjectOfType<ScoreManager>();
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

                // Enable the navigation system
                if (agent.enabled == false) agent.enabled = true;

                // Handle the state machine
                switch (state)
                {
                    case AI_State.idle:
                        // Wander around and such
                        HandleIdleState();

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
                        // If the state is null we need to reset to idle.
                        SwapState(AI_State.idle);
                        break;
                }

                // Move the character
                HandleMovement();
            }
            else
            {
                // Don't do AI stuff

                // Disable the navigation system
                if (agent.enabled == true) agent.enabled = false;
            }
        }

        /// <summary>
        /// Things to happen when this character dies.
        /// </summary>
        private void OnDeath()
        {
            // Do some death stuff

            if (inputManager.isPlayer == false)
            {
                // Add score if not the player
                scoreManager.ModifyScore(scoreToGiveOnDeath);
            }

            // Try to notify any listeners.
            onDeathEvent?.Invoke();

            // FIXME: Don't do this.
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Changes the state of this object.
        /// </summary>
        /// <param name="_state"></param>
        public void SwapState(AI_State _state)
        {
            state = _state;
        }

        /// <summary>
        /// Sets the target for this object.
        /// </summary>
        /// <param name="target"></param>
        public void SetTarget(Transform _target)
        {
            target = _target;
        }

        /// <summary>
        /// Move this character by sending input to the input system.
        /// </summary>
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
                }
                else
                {
                    // Set the position to move to
                    targetPosition = movementPath.corners[pathIndex];

                    // Get the direction from the position
                    inputDirection = targetPosition - _transform.position;

                    // Check if we are close enough to stop or move on
                    if (Vector3.Distance(_transform.position, targetPosition) <= stoppingDistance)
                    {
                        // Move to the next item in the array
                        pathIndex++;
                    }
                }
            }

            // Convert the input from a vector3 into a vector2
            inputDirection.y = inputDirection.z;
            inputDirection.z = 0;

            // Feed the input to the input manager.
            inputManager.HandleMovement(inputDirection);
        }

        /// <summary>
        /// Creates a new position to wander towards
        /// </summary>
        private void FindWanderPosition()
        {
            // pick a random position within range that is on the navmesh
            Vector3 wanderTo;

            // Commit to a loop where we look for a point on the navmesh
            do
            {
                // Create a direction to move towards
                var wanderDirection = Random.insideUnitSphere * walkRadius;

                // Add our position to that direction to convert it into a world space
                wanderDirection += _transform.position;
                // Set the Y to our position to reset it
                wanderDirection.y = _transform.position.y;

                // Try to get a position from that point
                wanderTo = GetNavMeshPosition(wanderDirection);
            }
            while (wanderTo == Vector3.zero);

            // Generate a path to the point
            CalculatePath(wanderTo);
        }

        /// <summary>
        /// Do various idle things, such as wandering and standing around.
        /// </summary>
        private void HandleIdleState()
        {
            // If we are just chilling, let's get some action
            if (combatManager.isArmed && state != AI_State.fighting)
                SwapState(AI_State.fighting);

            // Find a wander point and move there
            if (Vector3.Distance(_transform.position, targetPosition) <= stoppingDistance)
            {
                if (timeInPosition > movementFrequency)
                {
                    // Find a new wonder pos
                    FindWanderPosition();
                }
                else
                {
                    // Increment the amount of time we have spent in this position.
                    timeInPosition += Time.deltaTime;
                }
            }
        }

        /// <summary>
        /// Run away from the target.
        /// </summary>
        private void HandleScaredState()
        {
            // Find a point farthest away from the attacker and run there

            if (target == null)
            {
                // Return to idle if the target no longer exists.
                SwapState(AI_State.idle);
            }
            else
            {
                // Generate a distance to run
                var distToTarget = Vector3.Distance(_transform.position, target.position);
                var runDist = Random.Range(10, 30) + distToTarget;

                // Create a point to flee to
                Vector3 fleeTo;

                do
                {
                    // Find a direction to move in
                    var direction = Random.insideUnitSphere * runDist;

                    // Add our position to that direction
                    direction += _transform.position;
                    // Reset the Y for calculations later
                    direction.y = _transform.position.y;

                    // Try to set a point to navigate to
                    fleeTo = GetNavMeshPosition(direction);

                    // Recalculate how far the point is from the target
                    distToTarget = Vector3.Distance(fleeTo, target.position);
                } while (distToTarget < runDist || fleeTo == Vector3.zero);

                // Generate a path to the point
                CalculatePath(fleeTo);
            }
        }

        /// <summary>
        /// Go to the target and shoot at them.
        /// </summary>
        private void HandleFightingState()
        {
            // Iterate the time since we've attacked
            timeSinceAttack += Time.deltaTime;

            // Stand within attacking distance and attack the target
            if (target == null)
            {
                SwapState(AI_State.idle);
            }
            else
            {
                // Try to attack
                if (timeSinceAttack > attackFrequency)
                {
                    // Check if we can even see the target
                    var targetVisable = CanSeeObject(target);

                    if (targetVisable)
                    {
                        // If the target is visable we should attack them
                        inputManager.Fire1_performed();

                        // Wait at least one frame between attacks
                        timeSinceAttack = -Time.deltaTime;
                    }
                    else
                    {
                        // If the target is not visable we should move
                        var point = target.position;

                        // Clamp the position to our range
                        point.x = Mathf.Clamp(_transform.position.x, point.x - visabilityRange, point.x + visabilityRange);
                        point.z = Mathf.Clamp(_transform.position.z, point.z - visabilityRange, point.z + visabilityRange);

                        // Create a point that we can move to on the nav mesh
                        var moveTo = GetNavMeshPosition(point);

                        // If the position we picked is null, find a wander position instead
                        if (moveTo == Vector3.zero)
                        {
                            FindWanderPosition();
                        }
                        else
                        {
                            // Generate a path to the point
                            CalculatePath(moveTo);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns true if the object is withing the line of sight.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns a position that can be moved to on the nav mesh.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
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
    }
}