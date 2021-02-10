using Player.Stats;
using UnityEngine;
using Utility;

namespace PuppetMaster
{
    public class CharacterStats : MonoBehaviour, IDamagable
    {
        public Container health;
        public Container armor;

        public System.Action onHitCallback;
        public System.Action onDiedCallback;

        public Transform lastAttacker { get; private set; }

        public void TakeDamage(int damage, Transform damager)
        {
            lastAttacker = damager;

            if (armor.empty)
            {
                // Deal direct damage
                health.ModifyValue(-damage);
            }
            else
            {
                // Deal damage with armor
                damage = Mathf.CeilToInt(damage * Random.Range(0.1f, 0.9f));

                armor.ModifyValue(-damage);

                var dam = Mathf.Clamp(damage - armor.value, 0, damage);
                health.ModifyValue(-dam);
            }

            if (health.empty)
            {
                onDiedCallback?.Invoke();
            }
            else
            {
                onHitCallback?.Invoke();
            }
        }
    }
}