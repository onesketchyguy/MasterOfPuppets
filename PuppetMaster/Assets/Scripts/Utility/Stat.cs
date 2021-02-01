using System.Collections.Generic;

namespace Player.Stats
{
    [System.Serializable]
    public class Stat
    {
        /// <summary>
        /// The starting value of this stat.
        /// </summary>
        [UnityEngine.SerializeField]
        public int baseValue = 1;

        /// <summary>
        /// A dictionary of values that modify the baseValue;
        /// </summary>
        public Dictionary<string, int> modifiers = new Dictionary<string, int>() { };

        /// <summary>
        /// Retrieve the value of this stat, in addition to all of it's modifiers.
        /// </summary>
        /// <returns></returns>
        public int GetValue()
        {
            int value = baseValue; //Set the starting value.

            foreach (var mod in modifiers)
            {
                value += mod.Value; //Add all the modifiers to the base value.
            }

            return value;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return GetValue().ToString();
        }

        public static bool operator ==(Stat a, int b)
        {
            return a.GetValue() == b;
        }

        public static bool operator !=(Stat a, int b)
        {
            return a.GetValue() != b;
        }

        public static bool operator ==(Stat a, float b)
        {
            return a.GetValue() == b;
        }

        public static bool operator !=(Stat a, float b)
        {
            return a.GetValue() != b;
        }
    }
}