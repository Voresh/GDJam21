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

    protected Health Health;
    private PriceBar _PriceBar;
    private HealthBar _HealthBar;
    private bool _LastDeadStatus; //hack
    private bool _Initialized;
    
    public bool Repaired => !Health.Dead;

    private void Start() {
        Health = GetComponent<Health>();
        Health.onDeadStatusUpdated += OnDeadStatusUpdated;
        if (PriceBarPrefab != null) {
            _PriceBar = Instantiate(PriceBarPrefab);
            _PriceBar.Price = ModulesController.Instance.GetRepairPrice(this);
        }
        if (HealthBarPrefab != null) {
            _HealthBar = Instantiate(HealthBarPrefab);
        }
        if (RepairedAtStart)
            Health.RestoreHealth();
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
            _PriceBar.Text.text = ModulesController.Instance.GetRepairPrice(this).ToString();
        }
        if (_HealthBar != null) {
            _HealthBar.transform.position = transform.position + Vector3.up * PriceHeightOffset;
            _HealthBar.Fill = (float) Health.HealthCurrent / Health.HealthMax;
        }
    }

    public void Repair() {
        Health.RestoreHealth();
    }
}
