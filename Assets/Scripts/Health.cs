using System;
using UnityEngine;

public class Health : MonoBehaviour {
    public int HealthMax = 100;
    public int HealthCurrent;
    public float HeightOffset = 2.5f;
    public HealthBar HealthBarPrefab;
    
    private HealthBar _HealthBar;

    private void Start() {
        _HealthBar = Instantiate(HealthBarPrefab);
    }

    private void Update() {
        _HealthBar.Fill = (float) HealthCurrent / HealthMax;
        _HealthBar.transform.position = transform.position + Vector3.up * HeightOffset;
    }
}
