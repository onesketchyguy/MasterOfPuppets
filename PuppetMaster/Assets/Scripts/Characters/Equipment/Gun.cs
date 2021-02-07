using System.Collections;
using UnityEngine;

namespace PuppetMaster
{
    public class Gun : BaseWeapon
    {
        [SerializeField]
        private Transform firePoint = null;

        private float lastUse;

        private Transform holder;

        private void Start()
        {
            holder = GetComponentInParent<CharacterInputController>().transform;
        }

        public override void Use()
        {
            base.Use();

            var time = Time.timeSinceLevelLoad;
            if (lastUse > time) return;

            lastUse = (time + info.yieldBetweenAttacks);

            // Instantiate a projectile and sent it in a direction
            var obj = Utility.ObjectPool.Get(info.projectilePrefab,
                firePoint.position, firePoint.rotation);

            var projectile = obj.GetComponent<Projectile>();
            projectile.SetSender(holder);
            projectile.SetDamage(info.weaponDamage);
        }

        public override IEnumerator OnReload()
        {
            return base.OnReload();
        }
    }
}