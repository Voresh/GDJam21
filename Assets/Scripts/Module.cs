using System;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Module : MonoBehaviour {
    public GameObject RepairedView;
    public GameObject DestroyedView;
    private Health _Health;

    private void Start() {
        _Health = GetComponent<Health>();
        _Health.onDeadStatusUpdated += OnDeadStatusUpdated;
    }

    private void OnDeadStatusUpdated(bool dead) {
        RepairedView.SetActive(!dead);
        DestroyedView.SetActive(dead);
    }
}
