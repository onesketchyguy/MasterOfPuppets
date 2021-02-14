using UnityEngine;

namespace PuppetMaster
{
    public class BaseWeapon : MonoBehaviour
    {
        [Tooltip("The core information for this weapon.")]
        [SerializeField] internal WeaponInfo info = null;

        /// <summary>
        /// Details the last time this weapon was fired.
        /// </summary>
        protected float lastUse;

        /// <summary>
        /// Returns true if there is a holder.
        /// </summary>
        public bool beingHeld
        {
            get
            {
                return holder != null;
            }
        }

        /// <summary>
        /// Returns the character currently holding this weapon.
        /// </summary>
        protected Transform holder { get; private set; }

        /// <summary>
        /// Sets the value of the holder.
        /// </summary>
        /// <param name="sender">Who's holding this weapon.</param>
        public void SetHolder(Transform sender)
        {
            holder = sender;
        }

        /// <summary>
        /// Returns the damage value from the info object.
        /// </summary>
        public int damage
        {
            get
            {
                return info.weaponDamage;
            }
        }

        /// <summary>
        /// If false prevents the weapon from being used.
        /// </summary>
        internal bool canUse = true;

        /// <summary>
        /// Trigger this weapon.
        /// </summary>
        public virtual void Use()
        {
            if (canUse == false) return;

            // Use this shit on a mother fucker
        }
    }
}