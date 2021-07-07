using System.Collections.Generic;
using UnityEngine;

namespace PuppetMaster.CharacterCreation
{
#if UNITY_EDITOR

    using UnityEditor;

    [CustomEditor(typeof(CharacterTypeList))]
    public class CharacterTypeListEditor : Editor
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

        private string GetAssetDirectoryFromItem()
        {
            var path = "";
            var obj = Selection.activeObject;
            if (obj == null) path = "Assets";
            else path = AssetDatabase.GetAssetPath(obj.GetInstanceID());

            var array = path.Split('/');

            string finalPath = "";
            for (int i = 0; i < array.Length - 1; i++)
            {
                finalPath += array[i] + '/';
            }

            return finalPath;
        }

        private CharacterType[] GetItemsFromDirectory(string folder)
        {
            string dir = GetAssetDirectory(folder);

            string[] guids = AssetDatabase.FindAssets($"t:{nameof(CharacterType)}", new[] { dir });

            int count = guids.Length;
            var items = new List<CharacterType>();
            for (int n = 0; n < count; n++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[n]);

                items.Add(AssetDatabase.LoadAssetAtPath<CharacterType>(path));
            }

            return items.ToArray();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Get files from child directory"))
            {
                var mTarget = ((CharacterTypeList)target);

                var path = GetAssetDirectoryFromItem();

                mTarget.types = GetItemsFromDirectory(path);
                Debug.Log($"Found {mTarget.types.Length} items in {path}");
            }
        }
    }

#endif

    [CreateAssetMenu(menuName = "APPack/Character type list")]
    public class CharacterTypeList : ScriptableObject
    {
        public CharacterType[] types;
    }
}