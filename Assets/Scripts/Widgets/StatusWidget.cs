using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            var worldDirection = module.transform.position - PlayerController.Instance.Position;
            var rotation = Quaternion.LookRotation(worldDirection, Vector3.up);
            var y = rotation.eulerAngles.y;
            widget.Arrow.rotation = Quaternion.Slerp(widget.Arrow.rotation, Quaternion.Euler(0f, 0f, -y), 10f * Time.deltaTime);
        }
    }

    private void RefreshModuleStatus(ModuleStatusWidget instance, Module module) {
        instance.Text.text = $"{module.Name} ({(module.Repaired ? "<color=green>Repaired</color>" : "<color=red>Destroyed</color>")})";
    }
}
