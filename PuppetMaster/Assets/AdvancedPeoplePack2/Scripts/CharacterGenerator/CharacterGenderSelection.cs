using AdvancedCustomizableSystem;
using UnityEngine;

namespace PuppetMaster.CharacterCreation
{
    public class CharacterGenderSelection : MonoBehaviour
    {
        // FIXME: Keep track of male and female characters so that when the user switches
        //        the sliders and such all update.

        [SerializeField] private GameObject[] maleUniqueItems = null;
        [SerializeField] private GameObject[] femaleUniqueItems = null;

        [SerializeField] private GameObject maleRootObject = null;
        [SerializeField] private GameObject femaleRootObject = null;

        [SerializeField] private bool genderMale = true;

        private void OnValidate()
        {
            if (maleRootObject != null && femaleRootObject != null)
            {
                UpdateGenderRoots();
            }
        }

        private void UpdateGenderRoots()
        {
            // Enable the appropriate root object
            maleRootObject.SetActive(genderMale == true);
            femaleRootObject.SetActive(genderMale == false);

            if (maleUniqueItems != null && maleUniqueItems.Length > 0)
                foreach (var item in maleUniqueItems)
                {
                    item.SetActive(genderMale == true);
                }

            if (femaleUniqueItems != null && femaleUniqueItems.Length > 0)
                foreach (var item in femaleUniqueItems)
                {
                    item.SetActive(genderMale == false);
                }
        }

        /// <summary>
        /// Swaps the gender value.
        /// </summary>
        public void SwapGenderRoot()
        {
            genderMale = !genderMale;

            UpdateGenderRoots();
        }

        /// <summary>
        /// Sets the gender to a given value.
        /// </summary>
        /// <param name="value">True if male, False if female.</param>
        public void SetGenderRoot(bool value)
        {
            genderMale = value;

            UpdateGenderRoots();
        }
    }
}