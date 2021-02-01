namespace Player.Stats
{
    public enum StatType
    {
        /// <summary>
        /// Measures: Natural athleticism, and bodily power.
        /// </summary>
        Strength,

        /// <summary>
        /// Measures: Physical agility, reflexes, balance, and poise.
        /// </summary>
        Dexterity,

        /// <summary>
        /// Measures: Health, stamina, and vital force.
        /// </summary>
        Endurance,

        /// <summary>
        /// Measures: Confidence, eloquence, and leadership.
        /// </summary>
        Charisma,

        /// <summary>
        /// Measures: Mental acuity, information recall, and analytical skill.
        /// </summary>
        Intelligence
    }

    [System.Serializable]
    public class CoreStats
    {
        private Stat[] stats;

        public void InitializeStats()
        {
            var array = System.Enum.GetNames(typeof(StatType));
            stats = new Stat[array.Length];
        }

        public Stat GetStat(StatType statToGet)
        {
            return stats[(int)statToGet];
        }

        public int GetAllStatValues()
        {
            int val = 0;

            for (int i = 0; i < stats.Length; i++)
            {
                val += stats[i].GetValue();
            }

            return val;
        }

        public int GetAllBaseValues()
        {
            int val = 0;

            for (int i = 0; i < stats.Length; i++)
            {
                val += stats[i].baseValue;
            }

            return val;
        }
    }
}