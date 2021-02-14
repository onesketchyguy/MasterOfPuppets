using UnityEngine;

namespace PuppetMaster
{
    public class Gun : BaseWeapon
    {
        [SerializeField]
        private Transform firePoint = null;

        private float lastUse;

        [SerializeField] private AudioClipPlayer audioSource;

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

            audioSource.PlayClip();
        }
    }
}