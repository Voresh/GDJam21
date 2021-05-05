using UnityEngine;

[RequireComponent(typeof(Health))]
public class Module : MonoBehaviour {
    public bool RepairedAtStart = true;
    public GameObject RepairedView;
    public GameObject DestroyedView;
    public PriceBar PriceBarPrefab;
    public HealthBar HealthBarPrefab;
    public int RepairPrice;
    public float PriceHeightOffset = 2f;

    private Health _Health;
    private PriceBar _PriceBar;
    private HealthBar _HealthBar;
    private bool _LastDeadStatus; //hack
    private bool _Initialized;
    
    public bool Repaired => !_Health.Dead;

    private void Start() {
        _Health = GetComponent<Health>();
        _Health.onDeadStatusUpdated += OnDeadStatusUpdated;
        if (PriceBarPrefab != null) {
            _PriceBar = Instantiate(PriceBarPrefab);
            _PriceBar.Price = RepairPrice;
        }
        if (HealthBarPrefab != null) {
            _HealthBar = Instantiate(HealthBarPrefab);
        }
        if (RepairedAtStart)
            _Health.RestoreHealth();
        OnDeadStatusUpdated(_Health.Dead);
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
        if (_PriceBar != null)
            _PriceBar.transform.position = transform.position + Vector3.up * PriceHeightOffset;
        if (_HealthBar != null) {
            _HealthBar.transform.position = transform.position + Vector3.up * PriceHeightOffset;
            _HealthBar.Fill = (float) _Health.HealthCurrent / _Health.HealthMax;
        }
    }

    public void Repair() {
        _Health.RestoreHealth();
    }
}
