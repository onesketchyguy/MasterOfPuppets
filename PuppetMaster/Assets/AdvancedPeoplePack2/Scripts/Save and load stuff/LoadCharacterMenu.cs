using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuppetMaster.CharacterCreation.SaveAndLoad
{
    public class LoadCharacterMenu : MonoBehaviour
    {
        [SerializeField] private GameObject loadButton = null;

        [SerializeField] private Transform saveRegionParent = null;

        private void OnEnable()
        {
            // FIXME: Load all characters items and into the saves region
        }
    }
}