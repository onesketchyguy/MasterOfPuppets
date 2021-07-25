using FileSystem;
using System.Collections.Generic;
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

        private List<CharacterData> characters = new List<CharacterData>();

        private void OnEnable()
        {
            var fileDir = FileSaveManager.GetFileDirectory(SaveCharacterMenu.SAVE_LOCATION, FileSaveManager.Directories.global, FileSaveManager.Extensions.json);
            if (System.IO.File.Exists(fileDir))
            {
                var fileLines = System.IO.File.ReadAllLines(fileDir);

                for (int i = 0; i < fileLines.Length; i++)
                {
                    if (fileLines[i].Length <= 1) continue;

                    var data = FileSaveManager.DeserializeString<CharacterData>(fileLines[i], extension);
                    if (data == null)
                    {
                        Debug.LogWarning($"Unable to deserialize data: {fileLines[i]}");

                        continue;
                    }

                    var buttonObj = Instantiate(loadButton, saveRegionParent);
                    buttonObj.GetComponent<LoadItemButton>().Initialize(data, this);
                    characters.Add(data);
                }
            }
            else
            {
                Debug.Log("No saves exist!");
                // FIXME: Add a message for the user to let them know no saves exist
            }

            /* OLD METHOD
            var files = FileSaveManager.GetFilesFromDirectory(directory, extension);
            foreach (var item in files)
            {
                var buttonObj = Instantiate(loadButton, saveRegionParent);
                var data = FileSaveManager.Load<CharacterData>(item, directory, extension);
                buttonObj.GetComponent<LoadItemButton>().Initialize(item, data, this, directory, extension);
            }*/
        }

        private void OnDisable()
        {
            foreach (var item in saveRegionParent.GetComponentsInChildren<Transform>())
            {
                if (item.transform == saveRegionParent) continue;
                Destroy(item.gameObject);
            }
        }

        public void LoadCharacter(CharacterData loadData)
        {
            genderSelection.SetGenderRoot(loadData.male);
            customCharacter.characterCustomization.SetCharacterSetup(loadData);
        }

        public void DeleteCharacter(CharacterData data)
        {
            var fileDir = FileSaveManager.GetFileDirectory(SaveCharacterMenu.SAVE_LOCATION, FileSaveManager.Directories.global, FileSaveManager.Extensions.json);
            if (System.IO.File.Exists(fileDir))
            {
                var items = new List<string>();

                var fileLines = System.IO.File.ReadAllLines(fileDir);
                for (int i = 0; i < fileLines.Length; i++)
                {
                    var isMale = fileLines[i].Contains("\"male\":true");

                    if (fileLines[i].Contains(data.characterName) && fileLines[i].Contains(data.characterTag) && isMale == data.male)
                    {
                        continue;
                    }

                    items.Add(fileLines[i]);
                }

                System.IO.File.WriteAllLines(fileDir, items.ToArray());
            }
            else
            {
                Debug.LogError("Dear god something has gone terribly wrong!");
                // FIXME: Add a message for the user to let them know no saves exist
            }

            OnDisable();
            OnEnable();
        }
    }
}