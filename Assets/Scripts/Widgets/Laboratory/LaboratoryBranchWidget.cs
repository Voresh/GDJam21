using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaboratoryBranchWidget : MonoBehaviour {
    public Transform BuffsRoot;
    public LaboratoryBuffWidget BuffWidgetPrefab;

    private List<LaboratoryBuffWidget> BuffWidgets = new List<LaboratoryBuffWidget>();
    
    public void Setup(string branchId) {
        foreach (var buff in BuffWidgets) {
            Destroy(buff.gameObject);
        }
        BuffWidgets.Clear();
        var laboratoryModule = (LaboratoryModule) ModulesController.Instance._Modules
            .First(_ => _ is LaboratoryModule);
        var branch = laboratoryModule.Branches
            .First(_ => _.Name == branchId);
        for (var index = 0; index < branch.Buffs.Count; index++) {
            if (index == 0)
                continue; //hack
            var buff = branch.Buffs[index];
            var buffWidget = Instantiate(BuffWidgetPrefab, BuffsRoot);
            var upgraded = laboratoryModule.UpgradeComplete(branchId, index);
            var upgradedText = upgraded ? $"\n<i><color=green>Upgrade complete</color></i>" : string.Empty;
            var notEnoughPointsText = !upgraded && !laboratoryModule.EnoughPoints(branchId, index) ? $"\n<i><color=red>Not enough points</color></i>" : string.Empty;
            buffWidget.Text.text = $"{buff.Type} {(buff.Amount > 0 ? "+" : string.Empty)}{buff.Amount * 100}%\nPrice: {buff.Price}{upgradedText}{notEnoughPointsText}";
            buffWidget.Button.interactable = laboratoryModule.UpgradeAvailable(branchId, index);
            //buffWidget.UpgradedRoot.SetActive(laboratoryModule.UpgradeComplete(branchId, index));
            buffWidget.Button.onClick.AddListener(() => {
                laboratoryModule.UpgradeBranchProgress(branchId);
                Setup(branchId);
            });
            BuffWidgets.Add(buffWidget);
        }
    }
}
