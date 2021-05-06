using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotation : MonoBehaviour
{
    public float RotationSpeed = 45f;

    void Update() {
        transform.Rotate(new Vector3(0, RotationSpeed, 0) * Time.deltaTime);
    }
}
