using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PuppetMaster.CharacterCreation.SaveAndLoad
{
    public class SaveCharacterMenu : MonoBehaviour
    {
        [SerializeField] private Dropdown tagDropdown = null;

        [SerializeField] private InputField nameText = null;

        [SerializeField] private Button saveButton = null;

        [SerializeField] private UICharacterCustomizerController customCharacter = null;
        [SerializeField] private CharacterGenderSelection genderSelection = null;

        [SerializeField] private CharacterTypeList characterList;

        [SerializeField] private FileSystem.FileSaveManager.Directories fileLocation;
        [SerializeField] private FileSystem.FileSaveManager.Extensions fileExtension;

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
            Debug.LogWarning($"WARNING! Tags not available yet! {nameText.text} will not be saved as a {characterList.types[tagDropdown.value].name}");
            var data = new CharacterData(customCharacter.characterCustomization.GetSetup());
            data.male = genderSelection.genderMale;
            data.characterTag = $"{characterList.types[tagDropdown.value].name}";

            FileSystem.FileSaveManager.Save(data, nameText.text, fileLocation, fileExtension);
        }
    }
}