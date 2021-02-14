using System.Collections;
using UnityEngine;
using Player.Input;
using System.Collections.Generic;

namespace PuppetMaster
{
    public class CombatManager : MonoBehaviour, IActionInputReceiver
    {
        [SerializeField] private Rigidbody rigidBody;
        [SerializeField] private BaseWeapon startWeapon;

        [SerializeField] private Transform weaponParentObject;

        [SerializeField] private float weaponPickupRange = 3;
        public BaseWeapon weapon { get; private set; }

        private Collider[] inRangeColliders;
        private HashSet<BaseWeapon> inRangeWeapons = new HashSet<BaseWeapon>();

        internal bool armed => weapon != null;
        private Transform _transform;

        private void Start()
        {
            // Cache some values
            if (rigidBody == null) rigidBody = GetComponent<Rigidbody>();
            _transform = transform;

            // Equip the starter weapon if one exists
            if (startWeapon != null) SwapWeapon(startWeapon);
        }

        private void FixedUpdate()
        {
            // Update nearby objects
            UpdateLocalObjects();
        }

        /// <summary>
        /// Updates the objects within range
        /// NOTE: This could be inside a dedicated interaction script
        /// </summary>
        private void UpdateLocalObjects()
        {
            // On the first of every 3 frames update the local objects with a physics update
            if (Time.frameCount % 3 == 0)
            {
                // Setup all the in range objects
                inRangeColliders = Physics.OverlapSphere(_transform.position, weaponPickupRange);
            }

            // On the second of every 3 frames update the objects with a foreach loop
            if (Time.frameCount % 3 == 1)
            {
                // Clear the list before beginning the search
                inRangeWeapons.Clear();

                // Do not attempt to continue if there is nothing to work with
                if (inRangeColliders == null || inRangeColliders.Length < 1) return;

                // Go through each item in the nearby objects
                foreach (var item in inRangeColliders)
                {
                    // If this item is a weapon add it to the weapon list
                    var _weapon = item.GetComponent<BaseWeapon>();
                    if (_weapon != null)
                    {
                        // If this weapon is already being held then fuck right off, bud
                        if (_weapon.beingHeld) continue;

                        // Add this weapon to our list of local weapons
                        inRangeWeapons.Add(_weapon);
                    }
                }

                // If not holding a weapon try to equip one
                if (weapon == null) EquipClosestWeapon();
            }
        }

        private void EquipClosestWeapon()
        {
            if (inRangeWeapons == null || inRangeWeapons.Count < 1) return;

            var closest = weaponPickupRange;

            BaseWeapon inRange = null;

            foreach (var baseWeapon in inRangeWeapons)
            {
                if (baseWeapon != weapon)
                {
                    var dist = Vector3.Distance(_transform.position, baseWeapon.transform.position);

                    if (dist < closest)
                    {
                        closest = dist;
                        inRange = baseWeapon;
                    }
                }
            }

            if (inRange != null) SwapWeapon(inRange);
        }

        /// <summary>
        /// Drops any existing weapon, and equips a designated one.
        /// </summary>
        /// <param name="weaponObject"></param>
        public void SwapWeapon(BaseWeapon weaponObject)
        {
            // Drop the current weapon
            DropWeapon();

            // Add the weapon to the slot
            EquipWeapon(weaponObject);
        }

        /// <summary>
        /// Equip a weapon to the weapon slot.
        /// </summary>
        /// <param name="weaponObject"></param>
        public void EquipWeapon(BaseWeapon weaponObject)
        {
            // Set the object to the pivot spot
            weaponObject.transform.SetParent(weaponParentObject);
            weaponObject.transform.position = weaponParentObject.position;
            weaponObject.transform.rotation = weaponParentObject.rotation;

            // Set the local data
            weapon = weaponObject;

            // Let this little honey know who's boss
            weapon.SetHolder(transform);
        }

        /// <summary>
        /// Drops the current weapon if one exists.
        /// </summary>
        public void DropWeapon()
        {
            if (weapon != null)
            {
                // Drop the current weapon on the ground where we are standing
                weapon.transform.position = transform.position;
                weapon.transform.SetParent(null);

                // Clear the rotation at to reset the object
                weapon.transform.rotation = Quaternion.identity;

                // Let the weapon know we don't need it any more, sexually or otherwise
                weapon.SetHolder(null);
                weapon = null;
            }
        }

        public void OnFire1()
        {
            // Started attack input

            // Stop any existing fire routines
            StopAllCoroutines();

            // Start a new fire routine
            StartCoroutine(UseWeapon());
        }

        public void OnFire1Up()
        {
            // Ended attack input

            // Stop any existing fire routines
            StopAllCoroutines();
        }

        public void OnFire2()
        {
            // Not being used
        }

        public void OnFire2Up()
        {
            // Pickup the closest weapon
            EquipClosestWeapon();
        }

        /// <summary>
        /// Iterativly call the use function of the weapon.
        /// </summary>
        /// <returns></returns>
        private IEnumerator UseWeapon()
        {
            while (true)
            {
                if (weapon == null) yield break;

                // Use this weapon
                weapon.Use();

                yield return new WaitForEndOfFrame();
            }
        }
    }
}