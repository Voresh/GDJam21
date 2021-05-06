using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : JamBase<CameraController> {
    public float CameraHeight;
    public float MaxCameraHeight;
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
        var FightPosition = new Vector3(PlayerPosition.x, PlayerPosition.y + MaxCameraHeight, PlayerPosition.z + CameraRange);
        var smoothTime = Damping * (Vector3.Distance(transform.position, CameraPosition) * Time.deltaTime);
        if (Target != null) {
            smoothTime = Damping * (Vector3.Distance(transform.position, FightPosition) * Time.deltaTime);
            CameraPosition = new Vector3(PlayerPosition.x, PlayerPosition.y + MaxCameraHeight, PlayerPosition.z + CameraRange);
        }

        transform.position = Vector3.SmoothDamp(transform.position, CameraPosition, ref velocity, smoothTime);

    }
}
