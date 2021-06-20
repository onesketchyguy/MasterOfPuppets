using UnityEngine;
using AdvancedCustomizableSystem;

namespace PuppetMaster.CharacterCreation
{
#if UNITY_EDITOR

    using UnityEditor;
    using UnityEditorInternal;

    [CustomEditor(typeof(CharacterItem))]
    public class CharacterItemEditor : Editor
    {
        private SerializedProperty meshField;
        private SerializedProperty materialField;
        private SerializedProperty yOffsetField;
        private SerializedProperty hidePartsField;

        private void OnEnable()
        {
            meshField = serializedObject.FindProperty(nameof(CharacterItem.mesh));
            materialField = serializedObject.FindProperty(nameof(CharacterItem.mats));
            yOffsetField = serializedObject.FindProperty(nameof(CharacterItem.yOffset));
            hidePartsField = serializedObject.FindProperty(nameof(CharacterItem.hideParts));
        }

        public override void OnInspectorGUI()
        {
            var myTarget = (CharacterItem)target;

            myTarget.itemType = (ItemType)EditorGUILayout.EnumPopup("Type", myTarget.itemType);
            myTarget.icon = (Sprite)EditorGUILayout.ObjectField("Icon", myTarget.icon, typeof(Sprite), false);
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(meshField);

            if (myTarget.itemType != ItemType.Beard && myTarget.itemType != ItemType.Hair)
            {
                EditorGUILayout.PropertyField(materialField);
                EditorGUILayout.PropertyField(yOffsetField);
                EditorGUILayout.PropertyField(hidePartsField);
            }

            // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
            serializedObject.ApplyModifiedProperties();
        }
    }

#endif

    public enum ItemType
    {
        Hair,
        Beard,
        Hat,
        Shirt,
        Pants,
        Shoes,
        Accessory,
    }

    [CreateAssetMenu(menuName = "APPack/Character item")]
    public class CharacterItem : ScriptableObject
    {
        public ItemType itemType;

        public Sprite icon;

        public Mesh[] mesh;
        public string[] hideParts;
        public float yOffset = 0;
        public Material[] mats;

        public HairPreset GetHairPreset()
        {
            var preset = new HairPreset();

            preset.mesh = mesh;

            return preset;
        }

        public BeardPreset GetBeardPreset()
        {
            var preset = new BeardPreset();

            preset.mesh = mesh;

            return preset;
        }

        public ClothPreset GetClothPreset()
        {
            var preset = new ClothPreset();

            preset.mesh = mesh;
            preset.hideParts = hideParts;
            preset.yOffset = yOffset;
            preset.mats = mats;

            return preset;
        }
    }
}