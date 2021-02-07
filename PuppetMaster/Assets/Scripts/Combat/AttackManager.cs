using System.Collections;
using UnityEngine;
using Player.Input;
using Player.Stats;

namespace PuppetMaster
{
    public class AttackManager : MonoBehaviour, IActionInputReciever
    {
        [SerializeField] private Rigidbody rigidBody;
        private Transform _transform;

        [SerializeField] private float turnSpeed = 30;

        private Vector3 lookDirection;

        public BaseWeapon startWeapon;

        private BaseWeapon weapon;

        [SerializeField] private Transform weaponParentObject;

        [SerializeField] private Transform lookTargetObject;

        private Vector3 upOffset;

        /// <summary>
        /// TO BE MOVED TO WEAPON SCRIPT!
        /// </summary>
        public GameObject bulletPrefab;

        private void Start()
        {
            // Cache some values
            if (rigidBody == null) rigidBody = GetComponent<Rigidbody>();
            _transform = transform;

            upOffset = Vector3.up * 0.25f;

            // Equip the starter weapon if one exists
            if (startWeapon != null) SwapWeapon(startWeapon);
        }

        private void FixedUpdate()
        {
            // Look at cursor
            if (Time.frameCount % 3 == 0)
            {
                LookAtCursor();
            }
        }

        public void SwapWeapon(BaseWeapon weaponObject)
        {
            // Destroy any existing children
            var items = weaponParentObject.GetComponentsInChildren<BaseWeapon>();

            if (items != null && items.Length > 0)
            {
                for (int i = items.Length - 1; i >= 0; i--)
                    Destroy(items[i].gameObject);
            }

            // Add the weapon to the slot
            var go = Instantiate(weaponObject.gameObject, weaponParentObject);
            go.name = weaponObject.gameObject.name;

            weapon = go.GetComponent<BaseWeapon>();
        }

        private void LookAtCursor()
        {
            lookDirection = Utility.Utilities.GetMouseOffsetFromObject(_transform, 1.1f);

            lookTargetObject.position = _transform.position + lookDirection + upOffset;

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