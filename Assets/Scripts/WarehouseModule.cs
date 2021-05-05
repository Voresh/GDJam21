using System.Collections;
using UnityEngine;

public class WarehouseModule : Module {
    public Transform DropPoint;
    public GameObject[] DropItems;
    private bool Work;

    public GameObject Aidkit;
    
    protected override void AddStaticEffects() {
        Work = this.RepairedAtStart;
        BotSpawner.Instance.onWaveStatusUpdated += OnWaveStatusUpdated;
    }

    private void OnWaveStatusUpdated(bool active) {
        if (!active && Work && Aidkit == null) {
            var Drop = Instantiate(DropItems[Random.Range(0, DropItems.Length - 1)]);
            Drop.transform.position = DropPoint.position;
            Aidkit = Drop;
        }
    }

    protected override void UpdateEffects(bool active) {
        Work = active;
    }
}