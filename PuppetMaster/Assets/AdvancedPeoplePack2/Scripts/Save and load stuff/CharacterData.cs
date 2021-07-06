using AdvancedCustomizableSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuppetMaster.CharacterCreation
{
    public class CharacterData : CharacterCustomizationSetup
    {
        public bool male = false;
        public string characterTag = "";

        // FIXME: There has to be a better wat
        public CharacterData(CharacterCustomizationSetup characterData)
        {
            Fat = characterData.Fat;
            Muscles = characterData.Muscles;
            Slimness = characterData.Slimness;
            Thin = characterData.Thin;
            BreastSize = characterData.BreastSize;
            Neck_Width = characterData.Neck_Width;
            Ear_Size = characterData.Ear_Size;
            Ear_Angle = characterData.Ear_Angle;
            Jaw_Width = characterData.Jaw_Width;
            Jaw_Offset = characterData.Jaw_Offset;
            Jaw_Shift = characterData.Jaw_Shift;
            Cheek_Size = characterData.Cheek_Size;
            Chin_Offset = characterData.Chin_Offset;
            Eye_Width = characterData.Eye_Width;
            Eye_Form = characterData.Eye_Form;
            Eye_InnerCorner = characterData.Eye_InnerCorner;
            Eye_Corner = characterData.Eye_Corner;
            Eye_Rotation = characterData.Eye_Rotation;
            Eye_Offset = characterData.Eye_Offset;
            Eye_ScaleX = characterData.Eye_ScaleX;
            Eye_ScaleY = characterData.Eye_ScaleY;
            Eye_Size = characterData.Eye_Size;
            Eye_Close = characterData.Eye_Close;
            Eye_Height = characterData.Eye_Height;
            Brow_Height = characterData.Brow_Height;
            Brow_Shape = characterData.Brow_Shape;
            Brow_Thickness = characterData.Brow_Thickness;
            Brow_Length = characterData.Brow_Length;
            Nose_Length = characterData.Nose_Length;
            Nose_Size = characterData.Nose_Size;
            Nose_Angle = characterData.Nose_Angle;
            Nose_Offset = characterData.Nose_Offset;
            Nose_Bridge = characterData.Nose_Bridge;
            Nose_Hump = characterData.Nose_Hump;
            Mouth_Offset = characterData.Mouth_Offset;
            Mouth_Width = characterData.Mouth_Width;
            Mouth_Size = characterData.Mouth_Size;
            Mouth_Open = characterData.Mouth_Open;
            Mouth_Bulging = characterData.Mouth_Bulging;
            LipsCorners_Offset = characterData.LipsCorners_Offset;
            Face_Form = characterData.Face_Form;
            Chin_Width = characterData.Chin_Width;
            Chin_Form = characterData.Chin_Form;
            Head_Offset = characterData.Head_Offset;

            Smile = characterData.Smile;
            Sadness = characterData.Sadness;
            Surprise = characterData.Surprise;
            Thoughtful = characterData.Thoughtful;
            Angry = characterData.Angry;

            Hair = characterData.Hair;
            Beard = characterData.Beard;
            Height = characterData.Height;
            HeadSize = characterData.HeadSize;

            Hat = characterData.Hat;
            TShirt = characterData.TShirt;
            Pants = characterData.Pants;
            Shoes = characterData.Shoes;
            Accessory = characterData.Accessory;

            SkinColor = characterData.SkinColor;
            EyeColor = characterData.EyeColor;
            HairColor = characterData.HairColor;
            UnderpantsColor = characterData.UnderpantsColor;
        }
    }
}