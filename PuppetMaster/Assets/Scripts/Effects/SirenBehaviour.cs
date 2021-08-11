using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuppetMaster.Effects
{
    public class SirenBehaviour : MonoBehaviour
    {
        [SerializeField] private float alternateSpeed = 0.5f;

        [Header("Name of the objects to look for in children")]
        [SerializeField] private string nameRight = "SL_R";

        [SerializeField] private string nameLeft = "SL_L";

        [SerializeField] public bool lightsOn = true;
        [SerializeField] public bool soundOn = true;

        private bool onLeft = false;

        private MeshRenderer[] renderersLeft = null, renderersRight = null;

        [SerializeField] private AudioSource sirenSound = null;

        private void Start()
        {
            var leftRens = new List<MeshRenderer>();
            var rightRens = new List<MeshRenderer>();

            foreach (var child in transform.GetComponentsInChildren<MeshRenderer>())
            {
                if (child.name.Contains(nameRight)) rightRens.Add(child);

                if (child.name.Contains(nameLeft)) leftRens.Add(child);
            }

            renderersLeft = leftRens.ToArray();
            renderersRight = rightRens.ToArray();

            SetLightsEnabled(lightsOn);
            SetSoundEnabled(soundOn);
        }

        public void SetLightsEnabled(bool enabled)
        {
            lightsOn = enabled;

            if (lightsOn == true)
            {
                InvokeRepeating(nameof(AlternateLights), alternateSpeed, alternateSpeed);
            }
            else
            {
                CancelInvoke(nameof(AlternateLights));
                SetLights();
            }
        }

        public void SetSoundEnabled(bool enabled)
        {
            soundOn = enabled;

            // FIXME: Fade out the sound here
            sirenSound.enabled = enabled;
        }

        private void AlternateLights()
        {
            SetLights();
            onLeft = !onLeft;
        }

        private void SetLights()
        {
            foreach (var ren in renderersLeft)
            {
                if (onLeft == true && lightsOn == true)
                {
                    ren.material.EnableKeyword("_EMISSION");
                }
                else
                {
                    ren.material.DisableKeyword("_EMISSION");
                }
            }

            foreach (var ren in renderersRight)
            {
                if (onLeft == false && lightsOn == true)
                {
                    ren.material.EnableKeyword("_EMISSION");
                }
                else
                {
                    ren.material.DisableKeyword("_EMISSION");
                }
            }
        }
    }
}