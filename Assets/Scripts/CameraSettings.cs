using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSettings : MonoBehaviour
{
    public float CameraHeight;
    public float CameraRange;
    public float CameraRotation;

    void Start()
    {
        transform.rotation = Quaternion.Euler(CameraRotation, 0, 0);
    }
    
    void Update()
    {
        var PlayerPosition = PlayerController.Instance.Position;
        var CameraPosition = new Vector3(PlayerPosition.x, PlayerPosition.y + CameraHeight, PlayerPosition.z + CameraRange);
        transform.position = CameraPosition;
    }
}
