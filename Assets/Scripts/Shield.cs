using System;
using UnityEngine;

public class Shield : MonoBehaviour {
    public Health Health;
    public GameObject Attachement;
    
    private void Start() {
        Health = GetComponent<Health>();
        Health.RestoreHealth();
        Health.onDeadStatusUpdated += dead => {
            if (dead)
                Destroy(gameObject);
        };
    }

    private void Update() {
        if (Attachement != null) {
            transform.SetParent(Attachement.transform);
            transform.localPosition = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }
    }

    private void LateUpdate() {
        transform.rotation = Quaternion.identity;
    }

    private void FixedUpdate() {
        transform.rotation = Quaternion.identity;
    }
}
