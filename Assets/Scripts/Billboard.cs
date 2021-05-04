using UnityEngine;

public class Billboard : MonoBehaviour {
    public Transform RotationTarget;

    protected virtual void Start() {
        RotationTarget = CameraController.Instance.transform;
    }

    protected virtual void Update() {
        if (RotationTarget != null) {
            transform.forward = RotationTarget.transform.position - transform.position;
            var eulerRotation = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(90f, 0f, eulerRotation.z);
        }
    }
}
