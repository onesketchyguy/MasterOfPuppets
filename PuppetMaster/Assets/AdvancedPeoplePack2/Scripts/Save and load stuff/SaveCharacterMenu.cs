using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FileSystem;

namespace PuppetMaster.CharacterCreation.SaveAndLoad
{
    public class SaveCharacterMenu : MonoBehaviour
    {
        public const string SAVE_LOCATION = "CharacterData";

        [SerializeField] private Dropdown tagDropdown = null;

        [SerializeField] private InputField nameText = null;

        [SerializeField] private Button saveButton = null;

        [SerializeField] private UICharacterCustomizerController customCharacter = null;
        [SerializeField] private CharacterGenderSelection genderSelection = null;

        [SerializeField] private CharacterTypeList characterList;

        [SerializeField] private FileSaveManager.Directories fileLocation;
        [SerializeField] private FileSaveManager.Extensions fileExtension;

        private void OnEnable()
        {
            tagDropdown.ClearOptions();

            var options = new List<Dropdown.OptionData>();

            foreach (var item in characterList.types)
            {
                var dropdownOption = new Dropdown.OptionData();
                dropdownOption.text = item.name;

                options.Add(dropdownOption);
            }

            tagDropdown.options = options;

            nameText.onValueChanged.AddListener(_ => OnNameTextChanged());

            saveButton.onClick.AddListener(() => SaveItem());
            saveButton.interactable = false;
        }

        private void OnNameTextChanged()
        {
            saveButton.interactable = (nameText.text.Length > 1);
        }

        public void SaveItem()
        {
            Debug.Log($"Saving: {nameText.text}");
            var data = new CharacterData(customCharacter.characterCustomization.GetSetup());
            data.male = genderSelection.genderMale;
            data.characterTag = $"{characterList.types[tagDropdown.value].name}";
            data.characterName = nameText.text;

            // ALT
            //FileSystem.FileSaveManager.Save(data, nameText.text, fileLocation, fileExtension);
            var serializedData = FileSaveManager.SerializeToString(data, FileSaveManager.Extensions.json) + "\n";

            var fileDir = FileSaveManager.GetFileDirectory(SAVE_LOCATION, FileSaveManager.Directories.global, FileSaveManager.Extensions.json);
            if (System.IO.File.Exists(fileDir))
            {
                var fileLines = System.IO.File.ReadAllLines(fileDir);
                var totalLines = new List<string>();

                bool createdSave = false;

                for (int i = 0; i < fileLines.Length; i++)
                {
                    if (fileLines[i].Length <= 1) continue;

                    // FIXME: This will only support JSON!
                    var isMale = fileLines[i].Contains("\"male\":true");

                    if (fileLines[i].Contains(data.characterName) && fileLines[i].Contains(data.characterTag) && isMale == data.male)
                    {
                        // FIXME: We should make sure that the user wants to overrite this file

                        fileLines[i] = "";
                        fileLines[i] = serializedData;

                        createdSave = true;
                    }

                    totalLines.Add(fileLines[i]);
                }

                if (createdSave == false)
                {
                    totalLines.Add(serializedData);
                }

                System.IO.File.WriteAllLines(fileDir, totalLines.ToArray());
            }
            else
            {
                System.IO.File.WriteAllText(fileDir, serializedData);
            }
        }
    }
}