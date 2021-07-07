using UnityEngine;

namespace PuppetMaster.CharacterCreation
{
    [CreateAssetMenu(menuName = "APPack/Character type")]
    public class CharacterType : ScriptableObject
    {
        public bool canBePossessed = true;
        public bool canExercise = false;
        public bool canHeal = false;
        public bool canPutOutFire = false;

        [Range(0, 100)]
        [SerializeField] public int findPriestChance = 10;

        [Range(1.0f, 10.0f)]
        [SerializeField] private float characterSpeed = 5.0f;

        [Range(1.0f, 100.0f)]
        [SerializeField] private float characterHealth = 10.0f;

        [SerializeField] private int pointsOnKill = 10;

        [Range(0, 100)]
        [SerializeField] private int baseAgression = 25;

        [Tooltip("Range of mutation on the base aggression. " +
            "This will be added to this character at random, and will apply on top of the base aggression." +
            "Please note that this will be used in addition with a range between -'range' and 'range'.")]
        [Range(0, 100)]
        [SerializeField] private int aggressionRange = 5;

        public int GetAggression()
        {
            return Mathf.Clamp(baseAgression + Random.Range(-aggressionRange, aggressionRange), 0, 100);
        }

        private void OnValidate()
        {
            float val = Mathf.CeilToInt(pointsOnKill / 10.0f);
            pointsOnKill = (int)(val * 10);
        }
    }
}