using UnityEngine;

namespace PuppetMaster.CharacterCreation
{
#if UNITY_EDITOR

    using System.IO;
    using UnityEditor;
    using System.Collections.Generic;

    [CustomEditor(typeof(CharacterItemList))]
    public class CharacterItemListEditor : Editor
    {
        private string GetAssetDirectory(string fromDirectory)
        {
            string r = "";

            foreach (var item in fromDirectory.Split('/'))
            {
                if (r.Length > 0)
                {
                    r += $"/{item}";
                }
                else
                {
                    if (item == "Assets")
                        r += $"{item}";
                }
            }

            return r;
        }

        private CharacterItem[] GetItemsFromDirectory(string folder, bool male)
        {
            string dir = GetAssetDirectory(folder);

            string[] guids = AssetDatabase.FindAssets($"t:{nameof(CharacterItem)}", new[] { dir });

            int count = guids.Length;
            var items = new List<CharacterItem>();
            for (int n = 0; n < count; n++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[n]);

                if (dir.Contains(male ? "Male" : "Female") == true)
                {
                    items.Add(AssetDatabase.LoadAssetAtPath<CharacterItem>(path));
                }
            }

            return items.ToArray();
        }

        private CharacterItem[] GetItems(string folder, bool male)
        {
            var items = new List<CharacterItem>();
            var directories = Directory.EnumerateDirectories(folder);

            foreach (var dir in directories)
            {
                int startCount = items.Count;
                foreach (var item in GetItemsFromDirectory(dir, male)) items.Add(item);

                var additionalItems = GetItems(dir, male);
                foreach (var addItem in additionalItems)
                {
                    if (items.Contains(addItem) == false) items.Add(addItem);
                }
                Debug.Log($"Found {items.Count - startCount} items in {dir}");
            }

            return items.ToArray();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var myTarget = (CharacterItemList)target;

            if (GUILayout.Button("Grab items from directory"))
            {
                string directory = EditorUtility.OpenFolderPanel("Load items", "", "");
                var maleItems = GetItems(directory, true);
                var femaleItems = GetItems(directory, false);

                myTarget.maleCharacterItems = maleItems;
                myTarget.femaleCharacterItems = femaleItems;
            }

            if (myTarget.maleCharacterItems != null && myTarget.femaleCharacterItems != null)
            {
                if (GUILayout.Button("Remove duplicates"))
                {
                    var maleItems = new List<CharacterItem>();
                    var femaleItems = new List<CharacterItem>();

                    foreach (var item in myTarget.maleCharacterItems)
                        if (maleItems.Contains(item) == false)
                            maleItems.Add(item);

                    foreach (var item in myTarget.femaleCharacterItems)
                        if (femaleItems.Contains(item) == false)
                            femaleItems.Add(item);

                    myTarget.maleCharacterItems = maleItems.ToArray();
                    myTarget.femaleCharacterItems = femaleItems.ToArray();
                }

                GUILayout.Space(20);

                if (GUILayout.Button("Set sprites for items - DEMO"))
                {
                    string path = EditorUtility.OpenFolderPanel("Load png Textures", "", "");
                    string[] files = Directory.GetFiles(path);

                    var sprites = new HashSet<Sprite>();

                    foreach (string file in files)
                        if (file.EndsWith(".png"))
                        {
                            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(GetAssetDirectory(file));
                            sprites.Add(sprite);
                        }

                    foreach (var item in myTarget.maleCharacterItems)
                    {
                        foreach (var sprite in sprites)
                        {
                            if (sprite.name.ToUpper().Contains("_M"))
                            {
                                int similarities = 0;
                                var splits = sprite.name.Split('_');

                                foreach (var split in splits)
                                {
                                    if (item.name.Contains(split))
                                    {
                                        similarities++;
                                    }
                                }

                                if (similarities > 1 && splits.Length > 2 || similarities == 1 && splits.Length <= 2)
                                {
                                    item.icon = sprite;
                                    break;
                                }
                            }
                        }
                    }

                    foreach (var item in myTarget.femaleCharacterItems)
                    {
                        foreach (var sprite in sprites)
                        {
                            if (sprite.name.ToUpper().Contains("_F"))
                            {
                                int similarities = 0;
                                var splits = sprite.name.Split('_');

                                foreach (var split in splits)
                                {
                                    if (item.name.Contains(split))
                                    {
                                        similarities++;
                                    }
                                }

                                if (similarities > 1 && splits.Length > 2 || similarities == 1 && splits.Length <= 2)
                                {
                                    item.icon = sprite;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

#endif

    [CreateAssetMenu(menuName = "APPack/Character item list")]
    public class CharacterItemList : ScriptableObject
    {
        public CharacterItem[] maleCharacterItems;
        public CharacterItem[] femaleCharacterItems;

        public CharacterItem[] GetGenderItems(bool maleItems)
        {
            return maleItems ? maleCharacterItems : femaleCharacterItems;
        }
    }
}