using System.Collections;
using UnityEngine;
using Utility;

namespace Player.Stats
{
    public class CharacterStats : MonoBehaviour, IDamagable
    {
        public Container health;
        private Animator anim;

        public CoreStats CoreStats;

        /// <summary>
        /// The damage for this character to deal.
        /// </summary>
        public Stat damage;

        /// <summary>
        /// The armor value for this character.
        /// </summary>
        public Stat Armor;

        internal bool beingHit = false;

        public System.Action onHitCallBack;
        public System.Action onDiedCallback;

        private void Start()
        {
            CoreStats.InitializeStats();

            anim = GetComponentInChildren<Animator>();
        }

        public virtual void OnEnable()
        {
            health.MaxValue = CoreStats.GetStat(StatType.Endurance).GetValue() * 10;
            health.Refill();
        }

        public void TakeDamage(int damage, Transform damager)
        {
            var damageToDeal = damage;

            if (damageToDeal > 0)
            {
                health.ModifyValue(-damageToDeal);

                int poise = (CoreStats.GetStat(StatType.Dexterity).GetValue() +
                    Random.Range(-CoreStats.GetStat(StatType.Dexterity).GetValue(),
                    CoreStats.GetStat(StatType.Dexterity).GetValue()));

                if (damageToDeal > poise)
                {
                    StartCoroutine("Hit");
                }

                if (health.empty)
                {
                    Die();
                }
                else
                {
                    onHitCallBack?.Invoke();
                }
            }
        }

        public void RemoveBuff(Stat stat, string buffName, float waitTime = 0)
        {
            int o;

            stat.modifiers.TryGetValue(buffName, out o);

            if (o != 0) StartCoroutine(WaitRemoveBuff(stat, buffName, waitTime));
        }

        private IEnumerator WaitRemoveBuff(Stat stat, string buffName, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);

            stat.modifiers.Remove(buffName);
        }

        public virtual void Die()
        {
            Debug.Log($"{transform.name} died.");

            //Play animation.
            StartCoroutine(AnimateDeath());
        }

        public IEnumerator AnimateDeath()
        {
            AnimatorSetTrigger("Die");
            yield return new WaitForSeconds(1f);

            var offset = transform.position.z - 10;

            while (transform.position.z > offset)
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position - Vector3.up, Time.deltaTime);
                yield return new WaitForSeconds(Time.deltaTime);
            }

            gameObject.SetActive(false);
        }

        public IEnumerator Hit()
        {
            beingHit = true;

            AnimatorSetTrigger("Hit");
            yield return new WaitForSeconds(0.5f);

            beingHit = false;
        }

        public void AnimatorSetTrigger(string trigger)
        {
            if (anim != null) anim.SetTrigger(trigger);
        }
    }
}