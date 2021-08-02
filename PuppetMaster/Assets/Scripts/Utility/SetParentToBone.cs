// Forrest Lowe 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    // FIXME: NEEDS CODE REVIEW
    public class SetParentToBone : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private Transform parentToSearchUnder = null;

        [SerializeField] private string nameOfParentToFind = "";

        [Tooltip("If false, will search until finds last then uses that object, otherwise uses first object found.")]
        [SerializeField] private bool useFirst = true;

        [Header("Offset")]
        [SerializeField] private Vector3 defaultRotation = Vector3.zero;

        [SerializeField] private Vector3 offsetFromParent = Vector3.zero;

        private void Start()
        {
            Transform newParent = null;

            foreach (var child in parentToSearchUnder.GetComponentsInChildren<Transform>())
            {
                if (child.name == nameOfParentToFind)
                {
                    if (useFirst == false)
                    {
                        newParent = child;
                    }
                    else
                    {
                        SetParent(child);

                        return;
                    }
                }
            }

            SetParent(newParent);
        }

        private void SetParent(Transform newParent)
        {
            transform.SetParent(newParent);
            transform.localPosition = offsetFromParent;
            transform.localRotation = Quaternion.Euler(defaultRotation);
        }
    }
}