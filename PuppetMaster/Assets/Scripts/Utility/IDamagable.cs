using UnityEngine;

namespace Player.Stats
{
    public interface IDamagable
    {
        void TakeDamage(int damage, Transform damager);
    }
}