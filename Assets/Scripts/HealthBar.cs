using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    public Image FillImage;

    public Transform RotationTarget;
    
    public float Fill {
        set => FillImage.fillAmount = value;
    }

    private void Start() {
        RotationTarget = CameraController.Instance.transform;
    }

    private void Update() {
        if (RotationTarget != null) {
            transform.forward = RotationTarget.transform.position - transform.position;
            var eulerRotation = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(-90f, 180f, eulerRotation.z);
        }
    }
}
