using UnityEngine;

namespace PuppetMaster.CharacterCreation
{
    [CreateAssetMenu(menuName = "APPack/Character item list")]
    public class CharacterItemList : ScriptableObject
    {
        public CharacterItem[] maleCharacterItems;
        public CharacterItem[] femaleCharacterItems;

        public CharacterItem[] GetGenderItems(bool maleItems)
        {
            return maleItems ? maleCharacterItems : femaleCharacterItems;
        }
    }
}