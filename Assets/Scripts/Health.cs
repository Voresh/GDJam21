using System;
using UnityEngine;

public class Health : MonoBehaviour {
    public int HealthMax = 100;
    private int _HealthCurrent;
    public float HeightOffset = 2.5f;
    public HealthBar HealthBarPrefab;
    
    private HealthBar _HealthBar;

    public int HealthCurrent {
        get => _HealthCurrent;
        set {
            _HealthCurrent = value;
            onHealthUpdated.Invoke(_HealthCurrent);
        }
    }
    
    public event Action<int> onHealthUpdated = health => {};
    
    private void Start() {
        _HealthBar = Instantiate(HealthBarPrefab);
        HealthCurrent = HealthMax;
    }

    private void Update() {
        _HealthBar.Fill = (float) HealthCurrent / HealthMax;
        _HealthBar.transform.position = transform.position + Vector3.up * HeightOffset;
    }
}
