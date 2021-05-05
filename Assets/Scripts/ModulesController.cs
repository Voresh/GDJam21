using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class ModulesController : JamBase<ModulesController> {
    [FormerlySerializedAs("_Modules")]
    public List<Module> Modules = new List<Module>();
    public float InteractionRadius = 3f;
    private Module _ClosestModule;

    public bool ReadyToInteract => _ClosestModule != null;
    public bool InteractionAvailable => ReadyToInteract && PointsController.Instance.Points >= GetRepairPrice(_ClosestModule);
    public float GlobalRepairBuff { get; set; }

    private void Update() {
        var closestModule = Modules
            .Where(_ => !_.Repaired)
            .OrderBy(_ => (_.transform.position - PlayerController.Instance.Position).sqrMagnitude)
            .FirstOrDefault();
        if (closestModule == null) {
            _ClosestModule = null;
            return;
        }
        if ((closestModule.transform.position - PlayerController.Instance.Position).sqrMagnitude < InteractionRadius * InteractionRadius) {
            _ClosestModule = closestModule;
        }
        else {
            _ClosestModule = null;    
        }
    }

    public void TryInteract() {
        if (!ReadyToInteract)
            return;
        var repairPrice = GetRepairPrice(_ClosestModule);
        if (PointsController.Instance.Points < repairPrice)
            return;
        PointsController.Instance.Points -= repairPrice;
        _ClosestModule.Repair();
    }

    public int GetRepairPrice(Module module) {
        return Mathf.Max(0, module.RepairPrice + Mathf.RoundToInt(module.RepairPrice * GlobalRepairBuff));
    }
}
