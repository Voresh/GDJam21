using System;
using UnityEngine;

public class GameController : JamBase<GameController> {
    public void Start() {
        BotSpawner.Instance.onWaveSpawnStarted += OnWaveSpawnStarted;
    }

    private void OnWaveSpawnStarted(int wave) {
        Debug.Log($"next wave {wave + 1}");
        AnnouncementController.Instance.Schedule($"Next Wave: {wave + 1}");
    }
}
