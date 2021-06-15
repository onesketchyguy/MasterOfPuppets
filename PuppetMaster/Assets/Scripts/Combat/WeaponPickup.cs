using UnityEngine;

namespace PuppetMaster
{
    /// <summary>
    /// A pickup object which requires a base weapon in order to work.
    /// </summary>
    public class WeaponPickup : MonoBehaviour, IInteractable
    {
        [Tooltip("The weapon to pickup once in range of this object.")]
        [SerializeField] private BaseWeapon weapon = null;

        [Tooltip("The speed at which to rotate around the 'z' axis.")]
        [SerializeField] private float rotateSpeed = 30;

        /// <summary>
        /// A cached transform.
        /// </summary>
        private Transform _transform;

        /// <summary>
        /// Returns true if this object has a parent.
        /// </summary>
        /// <returns></returns>
        private bool GetBeingHeld()
        {
            return _transform != null && _transform.parent != null;
        }

        /// <summary>
        /// Called once every time this component needs validation within the editor.
        /// </summary>
        private void OnValidate()
        {
            // If possible try setting the weapon property automatically.
            if (weapon == null) weapon = GetComponent<BaseWeapon>();
        }

        private void Start()
        {
            // Cache the transform.
            _transform = transform;
        }

        private void Update()
        {
            // Don't rotate if currently being held.
            if (GetBeingHeld() == true) return;

            // Rotate around the 'z' axis based on the frame rate.
            _transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Picks this weapon up.
        /// </summary>
        /// <param name="sender"></param>
        public void Interact(GameObject sender)
        {
            // Try to get the combat manager component from the sender.
            var picker = sender.GetComponent<CombatManager>();

            if (picker != null)
            {
                // Try to equip this weapon.
                picker.SwapWeapon(weapon);
            }
        }
    }
}