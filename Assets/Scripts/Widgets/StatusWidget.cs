using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Widgets;

public class StatusWidget : MonoBehaviour {
    public Transform Root;
    public ModuleStatusWidget ModuleStatusWidgetPrefab;

    public List<(ModuleStatusWidget, Module)> _Widgets = new List<(ModuleStatusWidget, Module)>();
    
    private IEnumerator Start() {
        yield return null;
        foreach (var module in ModulesController.Instance.Modules) {
            var instance = Instantiate(ModuleStatusWidgetPrefab, Root);
            RefreshModuleStatus(instance, module);
            _Widgets.Add((instance, module));
            module.Health.onDeadStatusUpdated += dead => {
                RefreshModuleStatus(instance, module);
            };
        }
    }

    private void Update() {
        foreach (var (widget, module) in _Widgets) {
            widget.Arrow.gameObject.SetActive(!module.Repaired);
            var targetRotation = WaveArrowWidget.GetRouteDirection(module.transform.position);
            widget.Arrow.rotation = Quaternion.Slerp(widget.Arrow.rotation, targetRotation, 10f * Time.deltaTime);
            if (module.Repaired)
                widget.transform.SetAsFirstSibling();
            else 
                widget.transform.SetAsLastSibling();
        }
    }

    private void RefreshModuleStatus(ModuleStatusWidget instance, Module module) {
        instance.Text.text = $"{module.Name} ({(module.Unlocked ? (module.Repaired ? "<color=green>Repaired</color>" : "<color=red>Destroyed</color>") : "<color=red>Locked</color>")})";
    }
}
