using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : JamBase<CameraController> {
    public float CameraHeight;
    public float MaxCameraHeight;
    //public float CameraRange;
    public float CameraRotation;
    public float Damping = 5f;
    public float SoftDamping = 3f;
    public float DampingDamping = 3f;
    private float _CurrentDamping;
    //private Vector3 velocity = Vector3.zero;

    void Start() {
        Quaternion.Euler(CameraRotation, 0, 0);
        _CurrentDamping = Damping;
    }

    void Update() {
        _CurrentDamping = Mathf.Lerp(_CurrentDamping, PlayerController.Instance.NearbyTarget != null ? SoftDamping : Damping, DampingDamping * Time.deltaTime);
        var targetPosition = PlayerController.Instance.Position
            + Vector3.up * (PlayerController.Instance.NearbyTarget != null ? MaxCameraHeight : CameraHeight);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * _CurrentDamping);
        //var Target = PlayerController.Instance.NearbyTarget;
        //var PlayerPosition = PlayerController.Instance.Position;
        //var CameraPosition = new Vector3(PlayerPosition.x, PlayerPosition.y + CameraHeight, PlayerPosition.z + CameraRange);
        //var FightPosition = new Vector3(PlayerPosition.x, PlayerPosition.y + MaxCameraHeight, PlayerPosition.z + CameraRange);
        //var smoothTime = Damping * (Vector3.Distance(transform.position, CameraPosition) * Time.deltaTime);
        //if (Target != null) {
        //    smoothTime = Damping * (Vector3.Distance(transform.position, FightPosition) * Time.deltaTime);
        //    CameraPosition = new Vector3(PlayerPosition.x, PlayerPosition.y + MaxCameraHeight, PlayerPosition.z + CameraRange);
        //}
        //
        //transform.position = Vector3.SmoothDamp(transform.position, CameraPosition, ref velocity, smoothTime);
    }
}
