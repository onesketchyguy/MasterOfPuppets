using UnityEngine;

namespace PuppetMaster
{
    /// <summary>
    /// Projectile shooter.
    /// </summary>
    public class Gun : BaseWeapon
    {
        [Tooltip("Position to fire a projectile from.")]
        [SerializeField] private Transform firePoint = null;

        [Tooltip("Audio source to play the gun shots from.")]
        [SerializeField] private AudioClipPlayer audioSource;

        public override void Use()
        {
            base.Use();

            // Ensure we've waited long enough between uses
            var time = Time.timeSinceLevelLoad;
            if (lastUse > time) return;

            // Set the last use to now
            lastUse = (time + info.yieldBetweenAttacks);

            // Instantiate a projectile and sent it in a direction
            var obj = Utility.ObjectPool.Get(info.projectilePrefab,
                firePoint.position, firePoint.rotation);
            var projectile = obj.GetComponent<Projectile>();

            // Set the projectile variables
            projectile.SetSender(holder);
            projectile.SetDamage(info.weaponDamage);

            // Play a shot sound
            audioSource.PlayClip();
        }
    }
}