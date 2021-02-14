using System.Collections;
using UnityEngine;
using Player.Input;
using System.Collections.Generic;

namespace PuppetMaster
{
    public class CombatManager : MonoBehaviour, IActionInputReceiver
    {
        [SerializeField] private Rigidbody rigidBody;
        private Transform _transform;

        public BaseWeapon startWeapon;

        private BaseWeapon weapon;

        [SerializeField] private Transform weaponParentObject;

        [SerializeField] private float weaponPickupRange = 3;

        private Collider[] inRangeColliders;
        private HashSet<BaseWeapon> inRangeWeapons = new HashSet<BaseWeapon>();
        internal bool armed => weapon != null;

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
                inRangeWeapons.Clear();

                // Do not attempt to continue if there is nothing to work with
                if (inRangeColliders == null || inRangeColliders.Length < 1) return;

                // Go through each item in the nearby objects
                foreach (var item in inRangeColliders)
                {
                    // If this item is a weapon add it to the weapon list
                    var _weapon = item.GetComponent<BaseWeapon>();
                    if (_weapon != null) inRangeWeapons.Add(_weapon);
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

            if (inRange != null) EquipWeapon(inRange);
        }

        public void SwapWeapon(BaseWeapon weaponObject)
        {
            // Drop the current weapon
            DropWeapon();

            // Add the weapon to the slot
            EquipWeapon(weaponObject);
        }

        public void EquipWeapon(BaseWeapon weaponObject)
        {
            Debug.Log($"Equipping weapon: {weaponObject.name}");

            weaponObject.transform.SetParent(weaponParentObject);
            weaponObject.transform.position = weaponParentObject.position;
            weaponObject.transform.rotation = weaponParentObject.rotation;

            weapon = weaponObject;
        }

        public void DropWeapon()
        {
            if (weapon != null)
            {
                weapon.transform.position = transform.position + Vector3.up;
                weapon.transform.SetParent(null);

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
        }

        public void OnFire2Up()
        {
            // Pickup the closest weapon
            EquipClosestWeapon();
        }

        private IEnumerator UseWeapon()
        {
            while (true)
            {
                if (weapon == null) break;

                // Use this weapon
                weapon.Use();

                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForEndOfFrame();
        }
    }
}