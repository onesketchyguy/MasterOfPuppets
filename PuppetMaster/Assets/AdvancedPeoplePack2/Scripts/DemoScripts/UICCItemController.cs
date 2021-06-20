namespace PuppetMaster.CharacterCreation
{
    public class UICCItemController : UnityEngine.MonoBehaviour
    {
        public System.Action<int> onUseEvent = null;

        public UnityEngine.UI.Image iconImage;

        public int itemIndex;

        public void SetupItem(System.Action<int> useEvent, UnityEngine.Sprite icon, int index)
        {
            itemIndex = index;

            if (icon != null) iconImage.sprite = icon;
            onUseEvent = useEvent;
        }

        public void OnUseEvent()
        {
            // Do the thing with the things
            onUseEvent?.Invoke(itemIndex);
        }
    }
}