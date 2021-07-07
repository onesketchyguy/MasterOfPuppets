using UnityEngine;
using UnityEngine.UI;
using FileSystem;

namespace PuppetMaster.CharacterCreation.SaveAndLoad
{
    public class LoadItemButton : MonoBehaviour
    {
        [SerializeField] private Text nameText;

        private LoadCharacterMenu menu;
        private CharacterData characterData;
        private FileSaveManager.Directories dir;
        private FileSaveManager.Extensions ext;

        public void Initialize(string name, CharacterData characterData, LoadCharacterMenu menu, FileSaveManager.Directories dir, FileSaveManager.Extensions ext)
        {
            nameText.text = name;
            this.characterData = characterData;
            this.dir = dir;
            this.ext = ext;
            this.menu = menu;
        }

        public void Load()
        {
            menu.LoadCharacter(characterData);
        }

        public void Delete()
        {
            if (FileSaveManager.DeleteFile(nameText.text, dir, ext))
            {
                Destroy(gameObject);
            }
        }
    }
}