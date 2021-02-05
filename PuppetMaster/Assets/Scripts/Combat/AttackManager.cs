using System.Collections;
using UnityEngine;
using Player.Input;
using Player.Stats;

namespace PuppetMaster
{
    public class AttackManager : MonoBehaviour, IActionInputReciever
    {
        public new Rigidbody rigidbody;
        private Transform _transform;

        public float turnSpeed = 30;

        private Vector3 lookDirection;

        /// <summary>
        /// TO BE MOVED TO WEAPON SCRIPT!
        /// </summary>
        public GameObject bulletPrefab;

        private void Start()
        {
            // Cache some values
            if (rigidbody == null) rigidbody = GetComponent<Rigidbody>();
            _transform = transform;
        }

        private void FixedUpdate()
        {
            // Look at cursor
            if (Time.frameCount % 3 == 0)
            {
                LookAtCursor();
            }
        }

        private void LookAtCursor()
        {
            lookDirection = Utility.Utilities.GetMouseOffsetFromObject(_transform, 1.1f);

            UpdateLookRotation();
        }

        private void UpdateLookRotation()
        {
            var targetDir = lookDirection + _transform.position;
            var localTarget = _transform.InverseTransformPoint(targetDir);

            var angle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

            var eulerAngleVelocity = new Vector3(0, angle, 0);
            var deltaRotation = Quaternion.Euler(eulerAngleVelocity * turnSpeed * Time.fixedDeltaTime);
            rigidbody.MoveRotation(rigidbody.rotation * deltaRotation);
        }

        public void OnFire1()
        {
            // Started attack input

            // Stop any existing fire routines
            StopAllCoroutines();

            // Start a new fire routine
            StartCoroutine(FireWeapon());
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

        private IEnumerator FireWeapon()
        {
            while (true)
            {
                // Fire off weapon

                // Instantiate a projectile and sent it in a direction
                // TO BE MOVE TO WEAPON SCRIPT
                var obj = Utility.ObjectPool.Get(bulletPrefab, transform.position + transform.forward, transform.rotation);

                yield return new WaitForEndOfFrame();
            }
        }
    }
}