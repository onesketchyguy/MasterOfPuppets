using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuppetMaster.Effects
{
    public class ScrollTextureEffect : MonoBehaviour
    {
        [SerializeField] private Renderer m_renderer;

        [SerializeField] private Vector2 offsetAmount = Vector2.up;

        private Material scrollingMat;

        private Vector2 currentOffset;

        private void OnValidate()
        {
            if (m_renderer == null)
            {
                m_renderer = GetComponent<Renderer>();
            }
        }

        // Start is called before the first frame update
        private void Start()
        {
            scrollingMat = m_renderer.material;
        }

        // Update is called once per frame
        private void Update()
        {
            currentOffset += offsetAmount * Time.deltaTime;

            scrollingMat.mainTextureOffset = currentOffset;
        }
    }
}