using System;
using UnityEngine;

public class GameController : JamBase<GameController> {
    public void Start() {
        PlayerController.Instance.onDied += OnPlayerDied;
        BotSpawner.Instance.onWaveSpawnStarted += OnWaveSpawnStarted;
    }

    private void OnPlayerDied() {
        Debug.Log("end game");
        BotSpawner.Instance.enabled = false;
    }

    private void OnWaveSpawnStarted(int wave) {
        Debug.Log($"next wave {wave + 1}");
        AnnouncementController.Instance.Schedule($"Next Wave: {wave + 1}");
    }
}
