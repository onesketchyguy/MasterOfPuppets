using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PuppetMaster
{
    // FIXME: swap this out with a list of scriptable objects in the future
    public enum CharacterType
    {
        Civilian,
        Priest,
        Police,
        AdvancedPolice,
        Fireman,
        Doctor
    }
}

namespace PuppetMaster.CharacterCreation.SaveAndLoad
{
    public class SaveCharacterMenu : MonoBehaviour
    {
        [SerializeField] private Dropdown tagDropdown = null;

        [SerializeField] private InputField nameText = null;

        [SerializeField] private Button saveButton = null;

        [SerializeField] private UICharacterCustomizerController customCharacter = null;
        [SerializeField] private CharacterGenderSelection genderSelection = null;

        private void OnEnable()
        {
            tagDropdown.ClearOptions();

            var items = System.Enum.GetNames(typeof(CharacterType));
            var options = new List<Dropdown.OptionData>();

            foreach (var item in items)
            {
                var dropdownOption = new Dropdown.OptionData();
                dropdownOption.text = item;

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
            Debug.LogWarning($"WARNING! Tags not available yet! {nameText.text} will not be saved as a {(CharacterType)tagDropdown.value}");
            var data = new CharacterData(customCharacter.characterCustomization.GetSetup());
            data.male = genderSelection.genderMale;
            data.characterTag = $"{(CharacterType)tagDropdown.value}";

            FileSystem.FileSaveManager.Save(data, nameText.text, FileSystem.FileSaveManager.Directories.global, FileSystem.FileSaveManager.Extensions.json);
        }
    }
}