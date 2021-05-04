using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float CameraHeight;
    public float CameraRange;
    public float CameraRotation;

    void Start()
    {
        Debug.LogError("KUKU");
    }

    void Update()
    {
        var PlayerPosition = PlayerController.Instance.Position;
        var CameraPosition = new Vector3(PlayerPosition.x, PlayerPosition.y + CameraHeight, PlayerPosition.z + CameraRange);
        transform.position = CameraPosition;
        transform.rotation = Quaternion.Euler(CameraRotation, 0, 0);
    }
}
