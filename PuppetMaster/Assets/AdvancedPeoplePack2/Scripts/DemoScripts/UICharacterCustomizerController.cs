using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using AdvancedCustomizableSystem;
using UnityEngine.EventSystems;

namespace PuppetMaster.CharacterCreation
{
    /// <summary>
    /// Basically this whole script will need to be overhauled, it's only like this because it's easier to copy the
    /// demo scene script than it is to start from scratch.. Which we will need to do.
    /// </summary>

    public class UICharacterCustomizerController : MonoBehaviour
    {
        [Space(5)]
        public CharacterCustomization characterCustomization;

        public GameObject canvasObject;
        private bool canvasVisible = true;

        private float keyDown = 0.0f;

        [Space(15)]
        public Text playbutton_text;

        public Text lod_text;

        public Slider fatSlider;
        public Slider musclesSlider;

        public Slider slimnessSlider;
        public Slider breastSlider;

        public Slider heightSlider;

        public Slider legSlider;

        public Slider headSizeSlider;

        public Slider headOffsetSlider;

        public Slider[] faceShapeSliders;

        public Image SkinColorButtonColor;
        public Image EyeColorButtonColor;
        public Image HairColorButtonColor;
        public Image UnderpantsColorButtonColor;

        public Vector3[] CameraPositionForPanels;
        public Vector3[] CameraEulerForPanels;
        private int currentPanelIndex = 0;

        private Transform mainCam;

        #region ButtonEvents

        public void SetFaceShape(int index)
        {
            characterCustomization.SetFaceShape((FaceShapeType)(index), faceShapeSliders[index].value);
        }

        public void SetHeadOffset(float value)
        {
            characterCustomization.SetHeadOffset(value);
        }

        public void SetBodyFat(float value)
        {
            characterCustomization.SetBodyShape(BodyShapeType.Fat, value);
        }

        public void SetBodyMuscles(float value)
        {
            characterCustomization.SetBodyShape(BodyShapeType.Muscles, value);
        }

        public void SetBodySlimness(float value)
        {
            characterCustomization.SetBodyShape(BodyShapeType.Thin, value);
            characterCustomization.SetBodyShape(BodyShapeType.Slimness, value);
        }

        public void SetBodyBreast(float value)
        {
            characterCustomization.SetBodyShape(BodyShapeType.BreastSize, value,
                new string[] { "Chest", "Stomach", "Head" },
                new ClothesPartType[] { ClothesPartType.Shirt }
                );
        }

        public void SetHeight(float value)
        {
            characterCustomization.SetHeight(value);
        }

        public void SetHeadSize(float value)
        {
            characterCustomization.SetHeadSize(value);
        }

        private int lodIndex;

        public void Lod_Event(int next)
        {
            lodIndex += next;
            if (lodIndex < 0)
                lodIndex = 3;
            if (lodIndex > 3)
                lodIndex = 0;

            lod_text.text = lodIndex.ToString();

            characterCustomization.ForceLOD(lodIndex);
        }

        public void SetNewSkinColor(Color color)
        {
            SkinColorButtonColor.color = color;
            characterCustomization.SetBodyColor(BodyColorPart.Skin, color);
        }

        public void SetNewEyeColor(Color color)
        {
            EyeColorButtonColor.color = color;
            characterCustomization.SetBodyColor(BodyColorPart.Eye, color);
        }

        public void SetNewHairColor(Color color)
        {
            HairColorButtonColor.color = color;
            characterCustomization.SetBodyColor(BodyColorPart.Hair, color);
        }

        public void SetNewUnderpantsColor(Color color)
        {
            UnderpantsColorButtonColor.color = color;
            characterCustomization.SetBodyColor(BodyColorPart.Underpants, color);
        }

        public void EmotionsChange_Event(int index)
        {
            var emotion = characterCustomization.emotionPresets[index];
            if (emotion != null)
                characterCustomization.PlayEmotion(emotion.name, 2f);
        }

        public void HairChange_Event(int index) => characterCustomization.SetHairByIndex(index);

        public void BeardChange_Event(int index) => characterCustomization.SetBeardByIndex(index);

        public void ShirtChange_Event(int index) => characterCustomization.SetElementByIndex(ClothesPartType.Shirt, index);

