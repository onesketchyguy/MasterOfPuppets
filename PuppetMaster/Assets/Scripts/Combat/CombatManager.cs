using System.Collections;
using UnityEngine;
using Player.Input;
using System.Collections.Generic;

namespace PuppetMaster
{
    public class CombatManager : MonoBehaviour, IActionInputReceiver
    {
        [Header("Setup fields")]
        [SerializeField] private Rigidbody rigidBody;

        [Tooltip("Weapon to have on start. Leave empty to start unarmed.")]
        [SerializeField] private BaseWeapon startWeapon;

        [Tooltip("Object to use to hold onto the weapon.")]
        [SerializeField] private Transform weaponParentObject;

        [Tooltip("How close this transform can be before it can be picked up.")]
        [SerializeField] private float weaponPickupRange = 3;

        /// <summary>
        /// Returns the currently equipped weapon.
        /// </summary>
        public BaseWeapon weapon { get; private set; }

        /// <summary>
        /// Stores all objects within range.
        /// </summary>
        private Collider[] inRangeColliders;

        /// <summary>
        /// Stores all weapons in range.
        /// </summary>
        private HashSet<BaseWeapon> inRangeWeapons = new HashSet<BaseWeapon>();

        /// <summary>
        /// Returns whether or not this character is armed.
        /// </summary>
        internal bool armed => weapon != null;

        /// <summary>
        /// A cached transform.
        /// </summary>
        private Transform _transform;

        private void Start()
        {
            // Cache some values
            if (rigidBody == null) rigidBody = GetComponent<Rigidbody>();
            _transform = transform;

            // Equip the starter weapon if one exists
            if (startWeapon != null) SwapWeapon(startWeapon);
        }

        private void Update()
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

        /// <summary>
        /// Finds the closest weapon to this object, and equips it.
        /// </summary>
        private void EquipClosestWeapon()
        {
            // If no weapons are in range don't continue.
            if (inRangeWeapons == null || inRangeWeapons.Count < 1) return;

            // Store some information on which object is closest.
            BaseWeapon inRange = null;
            float closest = weaponPickupRange;

            // Iterate through each object to find the closest one.
            foreach (var baseWeapon in inRangeWeapons)
            {
                // Continue if the current weapon is the weapon currently being held.
                if (baseWeapon != weapon)
                {
                    // Store the distance to the current iteration object
                    var dist = Vector3.Distance(_transform.position, baseWeapon.transform.position);

                    // If this object is this closest one...
                    if (dist < closest)
                    {
                        // Go ahead and update the closest object to be this object.
                        closest = dist;
                        inRange = baseWeapon;
                    }
                }
            }

            // If there is a weapon in range equip it.
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