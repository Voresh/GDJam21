using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : JamBase<CameraController> {
    public float CameraHeight;
    public float CameraRange;
    public float CameraRotation;
    public float Damping = 7f;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        
        Quaternion.Euler(CameraRotation, 0, 0);
    }
    
    void Update()
    {
        var Target = PlayerController.Instance.NearbyTarget;
        var PlayerPosition = PlayerController.Instance.Position;
        var CameraPosition = new Vector3(PlayerPosition.x, PlayerPosition.y + CameraHeight, PlayerPosition.z + CameraRange);    
        var distance = Vector3.Distance(transform.position, CameraPosition);
        var smoothTime = distance * (Time.deltaTime * Damping);
        if (Target != null) {
            CameraPosition = new Vector3(PlayerPosition.x, PlayerPosition.y + CameraHeight * 1.2f, PlayerPosition.z + CameraRange);
            smoothTime = 0.3f;
        }

        transform.position = Vector3.SmoothDamp(transform.position, CameraPosition, ref velocity, smoothTime);

    }
}
