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

        public void ReloadItems(bool maleItems)
        {
            var characterItems = characterItemList.GetGenderItems(maleItems);

            foreach (var item in characterItems)
            {
                Transform parent = null;
                Action<int> action = null;

                switch (item.itemType)
                {
                    case ItemType.Hair:
                        parent = hairParent;

                        action = i => characterCustomizer.HairChange_Event(i);
                        break;

                    case ItemType.Beard:
                        parent = beardParent;

                        action = i => characterCustomizer.BeardChange_Event(i);
                        break;

                    case ItemType.Hat:
                        parent = hatParent;

                        action = i => characterCustomizer.HatChange_Event(i);
                        break;

                    case ItemType.Shirt:
                        parent = shirtParent;

                        action = i => characterCustomizer.ShirtChange_Event(i);
                        break;

                    case ItemType.Pants:
                        parent = pantsParent;

                        action = i => characterCustomizer.PantsChange_Event(i);
                        break;

                    case ItemType.Shoes:
                        parent = shoesParent;

                        action = i => characterCustomizer.ShoesChange_Event(i);
                        break;

                    case ItemType.Accessory:
                        parent = accessoryParent;

                        action = i => characterCustomizer.AccessoryChange_Event(i);
                        break;

                    default:
                        Debug.LogError($"ERROR! Unable to parse {nameof(ItemType)} more items will not be added.");
                        return;
                }

                var itemGameObject = Instantiate(characterItemPrefab, parent).GetComponent<UICCItemController>();
                int index = parent.childCount == 0 ? -1 : parent.childCount - 1;

                itemGameObject.SetupItem(action, item.icon, index);
            }
        }
    }
}