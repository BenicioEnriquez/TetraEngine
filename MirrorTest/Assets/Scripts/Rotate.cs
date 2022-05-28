using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float sensitivity = 1;

    void Update()
    {
        if (Input.GetMouseButton(2))
        {
            transform.Rotate(Vector3.down, Input.GetAxis("Mouse X") * sensitivity);
            transform.Rotate(Vector3.right, Input.GetAxis("Mouse Y") * sensitivity);
        }
    }
}
