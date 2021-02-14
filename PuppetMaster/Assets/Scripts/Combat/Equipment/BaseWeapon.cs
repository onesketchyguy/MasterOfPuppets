using UnityEngine;

namespace PuppetMaster
{
    public class BaseWeapon : MonoBehaviour
    {
        [SerializeField] internal WeaponInfo info = null;

        public bool beingHeld
        {
            get
            {
                return holder != null;
            }
        }

        protected Transform holder;

        public void SetHolder(Transform sender)
        {
            holder = sender;
        }

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
    }
}