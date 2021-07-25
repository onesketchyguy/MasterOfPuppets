using UnityEngine;
using UnityEngine.UI;

namespace PuppetMaster.CharacterCreation.SaveAndLoad
{
    public class LoadItemButton : MonoBehaviour
    {
        [SerializeField] private Text nameText;

        private LoadCharacterMenu menu;
        private CharacterData characterData;

        public void Initialize(CharacterData characterData, LoadCharacterMenu menu)
        {
            string gender = characterData.male ? "m_" : "f_";
            nameText.text = $"{gender}{characterData.characterName}";
            this.characterData = characterData;
            this.menu = menu;
        }

        public void Load() => menu.LoadCharacter(characterData);

        public void Delete() => menu.DeleteCharacter(characterData);
    }
}