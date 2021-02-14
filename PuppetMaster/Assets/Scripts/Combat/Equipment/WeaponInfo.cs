using UnityEngine;

namespace PuppetMaster
{
    [CreateAssetMenu(menuName = "Weapon/Weapon info", fileName = "New Weapon Info")]
    public class WeaponInfo : ScriptableObject
    {
        [Range(0, 2f)]
        public float yieldBetweenAttacks = 1;

        public GameObject projectilePrefab;
        public int weaponDamage;
    }
}