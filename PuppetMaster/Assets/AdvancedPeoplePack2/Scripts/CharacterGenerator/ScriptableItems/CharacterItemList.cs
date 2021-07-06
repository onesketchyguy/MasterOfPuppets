using UnityEngine;

namespace PuppetMaster.CharacterCreation
{
#if UNITY_EDITOR

    using System.IO;
    using UnityEditor;
    using System.Collections.Generic;
    using System.Linq;

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

        /// <summary>
        /// Returns the number of steps required to transform the source string
        /// into the target string.
        /// </summary>
        private int ComputeLevenshteinDistance(string source, string target)
        {
            if ((source == null) || (target == null)) return 0;
            if ((source.Length == 0) || (target.Length == 0)) return 0;
            if (source == target) return source.Length;

            int sourceWordCount = source.Length;
            int targetWordCount = target.Length;

            // Step 1
            if (sourceWordCount == 0)
                return targetWordCount;

            if (targetWordCount == 0)
                return sourceWordCount;

            int[,] distance = new int[sourceWordCount + 1, targetWordCount + 1];

            // Step 2
            for (int i = 0; i <= sourceWordCount; distance[i, 0] = i++) ;
            for (int j = 0; j <= targetWordCount; distance[0, j] = j++) ;

            for (int i = 1; i <= sourceWordCount; i++)
            {
                for (int j = 1; j <= targetWordCount; j++)
                {
                    // Step 3
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;

                    // Step 4
                    distance[i, j] = System.Math.Min(System.Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }

            return distance[sourceWordCount, targetWordCount];
        }

        /// <summary>
        /// Calculate percentage similarity of two strings
        /// <param name="source">Source String to Compare with</param>
        /// <param name="target">Targeted String to Compare</param>
        /// <returns>Return Similarity between two strings from 0 to 1.0</returns>
        /// </summary>
        private double CalculateSimilarity(string source, string target)
        {
            if ((source == null) || (target == null)) return 0.0;
            if ((source.Length == 0) || (target.Length == 0)) return 0.0;
            if (source == target) return 1.0;

            int stepsToSame = ComputeLevenshteinDistance(source, target);
            return (1.0 - ((double)stepsToSame / (double)System.Math.Max(source.Length, target.Length)));
        }

        private void SetItemIcons(ref CharacterItemList characterItemList)
        {
            string path = EditorUtility.OpenFolderPanel("Load png Textures", "", "");
            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);

            var sprites = new HashSet<Sprite>();

            Sprite GetItemSprite(string name, bool isFemale)
            {
                double itemSimilarity = 0.0;
                Sprite targetSprite = null;

                foreach (var sprite in sprites)
                {
                    double similarity = CalculateSimilarity(name, sprite.name);

                    if (similarity > itemSimilarity)
                    {
                        if (isFemale == false && sprite.name.ToLower().ToCharArray().LastOrDefault() == 'f') continue;
                        if (isFemale == true && sprite.name.ToLower().ToCharArray().LastOrDefault() == 'm') continue;

                        targetSprite = sprite;
                        itemSimilarity = similarity;
                    }
                }

                if (itemSimilarity < 0.5f)
                {
                    Debug.Log($"item:\"{name}\" set sprite to: null");
                    return null;
                }

                Debug.Log($"item:\"{name}\" set sprite to: \"{targetSprite.name}\" with a confidence of: {itemSimilarity * 100.0}%");

                return targetSprite;
            }

            foreach (string file in files)
                if (file.EndsWith(".png"))
                {
                    var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(GetAssetDirectory(file));
                    sprites.Add(sprite);
                }

            foreach (var item in characterItemList.maleCharacterItems) item.icon = GetItemSprite(item.name, false);
            foreach (var item in characterItemList.femaleCharacterItems) item.icon = GetItemSprite(item.name, true);
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

                if (GUILayout.Button("Set item sprites"))
                {
                    SetItemIcons(ref myTarget);
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