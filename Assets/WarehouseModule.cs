using System.Collections;
using UnityEngine;

public class WarehouseModule : Module {
    public GameObject SpawnPoin;

    private void onWaveStatusUpdated(int wave) {
    }

    protected override void UpdateEffects(bool active)
    {
        if (active)
        {
            BotSpawner.Instance.onWaveStatusUpdated;
        }
        else
        {
            return;
        }
    }
}

