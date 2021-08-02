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
        private Material matLeft = null, matRight = null;

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

            matLeft = renderersLeft[0].material;
            matRight = renderersRight[0].material;

            SetLightsEnabled(lightsOn);
            SetSoundEnabled(soundOn);
        }

        public void SetLightsEnabled(bool enabled)
        {
            lightsOn = enabled;

            if (lightsOn)
            {
                InvokeRepeating(nameof(Switch), alternateSpeed, alternateSpeed);
            }
            else
            {
                CancelInvoke(nameof(Switch));
            }
        }

        public void SetSoundEnabled(bool enabled)
        {
            soundOn = enabled;

            // FIXME: Fade out the sound here
            sirenSound.enabled = enabled;
        }

        private void Switch()
        {
            foreach (var ren in renderersLeft)
                ren.material = onLeft ? matLeft : matRight;

            foreach (var ren in renderersRight)
                ren.material = onLeft ? matRight : matLeft;

            onLeft = !onLeft;
        }
    }
}