using System.Collections.Generic;

namespace Utility
{
    /// <summary>
    /// Holds a value for the user.
    /// Contains a max, and a minimum.
    /// </summary>
    [System.Serializable]
    public class Container
    {
        /// <summary>
        /// A callback delegate for the value changing, outs the current health.
        /// </summary>
        public Utilities.ValueModified OnValueModifiedCallback;

        /// <summary>
        /// A callback delegate for the value changing, outs the change amount
        /// </summary>
        public Utilities.ValueModified OnValueModifiedCallback_ChangeAmount;

        /// <summary>
        /// The max this value can reach.
        /// </summary>
        public int MaxValue = 30;

        /// <summary>
        /// Returns the values empty state.
        /// Current value less than or equal to zero;
        /// </summary>
        public bool empty => value <= 0;

        public float value { get; private set; } = 30;

        /// <summary>
        /// Changes the health of the character in question.
        /// </summary>
        /// <param name="mod">Use a negative value to damage, and a posative to heal.</param>
        public void ModifyValue(float mod)
        {
            // Add the modification to the currentValue, then clamp it between the max and zero.
            float newValue = (value + mod >= MaxValue) ? MaxValue : (value + mod > 0) ? value + mod : 0;

            // Apply the new value.
            value = newValue;

            // Callback to any listeners.
            if (OnValueModifiedCallback != null)
            {
                OnValueModifiedCallback.Invoke(value);
            }
            if (OnValueModifiedCallback_ChangeAmount != null)
            {
                float modAmount = (value + mod >= MaxValue) ? 0 : (value + mod > 0) ? mod : 0;

                OnValueModifiedCallback_ChangeAmount.Invoke(modAmount);
            }
        }

        public void Refill()
        {
            value = MaxValue;
        }

        public void Clear()
        {
            value = 0;
        }

        public Container(int maxValue, Utilities.ValueModified onValueModifiedCallback)
        {
            OnValueModifiedCallback += onValueModifiedCallback;
            MaxValue = maxValue;
        }

        public Container(int maxValue = 3, float currentValue = -1)
        {
            if (currentValue == -1)
            {
                value = maxValue;
            }
            else
            {
                value = currentValue;
            }
            MaxValue = maxValue;
        }

        #region Operators

        public override bool Equals(object obj)
        {
            return obj is Container container &&
                   EqualityComparer<Utilities.ValueModified>.Default.Equals(OnValueModifiedCallback, container.OnValueModifiedCallback) &&
                   EqualityComparer<Utilities.ValueModified>.Default.Equals(OnValueModifiedCallback_ChangeAmount, container.OnValueModifiedCallback_ChangeAmount) &&
                   MaxValue == container.MaxValue &&
                   empty == container.empty &&
                   value == container.value;
        }

        public override int GetHashCode()
        {
            var hashCode = -426099894;
            hashCode = hashCode * -1521134295 + EqualityComparer<Utilities.ValueModified>.Default.GetHashCode(OnValueModifiedCallback);
            hashCode = hashCode * -1521134295 + EqualityComparer<Utilities.ValueModified>.Default.GetHashCode(OnValueModifiedCallback_ChangeAmount);
            hashCode = hashCode * -1521134295 + MaxValue.GetHashCode();
            hashCode = hashCode * -1521134295 + empty.GetHashCode();
            hashCode = hashCode * -1521134295 + value.GetHashCode();
            return hashCode;
        }

        #region Float

        public static bool operator ==(Container x, float y)
        {
            return x.value == y;
        }

        public static bool operator !=(Container x, float y)
        {
            return x.value != y;
        }

        public static bool operator >=(Container x, float y)
        {
            return x.value >= y;
        }

        public static bool operator <=(Container x, float y)
        {
            return x.value <= y;
        }

        public static bool operator >(Container x, float y)
        {
            return x.value >= y;
        }

        public static bool operator <(Container x, float y)
        {
            return x.value <= y;
        }

        #endregion Float

        #region Int

        public static bool operator ==(Container x, int y)
        {
            return x.value == y;
        }

        public static bool operator !=(Container x, int y)
        {
            return x.value != y;
        }

        public static bool operator >=(Container x, int y)
        {
            return x.value >= y;
        }

        public static bool operator <=(Container x, int y)
        {
            return x.value <= y;
        }

        public static bool operator >(Container x, int y)
        {
            return x.value >= y;
        }

        public static bool operator <(Container x, int y)
        {
            return x.value <= y;
        }

        #endregion Int

        #endregion Operators
    }
}