using UnityEngine;
using UnityEngine.UI;

namespace CustomUI
{
#if UNITY_EDITOR

    using UnityEditor;

    [CustomEditor(typeof(CustomToggle))]
    public class CustomToggleEditor : Editor
    {
        private SerializedProperty onToggleField;

        private void OnEnable()
        {
            onToggleField = serializedObject.FindProperty(nameof(CustomToggle.onValueChanged));
        }

        public override void OnInspectorGUI()
        {
            var toggle = (CustomToggle)target;

            toggle.isOn = EditorGUILayout.Toggle("isOn", toggle.isOn);
            EditorGUILayout.Space();

            // Text objects
            toggle.valueText = (Text)EditorGUILayout.ObjectField("Text Object", toggle.valueText, typeof(Text), true);
            toggle.onText = EditorGUILayout.TextField("On text", toggle.onText);
            toggle.offText = EditorGUILayout.TextField("Off text", toggle.offText);
            EditorGUILayout.Space();

            toggle.backImage = (Image)EditorGUILayout.ObjectField("Back image object", toggle.backImage, typeof(Image), true);
            toggle.onColor = EditorGUILayout.ColorField("On Color", toggle.onColor);
            toggle.offColor = EditorGUILayout.ColorField("Off Color", toggle.offColor);
            EditorGUILayout.Space();

            toggle.handle = (RectTransform)EditorGUILayout.ObjectField("Handle", toggle.handle, typeof(RectTransform), true);

            EditorGUILayout.PropertyField(onToggleField);

            // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
            serializedObject.ApplyModifiedProperties();

            // Validates this component
            toggle.OnValueModified();
        }

        [MenuItem("GameObject/UI/Custom Toggle - DEMO")]
        private static void CreateToggleObject()
        {
            var canvas = FindObjectOfType<Canvas>();

            var toggleParent = new GameObject("Toggle");
            toggleParent.transform.SetParent(canvas.transform);
            toggleParent.transform.localScale = Vector3.one;
            var rectTransform = toggleParent.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(120.0f, 72.0f);

            var toggleComponent = toggleParent.gameObject.AddComponent<CustomToggle>();

            var img = toggleParent.gameObject.AddComponent<Image>();
            img.color = new Color(255, 255, 255, 3);
            img.maskable = false;

            var textObject = new GameObject("Toggle Text");
            textObject.transform.SetParent(toggleParent.transform);
            rectTransform = textObject.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(120.0f, 36.0f);
            //rectTransform.anchorMin = new Vector2(0.0f, 0.5f);
            //rectTransform.anchorMax = new Vector2(1.0f, 1.0f);
            toggleComponent.valueText = textObject.gameObject.AddComponent<Text>();
            toggleComponent.valueText.alignment = TextAnchor.MiddleCenter;
            toggleComponent.valueText.fontSize = 30;

            var backGroundObject = new GameObject("Background");
            backGroundObject.transform.SetParent(toggleParent.transform);
            rectTransform = backGroundObject.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(60.0f, 17.0f);
            //rectTransform.anchorMin = new Vector2(0.0f, 0.5f);
            //rectTransform.anchorMax = new Vector2(1.0f, 1.0f);
            toggleComponent.backImage = backGroundObject.gameObject.AddComponent<Image>();

            var handleObject = new GameObject("Handle");
            handleObject.transform.SetParent(backGroundObject.transform);
            rectTransform = handleObject.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(36.0f, 36.0f);
            //rectTransform.anchorMin = new Vector2(0.0f, 0.0f);
            //rectTransform.anchorMax = new Vector2(1.0f, 0.5f);
            toggleComponent.handle = handleObject.gameObject.AddComponent<Image>().rectTransform;

            Selection.activeObject = toggleParent;

            toggleComponent.OnValueModified();
        }
    }

#endif

    public class CustomToggle : Toggle
    {
        [SerializeField] internal Text valueText = null;
        [SerializeField] internal string onText = "On";
        [SerializeField] internal string offText = "Off";

        [SerializeField] internal Image backImage = null;
        [SerializeField] internal Color onColor = Color.white;
        [SerializeField] internal Color offColor = Color.black;

        [SerializeField] internal RectTransform handle = null;

        internal void OnValueModified()
        {
            if (valueText != null)
            {
                valueText.text = isOn ? onText : offText;
            }

            if (backImage != null)
            {
                backImage.color = isOn ? onColor : offColor;

                if (handle != null)
                {
                    var newPos = handle.localPosition;
                    newPos.x = isOn ? backImage.rectTransform.rect.xMax : backImage.rectTransform.rect.xMin;

                    handle.localPosition = newPos;
                }
                else
                {
                    Debug.LogWarning($"No handle object setup on {gameObject.name}");
                }
            }
        }

        protected override void OnValidate()
        {
            // FIXME: Allow the user to use navigation events
            // Remove navigation events
            var nonNav = navigation;
            nonNav.mode = Navigation.Mode.None;
            navigation = nonNav;

            transition = Transition.None;
            toggleTransition = ToggleTransition.None;

            OnValueModified();
        }

        protected override void Start()
        {
            base.Start();
            onValueChanged.AddListener(_ => OnValueModified());
            onValueChanged.Invoke(isOn);
        }
    }
}