        public void PantsChange_Event(int index) => characterCustomization.SetElementByIndex(ClothesPartType.Pants, index);

        public void ShoesChange_Event(int index) => characterCustomization.SetElementByIndex(ClothesPartType.Shoes, index);

        public void HatChange_Event(int index) => characterCustomization.SetElementByIndex(ClothesPartType.Hat, index);

        public void AccessoryChange_Event(int index) => characterCustomization.SetElementByIndex(ClothesPartType.Accessory, index);

        public void SaveToFile()
        {
            characterCustomization.SaveToFile();
        }

        public void LoadFromFile()
        {
            characterCustomization.LoadFromFile();
        }

        public void ClearFromFile()
        {
            characterCustomization.ClearFromFile();
            characterCustomization.ResetAll();
        }

        public void Randomize()
        {
            characterCustomization.Randomize();
            UpdateSliders();
        }

        private bool walk_active = false;

        public void PlayAnim()
        {
            walk_active = !walk_active;

            foreach (Animator a in characterCustomization.GetAnimators())
                a.SetBool("walk", walk_active);

            playbutton_text.text = (walk_active) ? "STOP" : "PLAY";
        }

        #endregion ButtonEvents

        private void Update()
        {
            if (mainCam == null)
            {
                mainCam = Camera.main.transform;
            }
            else
            {
                mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, CameraPositionForPanels[currentPanelIndex], Time.deltaTime * 5);
                mainCam.transform.eulerAngles = Vector3.Lerp(mainCam.transform.eulerAngles, CameraEulerForPanels[currentPanelIndex], Time.deltaTime * 5);
            }

            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject() == true) return;

            if (Mouse.current.scroll.ReadValue().y > 0)
            {
                UpdateCameraPosition(1);
            }
            else if (Mouse.current.scroll.ReadValue().y < 0)
            {
                UpdateCameraPosition(-1);
            }

            if (Keyboard.current.hKey.isPressed)
            {
                if (keyDown <= 0.0f)
                {
                    canvasVisible = !canvasVisible;

                    canvasObject.SetActive(canvasVisible);
                }

                keyDown = 0.1f;
            }

            if (Keyboard.current.spaceKey.isPressed)
            {
                if (keyDown <= 0.0f)
                {
                    // Shift + space to zoom out
                    if (Keyboard.current.leftShiftKey.isPressed)
                    {
                        UpdateCameraPosition(-1);
                    }
                    // Space to zoom in
                    else
                    {
                        UpdateCameraPosition(1);
                    }
                }

                keyDown = 0.1f;
            }

            keyDown -= Time.deltaTime;
        }

        private void UpdateCameraPosition(int dir)
        {
            currentPanelIndex = Mathf.Clamp(currentPanelIndex + dir, 0, CameraPositionForPanels.Length - 1);
        }

        public void UpdateSliders()
        {
            // FIXME: Update all the sliders and such

            slimnessSlider.SetValueWithoutNotify(characterCustomization.GetBodyShapeWeight(BodyShapeType.Slimness.ToString()));
            fatSlider.SetValueWithoutNotify(characterCustomization.GetBodyShapeWeight(BodyShapeType.Slimness.ToString()));
            musclesSlider.SetValueWithoutNotify(characterCustomization.GetBodyShapeWeight(BodyShapeType.Muscles.ToString()));
            breastSlider.SetValueWithoutNotify(characterCustomization.GetBodyShapeWeight(BodyShapeType.BreastSize.ToString()));

            //heightSlider.SetValueWithoutNotify(characterCustomization.GetCharacterPart("Height");
            //headSizeSlider.SetValueWithoutNotify(characterCustomization.GetBodyShapeWeight(BodyShapeType.Slimness.ToString()));
            //headOffsetSlider.SetValueWithoutNotify(characterCustomization.GetBodyShapeWeight(BodyShapeType.Slimness.ToString()));
            //legSlider;
        }

        public void ApplySettings(CharacterCustomizationSetup characterSettings)
        {
            characterCustomization.SetCharacterSetup(characterSettings);
        }

        public CharacterCustomizationSetup GetSettings()
        {
            return characterCustomization.GetSetup();
        }
    }
}