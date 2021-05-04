using UnityEngine;

[RequireComponent(typeof(Health))]
public class Module : MonoBehaviour {
    public GameObject RepairedView;
    public GameObject DestroyedView;
    public PriceBar PriceBarPrefab;
    public int RepairPrice;
    public float PriceHeightOffset = 2f;

    private Health _Health;
    private PriceBar _PriceBar;

    private void Start() {
        _Health = GetComponent<Health>();
        _Health.onDeadStatusUpdated += OnDeadStatusUpdated;
        if (PriceBarPrefab != null) {
            _PriceBar = Instantiate(PriceBarPrefab);
            _PriceBar.Price = RepairPrice;
        }
    }

    private void OnDeadStatusUpdated(bool dead) {
        RepairedView.SetActive(!dead);
        DestroyedView.SetActive(dead);
    }

    private void Update() {
        _PriceBar.transform.position = transform.position + Vector3.up * PriceHeightOffset;
    }
}
