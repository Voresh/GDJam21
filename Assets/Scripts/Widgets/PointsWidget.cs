using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PointsWidget : MonoBehaviour {
    [FormerlySerializedAs("Text")]
    public Text RepairPointsText;
    public Text UpgradePointsText;
    public float PointsUpdateAnimationDuration = 0.5f;
    
    private float _LastRepairPointsUpdateTime;
    private float _LastUpgradePointsUpdateTime;
    
    private void Start() {
        RepairPointsText.text = "Repair points: 0";
        UpgradePointsText.text = "Upgrade points: 0";
        PointsController.Instance.onPointsUpdated += OnRepairPointsUpdated;
        var laboratoryModule = (LaboratoryModule) ModulesController.Instance._Modules
            .First(_ => _ is LaboratoryModule);
        laboratoryModule.onPointsUpdated += OnUpgradePointsUpdated;
    }

    private void OnRepairPointsUpdated(int points) {
        RepairPointsText.text = $"Repair points: {PointsController.Instance.Points}";
        _LastUpgradePointsUpdateTime = Time.time;
    }

    private void OnUpgradePointsUpdated(int points) {
        UpgradePointsText.text = $"Upgrade points: {points}";
        _LastRepairPointsUpdateTime = Time.time;
    }

    private void Update() {
        var pastTime = Time.time - _LastRepairPointsUpdateTime;
        if (pastTime < PointsUpdateAnimationDuration) {
            RepairPointsText.transform.localScale = Vector3.one * Mathf.Lerp(1f, 1.25f, 1 - pastTime / PointsUpdateAnimationDuration);
        }
        pastTime = Time.time - _LastUpgradePointsUpdateTime;
        if (pastTime < PointsUpdateAnimationDuration) {
            UpgradePointsText.transform.localScale = Vector3.one * Mathf.Lerp(1f, 1.25f, 1 - pastTime / PointsUpdateAnimationDuration);
        }
    }
}
