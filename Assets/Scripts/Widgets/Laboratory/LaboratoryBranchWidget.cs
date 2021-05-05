using UnityEngine;

public class LaboratoryBranchWidget : MonoBehaviour {
    public Transform BuffsRoot;
    public LaboratoryBuffWidget BuffWidgetPrefab;

    //private List<LaboratoryBuffWidget> BranchWidgets = new List<LaboratoryBuffWidget>();
    
    //private void Setup(string branch) {
    //    foreach (var branch in BranchWidgets) {
    //        Destroy(branch.gameObject);
    //    }
    //    BranchWidgets.Clear();
    //    var laboratoryModule = (LaboratoryModule) ModulesController.Instance._Modules.First(_ => _ is LaboratoryModule);
    //    foreach (var branch in laboratoryModule.Branches) {
    //        var branchWidget = Instantiate(BranchPrefab, BranchesRoot);
    //        // branchWidget //todo: setup
    //        BranchWidgets.Add(branchWidget);
    //    }
    //}
}
