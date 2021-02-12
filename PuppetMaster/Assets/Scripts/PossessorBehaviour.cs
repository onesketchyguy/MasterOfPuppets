using UnityEngine;
using Player.Input;

namespace PuppetMaster
{
    public class PossessorBehaviour : MonoBehaviour, IActionInputReciever
    {
        [SerializeField] private LayerMask possessableMask = 0;
        [SerializeField] private float overlapRadius = 3;

        [Space]
        [SerializeField] private GameObject display;

        private Transform _tran;
        private Transform _possessed;

        [SerializeField] private UnityEngine.Events.UnityEvent onPossessionEvent;

        private CharacterInput GetPossessing()
        {
            var r = CharacterInput.playerControlled;

            if (r.gameObject == gameObject) return null;

            return r;
        }

        private void Start()
        {
            _tran = transform;
        }

        private void Update()
        {
            if (GetPossessing() == null && display.activeSelf == false)
            {
                // Set our visuals to active.
                display.SetActive(true);
            }
            else if (GetPossessing() != null && display.activeSelf == true)
            {
                // Set our visuals to inactive.
                display.SetActive(false);
            }
            else if (GetPossessing() != null)
            {
                // Move to the actively controlled character
                _tran.position = _possessed.position;
            }
        }

        public void OnFire1()
        {
            // Posess a body
            var collisions = Physics.OverlapSphere(_tran.position, overlapRadius, possessableMask);

            if (collisions != null)
            {
                // Get the closest possessable object
                Collider closest = null;
                var distToClosest = overlapRadius;

                foreach (var item in collisions)
                {
                    if (item == closest || item.transform == _tran) continue;

                    // Check if this object is closer than the stored closest object
                    var dist = Vector3.Distance(item.transform.position, _tran.position);
                    if (dist < distToClosest)
                    {
                        closest = item;
                        distToClosest = dist;
                    }
                }

                // Possess the closest character
                Possess(closest.gameObject);
            }
        }

        public void OnFire1Up()
        {
        }

        public void OnFire2()
        {
        }

        public void OnFire2Up()
        {
        }

        private void Possess(GameObject obj)
        {
            onPossessionEvent?.Invoke();

            // Set the object to recieve player input
            CharacterInput.playerControlled = obj.GetComponent<CharacterInput>();
            _possessed = obj.transform;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, overlapRadius);
        }
    }
}