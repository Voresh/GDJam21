using System;
using UnityEngine;

public class Health : MonoBehaviour {
    public int HealthMax = 100;
    private int _HealthCurrent;
    public float HeightOffset = 2.5f;
    public HealthBar HealthBarPrefab;
    
    private HealthBar _HealthBar;
    public bool Dead { get; private set; }
    
    public int HealthCurrent {
        get => _HealthCurrent;
        set {
            var lastHealth = _HealthCurrent;
            _HealthCurrent = Mathf.Max(0, value);
            if (!Dead && _HealthCurrent == 0) {
                Dead = true;
                onDeadStatusUpdated.Invoke(Dead);
            }
            else if (Dead && _HealthCurrent > 0) {
                Dead = false;
                onDeadStatusUpdated.Invoke(Dead);
            }
            if (lastHealth != _HealthCurrent)
                onHealthUpdated.Invoke(_HealthCurrent);
        }
    }
    
    public event Action<int> onHealthUpdated = health => {};
    public event Action<bool> onDeadStatusUpdated = dead => {};
    
    private void Start() {
        if (HealthBarPrefab != null)
            _HealthBar = Instantiate(HealthBarPrefab);
        HealthCurrent = HealthMax;
    }

    private void Update() {
        if (_HealthBar == null)
            return;
        _HealthBar.Fill = (float) HealthCurrent / HealthMax;
        _HealthBar.transform.position = transform.position + Vector3.up * HeightOffset;
    }
}
