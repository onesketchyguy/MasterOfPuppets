using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using AdvancedCustomizableSystem;

namespace PuppetMaster.CharacterCreation
{
    public class AdvancedColorPicker : MonoBehaviour
    {
        private Image mainImage;
        public RectTransform pickerIcon;
        public Image colorPreview;
        private bool _activeCursor;
        public Vector2 offset;
        public UICharacterCustomizerController UIControllerDEMO;
        public Canvas Canvas;

        private Mouse currentMouse;

        private Vector3 GetMousePosition()
        {
            if (currentMouse == null)
            {
                currentMouse = Mouse.current;
            }

            var value = currentMouse.position.ReadValue();

            return value;
        }

        private void Awake()
        {
            mainImage = GetComponent<Image>();
        }

        public void CursorEnter()
        {
            if (!pickerIcon.gameObject.activeSelf)
            {
                pickerIcon.gameObject.SetActive(true);
                _activeCursor = true;
            }
        }

        private void Update()
        {
            if (_activeCursor)
            {
                CursorMove();
            }
        }

        private Color _findColor;
        public Vector2 realSize;

        public void CursorMove()
        {
            pickerIcon.position = GetMousePosition();

            realSize = mainImage.rectTransform.rect.size * Canvas.scaleFactor;

            offset = (mainImage.rectTransform.position - GetMousePosition());

            offset = new Vector2((256 / realSize.x) * offset.x, (256 / realSize.y) * offset.y);

            _findColor = mainImage.sprite.texture.GetPixel((int)-offset.x, 256 - (int)offset.y);

            if (_findColor.a == 1)
                colorPreview.color = _findColor;
        }

        public void CursorPickSkin()
        {
            if (_findColor.a == 1)
                UIControllerDEMO.SetNewSkinColor(_findColor);
        }

        public void CursorPickEye()
        {
            if (_findColor.a == 1)
                UIControllerDEMO.SetNewEyeColor(_findColor);
        }

        public void CursorPickHair()
        {
            if (_findColor.a == 1)
                UIControllerDEMO.SetNewHairColor(_findColor);
        }

        public void CursorPickUnderpants()
        {
            if (_findColor.a == 1)
                UIControllerDEMO.SetNewUnderpantsColor(_findColor);
        }

        public void CursorExit()
        {
            if (pickerIcon.gameObject.activeSelf)
            {
                pickerIcon.gameObject.SetActive(false);
                _activeCursor = false;
            }
        }
    }
}