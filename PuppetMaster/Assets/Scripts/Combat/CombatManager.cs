using System.Collections;
using UnityEngine;
using Player.Input;
using System.Collections.Generic;

namespace PuppetMaster
{
    public class CombatManager : MonoBehaviour, IActionInputReciever
    {
        [SerializeField] private Rigidbody rigidBody;
        private Transform _transform;

        [SerializeField] private float turnSpeed = 30;

        private Vector3 lookDirection;

        public BaseWeapon startWeapon;

        private BaseWeapon weapon;

        [SerializeField] private Transform weaponParentObject;

        [SerializeField] private Transform lookTargetObject;

        [SerializeField] private float weaponPickupRange = 3;

        private Vector3 verticalOffset;

        private Collider[] inRangeColliders;
        private HashSet<BaseWeapon> inRangeWeapons = new HashSet<BaseWeapon>();

        private void Start()
        {
            // Cache some values
            if (rigidBody == null) rigidBody = GetComponent<Rigidbody>();
            _transform = transform;

            verticalOffset = Vector3.up * 0.25f;

            // Equip the starter weapon if one exists
            if (startWeapon != null) SwapWeapon(startWeapon);
        }

        private void FixedUpdate()
        {
            // Look at cursor
            if (Time.frameCount % 3 == 0)
            {
                LookAtCursor();

                // Update nearby objects
                inRangeColliders = Physics.OverlapSphere(_transform.position, weaponPickupRange);

                inRangeWeapons.Clear();

                foreach (var item in inRangeColliders)
                {
                    var _weapon = item.GetComponent<BaseWeapon>();

                    if (_weapon != null)
                    {
                        inRangeWeapons.Add(_weapon);
                    }
                }

                if (weapon == null)
                {
                    EquipClosestWeapon();
                }
            }
        }

        private void EquipClosestWeapon()
        {
            if (inRangeColliders == null || inRangeColliders.Length < 1) return;

            var closest = weaponPickupRange;

            BaseWeapon inRange = null;

            foreach (var baseWeapon in inRangeWeapons)
            {
                if (baseWeapon)
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

        private void LookAtCursor()
        {
            lookDirection = Utility.Utilities.GetMouseOffsetFromObject(_transform, 1.1f);

            lookTargetObject.position = _transform.position + lookDirection + verticalOffset;

            UpdateLookRotation();
        }

        private void UpdateLookRotation()
        {
            var targetDir = lookDirection + _transform.position;
            var localTarget = _transform.InverseTransformPoint(targetDir);

            var angle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

            var eulerAngleVelocity = new Vector3(0, angle, 0);
            var deltaRotation = Quaternion.Euler(eulerAngleVelocity * turnSpeed * Time.fixedDeltaTime);
            rigidBody.MoveRotation(rigidBody.rotation * deltaRotation);
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
            // Pickup the closest weapon
            EquipClosestWeapon();
        }

        public void OnFire2Up()
        {
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