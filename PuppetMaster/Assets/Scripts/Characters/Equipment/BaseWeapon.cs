using System.Collections;
using UnityEngine;

namespace PuppetMaster
{
    public class BaseWeapon : MonoBehaviour
    {
        [SerializeField] internal WeaponInfo info = null;

        public float damage
        {
            get
            {
                return info.weaponDamage;
            }
        }

        internal bool canUse = true;

        public virtual void Use()
        {
            if (canUse == false) return;

            // Use this shit on a mother fucker
        }

        public virtual IEnumerator OnReload()
        {
            canUse = false;

            yield return null;

            canUse = true;
        }
    }
}