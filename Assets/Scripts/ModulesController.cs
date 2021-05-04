using System.Collections.Generic;
using System.Linq;

public class ModulesController : JamBase<ModulesController> {
    public List<Module> _Modules = new List<Module>();
    public float InteractionRadius = 3f;
    private Module _ClosestModule;

    public bool ReadyToInteract => _ClosestModule != null;

    private void Update() {
        var closestModule = _Modules
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
        if (PointsController.Instance.Points < _ClosestModule.RepairPrice)
            return;
        PointsController.Instance.Points -= _ClosestModule.RepairPrice;
        _ClosestModule.Repair();
    }
}
