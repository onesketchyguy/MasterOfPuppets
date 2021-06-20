using UnityEngine;

// Cartoon FX  - (c) 2015 Jean Moreno
// Modified - Forrest Lowe 2021

// Indefinitely rotates an object at a constant speed

public class CFX_AutoRotate : MonoBehaviour
{
    // Rotation speed & axis
    public Vector3 rotation;

    // Rotation space
    public Space space = Space.Self;

    private Transform _transform;

    private void Start()
    {
        _transform = transform;
    }

    private void Update()
    {
        _transform.Rotate(rotation * Time.deltaTime, space);
    }
}