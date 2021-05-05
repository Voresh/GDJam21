using System.Collections;
using UnityEngine;

public class WarehouseModule : Module {
    public Transform DropPoint;
    public GameObject[] DropItems;
    private bool Work;

    public void Start() {
        Work = this.RepairedAtStart;
        BotSpawner.Instance.onWaveStatusUpdated += OnWaveStatusUpdated;
    }

    private void OnWaveStatusUpdated(bool active)
    {
        if (!active && Work)
            Instantiate(DropItems[Random.Range(0, DropItems.Length - 1)], DropPoint);
    }

    protected override void UpdateEffects(bool active)
    {
        Work = active;
    }
}

