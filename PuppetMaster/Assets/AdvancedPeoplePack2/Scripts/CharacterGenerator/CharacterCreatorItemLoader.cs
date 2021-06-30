using System;
using UnityEngine;

namespace PuppetMaster.CharacterCreation
{
    public class CharacterCreatorItemLoader : MonoBehaviour
    {
        public Transform accessoryParent, beardParent, hairParent, hatParent, pantsParent, shirtParent, shoesParent;

        [Space]
        [SerializeField] private UICharacterCustomizerController characterCustomizer;

        [SerializeField] private CharacterItemList characterItemList;

        public GameObject characterItemPrefab;

        private void DestroyChildren(Transform parent)
        {
            foreach (var child in parent.GetComponentsInChildren<Transform>())
            {
                if (child == parent) continue;
                Destroy(child.gameObject);
            }
        }

        public void ReloadItems(bool maleItems)
        {
            var characterItems = characterItemList.GetGenderItems(maleItems);

            characterCustomizer.characterCustomization.hairPresets.Clear();
            characterCustomizer.characterCustomization.beardPresets.Clear();
            characterCustomizer.characterCustomization.hatsPresets.Clear();
            characterCustomizer.characterCustomization.shirtsPresets.Clear();
            characterCustomizer.characterCustomization.pantsPresets.Clear();
            characterCustomizer.characterCustomization.shoesPresets.Clear();
            characterCustomizer.characterCustomization.accessoryPresets.Clear();

            var hairPreset = new AdvancedCustomizableSystem.HairPreset();
            var beardPreset = new AdvancedCustomizableSystem.BeardPreset();
            var clothPreset = new AdvancedCustomizableSystem.ClothPreset();

            DestroyChildren(accessoryParent);
            DestroyChildren(beardParent);
            DestroyChildren(hairParent);
            DestroyChildren(hatParent);
            DestroyChildren(pantsParent);
            DestroyChildren(shirtParent);
            DestroyChildren(shoesParent);

            foreach (var item in characterItems)
            {
                Transform parent = null;
                Action<int> action = null;
                int index = 0;

                switch (item.itemType)
                {
                    case ItemType.Hair:
                        parent = hairParent;

                        index = characterCustomizer.characterCustomization.hairPresets.Count;
                        action = i => characterCustomizer.HairChange_Event(i);

                        hairPreset.mesh = item.mesh;
                        hairPreset.name = item.name;
                        characterCustomizer.characterCustomization.hairPresets.Add(hairPreset);
                        break;

                    case ItemType.Beard:
                        parent = beardParent;

                        index = characterCustomizer.characterCustomization.beardPresets.Count;
                        action = i => characterCustomizer.BeardChange_Event(i);

                        beardPreset.mesh = item.mesh;
                        beardPreset.name = item.name;
                        characterCustomizer.characterCustomization.beardPresets.Add(beardPreset);
                        break;

                    case ItemType.Hat:
                        parent = hatParent;

                        index = characterCustomizer.characterCustomization.hatsPresets.Count;
                        action = i => characterCustomizer.HatChange_Event(i);

                        clothPreset.mesh = item.mesh;
                        clothPreset.name = item.name;
                        clothPreset.mats = item.mats;
                        clothPreset.hideParts = item.hideParts;
                        clothPreset.yOffset = item.yOffset;
                        characterCustomizer.characterCustomization.hatsPresets.Add(clothPreset);
                        break;

                    case ItemType.Shirt:
                        parent = shirtParent;

                        index = characterCustomizer.characterCustomization.shirtsPresets.Count;
                        action = i => characterCustomizer.ShirtChange_Event(i);

                        clothPreset.mesh = item.mesh;
                        clothPreset.name = item.name;
                        clothPreset.mats = item.mats;
                        clothPreset.hideParts = item.hideParts;
                        clothPreset.yOffset = item.yOffset;
                        characterCustomizer.characterCustomization.shirtsPresets.Add(clothPreset);
                        break;

                    case ItemType.Pants:
                        parent = pantsParent;

                        index = characterCustomizer.characterCustomization.pantsPresets.Count;
                        action = i => characterCustomizer.PantsChange_Event(i);

                        clothPreset.mesh = item.mesh;
                        clothPreset.name = item.name;
                        clothPreset.mats = item.mats;
                        clothPreset.hideParts = item.hideParts;
                        clothPreset.yOffset = item.yOffset;
                        characterCustomizer.characterCustomization.pantsPresets.Add(clothPreset);
                        break;

                    case ItemType.Shoes:
                        parent = shoesParent;

                        index = characterCustomizer.characterCustomization.shoesPresets.Count;
                        action = i => characterCustomizer.ShoesChange_Event(i);

                        clothPreset.mesh = item.mesh;
                        clothPreset.name = item.name;
                        clothPreset.mats = item.mats;
                        clothPreset.hideParts = item.hideParts;
                        clothPreset.yOffset = item.yOffset;
                        characterCustomizer.characterCustomization.shoesPresets.Add(clothPreset);
                        break;

                    case ItemType.Accessory:
                        parent = accessoryParent;

                        index = characterCustomizer.characterCustomization.accessoryPresets.Count;
                        action = i => characterCustomizer.AccessoryChange_Event(i);

                        clothPreset.mesh = item.mesh;
                        clothPreset.name = item.name;
                        clothPreset.mats = item.mats;
                        clothPreset.hideParts = item.hideParts;
                        clothPreset.yOffset = item.yOffset;
                        characterCustomizer.characterCustomization.accessoryPresets.Add(clothPreset);
                        break;

                    default:
                        Debug.LogError($"ERROR! Unable to parse {nameof(ItemType)} more items will not be added.");
                        return;
                }

                if (index == 0)
                {
                    var nullItemObject = Instantiate(characterItemPrefab, parent).GetComponent<UICCItemController>();

                    nullItemObject.SetupItem(action, null, -1);
                }

                var itemGameObject = Instantiate(characterItemPrefab, parent).GetComponent<UICCItemController>();

                itemGameObject.SetupItem(action, item.icon, index);

                hairPreset = new AdvancedCustomizableSystem.HairPreset();
                beardPreset = new AdvancedCustomizableSystem.BeardPreset();
                clothPreset = new AdvancedCustomizableSystem.ClothPreset();
            }
        }
    }
}