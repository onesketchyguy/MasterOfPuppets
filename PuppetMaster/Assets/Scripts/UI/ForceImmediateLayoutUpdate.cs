using UnityEngine;
using UnityEngine.UI;

namespace CustomUI
{
    public class ForceImmediateLayoutUpdate : MonoBehaviour
    {
        [SerializeField] private LayoutGroup layoutGroup;
        private RectTransform rectTransform;
        [SerializeField] private float waitBeforeUpdate = 0.0f;
        [SerializeField] private bool updatePeriodically = false;

        private void OnValidate()
        {
            if (layoutGroup == null)
            {
                layoutGroup = GetComponent<LayoutGroup>();
            }

            if (layoutGroup != null)
            {
                rectTransform = layoutGroup.GetComponent<RectTransform>();
            }
        }

        private void OnEnable()
        {
            Invoke(nameof(UpdateLayout), waitBeforeUpdate);
        }

        private void Update()
        {
            // In theory we should only update once every second
            if (Time.frameCount % 60 == 0)
            {
                UpdateLayout();
            }
        }

        private void UpdateLayout()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }
    }
}