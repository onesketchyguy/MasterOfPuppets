using UnityEngine;
using UnityEngine.UI;

namespace PuppetMaster.CharacterCreation.SaveAndLoad
{
    public class LoadItemButton : MonoBehaviour
    {
        [SerializeField] private Text nameText;
        private string saveName = "";

        private CharacterData characterData;

        public void Initialize(string name, CharacterData characterData)
        {
            nameText.text = name;
            this.characterData = characterData;
        }

        public void Load()
        {
            // FIXME: load character data
            throw new System.NotImplementedException();
        }

        public void Delete()
        {
            // FIXME: delete this save
            throw new System.NotImplementedException();
        }
    }
}