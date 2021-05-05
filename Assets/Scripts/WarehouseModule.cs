using System.Collections;
using UnityEngine;

public class WarehouseModule : Module {
    public GameObject DropPoint;
    public GameObject[] DropItems;

    public void Start() {
        BotSpawner.Instance.onWaveStatusUpdated += OnWaveStatusUpdated;
    }

    private void OnWaveStatusUpdated(bool active)
    {
        if (!active)
            Instantiate(DropItems[Random.Range(0, DropItems.Length - 1)]);
            AnnouncementController.Instance.Schedule($"Wave complete");
    }

    //protected override void UpdateEffects(bool active)
    //{
    //    if (active)
    //    {

    //    }
    //    else
    //    {
    //        return;
    //    }
    //}
}

