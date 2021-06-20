#if UNITY_EDITOR

using AdvancedCustomizableSystem;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace PuppetMaster.CharacterCreation
{
    [CustomEditor(typeof(CharacterCustomization))]
    public class CreateItemAssetsEditor : Editor
    {
        private const string PARENT_FOLDER = "_TempFolder";

        public static List<HairPreset> hairPresets = new List<HairPreset>();
        public static List<BeardPreset> beardPresets = new List<BeardPreset>();
        public static List<ClothPreset> hatsPresets = new List<ClothPreset>();
        public static List<ClothPreset> accessoryPresets = new List<ClothPreset>();
        public static List<ClothPreset> shirtsPresets = new List<ClothPreset>();
        public static List<ClothPreset> pantsPresets = new List<ClothPreset>();
        public static List<ClothPreset> shoesPresets = new List<ClothPreset>();

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var myTarget = (CharacterCustomization)target;

            hairPresets = myTarget.hairPresets;
            beardPresets = myTarget.beardPresets;
            hatsPresets = myTarget.hatsPresets;
            accessoryPresets = myTarget.accessoryPresets;
            shirtsPresets = myTarget.shirtsPresets;
            pantsPresets = myTarget.pantsPresets;
            shoesPresets = myTarget.shoesPresets;
        }

        private static void TryCreateHairAndBeardAssets()
        {
            var folder = "Hair";
            string directory = $"/{PARENT_FOLDER}/{folder}/";

            if (hairPresets != null && hairPresets.Count > 0)
            {
                if (AssetDatabase.IsValidFolder($"Assets{directory}") == false)
                    AssetDatabase.CreateFolder($"Assets/{PARENT_FOLDER}", folder);

                foreach (var item in hairPresets)
                {
                    var characterItem = new CharacterItem();
                    characterItem.mesh = item.mesh;
                    characterItem.itemType = ItemType.Hair;

                    AssetDatabase.CreateAsset(characterItem, $"Assets{directory}{item.name}.asset");
                }

                Debug.Log("Created hair assets.");
            }

            if (beardPresets != null && beardPresets.Count > 0)
            {
                folder = "Beards";
                directory = $"/{PARENT_FOLDER}/{folder}/";

                if (AssetDatabase.IsValidFolder($"Assets{directory}") == false)
                    AssetDatabase.CreateFolder($"Assets/{PARENT_FOLDER}", folder);

                foreach (var item in beardPresets)
                {
                    var characterItem = new CharacterItem();
                    characterItem.mesh = item.mesh;
                    characterItem.itemType = ItemType.Beard;

                    AssetDatabase.CreateAsset(characterItem, $"Assets{directory}{item.name}.asset");
                }

                Debug.Log("Created beard assets.");
            }
        }

        private static void CreateClothingItems(ClothPreset[] clothPreset, ItemType itemType, string folder)
        {
            string directory = $"/{PARENT_FOLDER}/{folder}/";

            if (AssetDatabase.IsValidFolder($"Assets{directory}") == false)
                AssetDatabase.CreateFolder($"Assets/{PARENT_FOLDER}", folder);

            foreach (var item in clothPreset)
            {
                var characterItem = new CharacterItem();
                characterItem.itemType = itemType;
                characterItem.mesh = item.mesh;
                characterItem.hideParts = item.hideParts;
                characterItem.mats = item.mats;
                characterItem.yOffset = item.yOffset;

                AssetDatabase.CreateAsset(characterItem, $"Assets{directory}{item.name}.asset");
            }

            Debug.Log($"Created {itemType.ToString().ToLower()} assets.");
        }

        // Creates a content menu for the CharacterCustomization item
        [MenuItem("CONTEXT/CharacterCustomization/Create items as assets", false, 0)]
        public static void CreateAssets()
        {
            Debug.Log("Creating assets. This may take some time...");

            if (AssetDatabase.IsValidFolder($"Assets/{PARENT_FOLDER}") == false)
                AssetDatabase.CreateFolder("Assets", PARENT_FOLDER);

            TryCreateHairAndBeardAssets();

            if (hatsPresets != null && hatsPresets.Count > 0)
                CreateClothingItems(hatsPresets.ToArray(), ItemType.Hat, "Hats");

            if (accessoryPresets != null && accessoryPresets.Count > 0)
                CreateClothingItems(accessoryPresets.ToArray(), ItemType.Accessory, "Accessories");

            if (shirtsPresets != null && shirtsPresets.Count > 0)
                CreateClothingItems(shirtsPresets.ToArray(), ItemType.Shirt, "Shirts");

            if (pantsPresets != null && pantsPresets.Count > 0)
                CreateClothingItems(pantsPresets.ToArray(), ItemType.Pants, "Pants");

            if (shoesPresets != null && shoesPresets.Count > 0)
                CreateClothingItems(shoesPresets.ToArray(), ItemType.Shoes, "Shoes");

            Debug.Log($"Done! You can find your assets at Assets/{PARENT_FOLDER}");
        }
    }
}

#endif