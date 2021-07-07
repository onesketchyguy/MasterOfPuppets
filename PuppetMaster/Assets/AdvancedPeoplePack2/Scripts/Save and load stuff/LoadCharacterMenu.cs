using FileSystem;
using UnityEngine;

namespace PuppetMaster.CharacterCreation.SaveAndLoad
{
    public class LoadCharacterMenu : MonoBehaviour
    {
        [SerializeField] private GameObject loadButton = null;

        [SerializeField] private Transform saveRegionParent = null;

        [SerializeField] private FileSaveManager.Directories directory;
        [SerializeField] private FileSaveManager.Extensions extension;

        [SerializeField] private CharacterGenderSelection genderSelection = null;
        [SerializeField] private UICharacterCustomizerController customCharacter = null;

        private void OnEnable()
        {
            var files = FileSaveManager.GetFilesFromDirectory(directory, extension);
            foreach (var item in files)
            {
                var buttonObj = Instantiate(loadButton, saveRegionParent);
                var data = FileSaveManager.Load<CharacterData>(item, directory, extension);
                buttonObj.GetComponent<LoadItemButton>().Initialize(item, data, this, directory, extension);
            }
        }

        private void OnDisable()
        {
            foreach (var item in saveRegionParent.GetComponentsInChildren<Transform>())
            {
                if (item.transform == saveRegionParent) continue;
                Destroy(item.gameObject);
            }
        }

        public void LoadCharacter(CharacterData data)
        {
            genderSelection.SetGenderRoot(data.male);
            customCharacter.characterCustomization.SetCharacterSetup(data);
        }
    }
}