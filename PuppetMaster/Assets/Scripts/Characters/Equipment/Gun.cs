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

        [SerializeField] private AudioClipPlayer audioSource;

        private void VerifySetup()
        {
            if (holder)
                holder = GetComponentInParent<CharacterInput>().transform;
        }

        public override void Use()
        {
            base.Use();
            VerifySetup();

            var time = Time.timeSinceLevelLoad;
            if (lastUse > time) return;

            lastUse = (time + info.yieldBetweenAttacks);

            // Instantiate a projectile and sent it in a direction
            var obj = Utility.ObjectPool.Get(info.projectilePrefab,
                firePoint.position, firePoint.rotation);

            var projectile = obj.GetComponent<Projectile>();
            projectile.SetSender(holder);
            projectile.SetDamage(info.weaponDamage);

            audioSource.PlayClip();
        }

        public override IEnumerator OnReload()
        {
            return base.OnReload();
        }
    }
}