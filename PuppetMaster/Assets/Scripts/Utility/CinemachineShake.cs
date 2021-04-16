using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine
{
    public class CinemachineShake : MonoBehaviour
    {
        private CinemachineVirtualCamera vc;
        private CinemachineBasicMultiChannelPerlin perlinNoise;

        public float decayTime = 50;

        private void OnValidate()
        {
            if (vc == null)
            {
                vc = GetComponent<CinemachineVirtualCamera>();
            }

            if (perlinNoise == null)
            {
                perlinNoise = vc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            }
        }

        private void Start()
        {
            OnValidate();
        }

        private void Update()
        {
            if (perlinNoise.m_AmplitudeGain > 0)
            {
                perlinNoise.m_AmplitudeGain = Mathf.MoveTowards(perlinNoise.m_AmplitudeGain, 0, decayTime * Time.deltaTime);
            }
        }

        public void Shake(float intensity)
        {
            perlinNoise.m_AmplitudeGain = intensity;
        }
    }
}