using System.Linq;
using UnityEngine;

public class RepairController : JamBase<RepairController> {
    public float InteractionRadius = 3f;
    private IRepairable _ClosestRepairable;

    public bool ReadyToInteract => _ClosestRepairable != null;
    public bool InteractionAvailable => ReadyToInteract && PointsController.Instance.Points >= GetRepairPrice(_ClosestRepairable);
    public float GlobalRepairBuff { get; set; }

    private void Update() {
        var closestModule = ModulesController.Instance.Modules
            .Select(_ => (IRepairable) _)
            .Concat(DoorsModule.Instance.Doors.Select(_ => (IRepairable) _))
            .Where(_ => !_.Repaired)
            .OrderBy(_ => (_.RepairablePosition - PlayerController.Instance.Position).sqrMagnitude)
            .FirstOrDefault();
        if (closestModule == null) {
            _ClosestRepairable = null;
            return;
        }
        if ((closestModule.RepairablePosition - PlayerController.Instance.Position).sqrMagnitude < InteractionRadius * InteractionRadius) {
            _ClosestRepairable = closestModule;
        }
        else {
            _ClosestRepairable = null;    
        }
    }

    public void TryInteract() {
        if (!ReadyToInteract)
            return;
        var repairPrice = GetRepairPrice(_ClosestRepairable);
        if (PointsController.Instance.Points < repairPrice)
            return;
        PointsController.Instance.Points -= repairPrice;
        _ClosestRepairable.Repair();
    }

    public int GetRepairPrice(IRepairable module) {
        return Mathf.Max(0, module.RepairPrice + Mathf.RoundToInt(module.RepairPrice * GlobalRepairBuff));
    }
}
