using UnityEngine;

namespace PuppetMaster
{
    public class WeaponPickup : MonoBehaviour, IInteractable
    {
        [SerializeField] private BaseWeapon weapon = null;
        [SerializeField] private float turnSpeed = 30;

        private Transform _tran;

        private bool GetBeingHeld()
        {
            return transform.parent != null;
        }

        private void OnValidate()
        {
            if (weapon == null)
            {
                weapon = GetComponent<BaseWeapon>();
            }
        }

        private void Start()
        {
            _tran = transform;
        }

        private void Update()
        {
            var beingHeld = GetBeingHeld();
            if (beingHeld) return;

            _tran.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
        }

        public void Interact(GameObject sender)
        {
            var picker = sender.GetComponent<CombatManager>();

            picker.SwapWeapon(weapon);
        }
    }
}