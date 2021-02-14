using UnityEngine;

namespace PuppetMaster
{
    /// <summary>
    /// Stores weapon data in a scriptable object.
    /// </summary>
    [CreateAssetMenu(menuName = "Weapon/Weapon info", fileName = "New Weapon Info")]
    public class WeaponInfo : ScriptableObject
    {
        [Tooltip("Wait between attacks in seconds.")]
        [Range(0, 2f)]
        public float yieldBetweenAttacks = 1;

        [Tooltip("Prefab to use as a projectile. Leave empty to use no projectile.")]
        public GameObject projectilePrefab;

        [Tooltip("Amount of damage to deal apon impacting a damagable target.")]
        public int weaponDamage;
    }
}