using UnityEngine;

public class GameController : JamBase<GameController> {
    public void Start() {
        BotSpawner.Instance.onWaveSpawnStarted += OnWaveSpawnStarted;
        BotSpawner.Instance.onWaveStatusUpdated += OnWaveStatusUpdated;
        PlayerController.Instance.Health.onDeadStatusUpdated += OnDeadStatusUpdated;
    }

    private void OnWaveStatusUpdated(bool active) {
        if (!active)
            AnnouncementController.Instance.Schedule($"Wave complete");
    }

    private void OnDeadStatusUpdated(bool dead) {
        if (!dead)
            AnnouncementController.Instance.Schedule("Player respawned!");
    }

    private void OnWaveSpawnStarted(int wave) {
        Debug.Log($"next wave {wave + 1}");
        AnnouncementController.Instance.Schedule($"Wave: {wave + 1}");
    }
}
