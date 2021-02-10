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

        [SerializeField] private int scoreToGiveOnDeath = 100;

        [Space]
        [SerializeField] private NavMeshAgent agent;

        [SerializeField] private CharacterInput inputManager;
        [SerializeField] private CharacterStats stats;

        public AI_State state;

        private int updateFrame = 0;
        private const int UPDATE_FRAME_LIMIT = 3;

        private Transform _trans;

        private NavMeshPath movementPath;
        private int pathIndex = 0;

        private Transform target;
        private Vector3 moveTo;
        private float timeInPosition;

        [Tooltip("How long to wait in seconds before moving again.")]
        [SerializeField] private float movementFrequency = 3;

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
            _trans = transform;

            // By choosing a random fram to choose to update on each AI
            // won't (necessarilly) be updating on the same frame.
            updateFrame = Random.Range(0, UPDATE_FRAME_LIMIT);

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
                if (agent.enabled == false)
                    agent.enabled = true;

                HandleStateMachine();
                HandleMovement();
            }
            else
            {
                // Dont do AI stuff
                if (agent.enabled == true)
                    agent.enabled = false;
            }
        }

        private void OnDeath()
        {
            // Do some died stuff

            if (inputManager.isPlayer == false)
                // Add score
                scoreManager.ModifyScore(scoreToGiveOnDeath);
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
            if (movementPath != null)
            {
                moveTo = movementPath.corners[pathIndex];

                var inputDirection = moveTo - transform.position;

                inputManager.HandleMovement(inputDirection);

                if (Vector3.Distance(_trans.position, moveTo) <= agent.stoppingDistance)
                {
                    pathIndex++;

                    if (pathIndex >= movementPath.corners.Length)
                    {
                        pathIndex = 0;
                        movementPath.ClearCorners();
                    }
                }
            }
        }

        private void HandleStateMachine()
        {
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
                    break;
            }
        }

        private void HandleIdleState()
        {
            // Find a wander point and move there

            if (Vector3.Distance(_trans.position, moveTo) <= agent.stoppingDistance)
            {
                if (timeInPosition > movementFrequency)
                {
                    // Find a new wonder pos
                    // FIXME: pick a random position within range that is on the navmesh

                    var wanderPoint = Random.insideUnitSphere; // this wont work yo
                    var moveTo = _trans.position + wanderPoint;

                    // Store the calculated path and move following that path
                    agent.CalculatePath(moveTo, movementPath);
                    timeInPosition = 0;
                }
                else
                {
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

                if (Vector3.Distance(_trans.position, target.position) < runDist)
                {
                    var distance = Random.insideUnitCircle * runDist;
                    var moveTo = _trans.position + new Vector3(distance.x, 0, distance.y);

                    // Store the calculated path and move following that path
                    agent.CalculatePath(moveTo, movementPath);
                    timeInPosition = 0;
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
                    // FIXME: Implement some sort of move to within range
                }
            }
        }

        public bool CanSeeObject(Transform obj)
        {
            // FIXME: launch a ray out and check if it hits the target object

            return false;
        }
    }
}