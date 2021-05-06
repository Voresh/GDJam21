using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Health))]
public class Module : MonoBehaviour, IRepairable {
    public bool RepairedAtStart = true;
    public GameObject RepairedView;
    public GameObject DestroyedView;
    public PriceBar PriceBarPrefab;
    public HealthBar HealthBarPrefab;
    public float HealRate = 1f;
    public int HealAmount = 5;
    public Sensor Sensor;
    public string Name;
    public string Description;
    public string Penalty;
    [FormerlySerializedAs("RepairPrice")]
    public int InitialRepairPrice;
    public float PriceHeightOffset = 2f;
    public bool Unlocked = true;
    
    public Health Health;
    private PriceBar _PriceBar;
    private HealthBar _HealthBar;
    private bool _LastDeadStatus; //hack
    private bool _Initialized;
    private float _LastHealTime;
    public Vector3 RepairablePosition => transform.position;
    public bool Repaired => !Health.Dead;
    public int RepairPrice => InitialRepairPrice;

    private void Awake() {
        Health = GetComponent<Health>();
    }

    private void Start() {
        Health.onDeadStatusUpdated += OnDeadStatusUpdated;
        if (PriceBarPrefab != null) {
            _PriceBar = Instantiate(PriceBarPrefab);
            _PriceBar.Price = RepairController.Instance.GetRepairPrice(this);
        }
        if (HealthBarPrefab != null) {
            _HealthBar = Instantiate(HealthBarPrefab);
        }
        if (RepairedAtStart)
            Health.RestoreHealth();
        else
            Health.HealthCurrent = 0;
        OnDeadStatusUpdated(Health.Dead);
        AddStaticEffects();
    }

    protected virtual void AddStaticEffects() { }

    private void OnDeadStatusUpdated(bool dead) {
        RepairedView.SetActive(!dead);
        DestroyedView.SetActive(dead);
        _PriceBar.gameObject.SetActive(dead);
        _HealthBar.gameObject.SetActive(!dead);
        if (_LastDeadStatus != dead || !_Initialized) {
            UpdateEffects(!dead);
            _Initialized = true;
        }
        _LastDeadStatus = dead;
    }

    protected virtual void UpdateEffects(bool active) { }

    private void Update() {
        if (_PriceBar != null) {
            _PriceBar.transform.position = transform.position + Vector3.up * PriceHeightOffset;
            _PriceBar.Text.text = RepairController.Instance.GetRepairPrice(this).ToString();
        }
        if (_HealthBar != null) {
            _HealthBar.transform.position = transform.position + Vector3.up * PriceHeightOffset;
            _HealthBar.Fill = (float) Health.HealthCurrent / Health.HealthMax;
        }
        if (!Health.Dead && Time.time - _LastHealTime > HealRate) {
            Health.HealthCurrent += HealAmount;
            _LastHealTime = Time.time;
        } 
    }

    public void Repair() {
        Health.RestoreHealth();
    }
}
