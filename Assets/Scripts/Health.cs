using System;
using UnityEngine;

public class Health : MonoBehaviour {
    public int StartMaxHealth = 100;
    public int HealthMax => StartMaxHealth + Mathf.RoundToInt(StartMaxHealth * GlobalHealthBuff);
    private int _HealthCurrent;
    public float HeightOffset = 2.5f;
    public HealthBar HealthBarPrefab;
    public float GlobalHealthBuff;
    private HealthBar _HealthBar;
    public bool Dead { get; private set; } = true;
    public bool Invulnerable;
    public int HealthCurrent {
        get => _HealthCurrent;
        set {
            if (value < _HealthCurrent && Invulnerable)
                return;
            var lastHealth = _HealthCurrent;
            _HealthCurrent = Mathf.Clamp(value, 0, HealthMax);
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
    }

    public void RestoreHealth() {
        HealthCurrent = HealthMax;
    }
    
    private void Update() {
        if (_HealthBar == null)
            return;
        _HealthBar.Fill = (float) HealthCurrent / HealthMax;
        _HealthBar.transform.position = transform.position + Vector3.up * HeightOffset;
        _HealthBar.gameObject.SetActive(HealthCurrent > 0);
    }
}
