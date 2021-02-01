using System.Collections;
using UnityEngine;
using Convienience;

namespace Player.Stats
{
    public class CharacterStats : MonoBehaviour, IDamagable
    {
        new public string name;

        public Container health;
        public Container experience;

        public int level = 1;

        internal int statPoints;

        private Animator anim;

        private void Start()
        {
            anim = GetComponentInChildren<Animator>();
        }

        private int CalculateStatPointRoll()
        {
            int toReturn = 0;

            int passes = 0;

            while (passes < 5)
            {
                toReturn += Random.Range(1, 3);

                passes++;
            }

            return toReturn;
        }

        public void RemoveBuff(Stat stat, string buffName, float waitTime = 0)
        {
            int o = 0;

            stat.modifiers.TryGetValue(buffName, out o);

            if (o != 0 && waitTime > 0) StartCoroutine(WaitRemoveBuff(stat, buffName, waitTime));
            else if (o != 0) stat.modifiers.Remove(buffName);
        }

        private IEnumerator WaitRemoveBuff(Stat stat, string buffName, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);

            stat.modifiers.Remove(buffName);
        }

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

        public delegate void OnHitEvent();

        public OnHitEvent OnHitCallBack;

        public virtual void OnEnable()
        {
            experience.MaxValue = 40;

            statPoints = CalculateStatPointRoll();

            experience.ModifyValue(-experience.MaxValue);

            UpdateStats();

            health.ModifyValue(health.MaxValue);

            gameObject.name = name;

            health.OnValueModifiedCallback += HPChanged;
        }

        private void LateUpdate()
        {
            UpdateStats();
        }

        public void TakeDamage(int damage, Transform damager)
        {
            //int minDamage = damage - armor.GetValue();
            //minDamage = Mathf.Clamp(minDamage, 0, int.MaxValue);
            //int maxDamage = damage * 2;
            //int damageToDeal = Standards.useRandomDamage ? Random.Range(minDamage, maxDamage) : minDamage;

            var damageToDeal = damage;

            //string damageText = damageToDeal > 0 ? damageToDeal.ToString() : "miss";
            //Color color = damageToDeal < damage ? Color.blue : damageToDeal < maxDamage ? Color.white : Color.red; // Replace with a proper find of crit and min
            //float randomArea = 0.5f;
            //Vector3 ran = new Vector3(Random.Range(-randomArea, randomArea), Random.Range(-randomArea, randomArea), Random.Range(-randomArea, randomArea));
            //uiManager.get.DrawDamageText(transform.position + ran, $"{damageText}", color);

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
                    var stats = damager.gameObject.GetComponent<CharacterStats>();

                    if (stats != null)
                    {
                        int experienceToGive = CoreStats.GetAllStatValues() +
                            (CoreStats.GetAllStatValues() / Random.Range(1, 5));

                        stats.experience.ModifyValue(experienceToGive);
                    }

                    Die();
                    return;
                }

                if (OnHitCallBack != null)
                    OnHitCallBack.Invoke();
            }
        }

        private void HPChanged(float newValue)
        {
            if (OnHitCallBack != null)
                OnHitCallBack.Invoke();
        }

        public virtual void Die()
        {
            Debug.Log($"{transform.name} died.");

            //Play animation.

            StartCoroutine(AnimateDeath());
        }

        private void UpdateStats()
        {
            float lastHealth = health.MaxValue;

            if (experience.value == experience.MaxValue)
            {
                experience.ModifyValue(-experience.MaxValue);

                experience.MaxValue = ((experience.MaxValue * 2) + (experience.MaxValue / 2)) -
                    (CoreStats.GetStat(StatType.Intelligence).GetValue() * Random.Range(0, level));

                level++;
                statPoints++;
            }

            health.MaxValue = (CoreStats.GetStat(StatType.Endurance).GetValue() * 10);

            if (health.MaxValue != lastHealth)
            {
                if (OnHitCallBack != null)
                    OnHitCallBack.Invoke();
            }
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