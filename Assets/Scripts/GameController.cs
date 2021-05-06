using System.Collections;
using UnityEngine;

public class GameController : JamBase<GameController> {
    public IEnumerator Start() {
        BotSpawner.Instance.onWaveSpawnStarted += OnWaveSpawnStarted;
        BotSpawner.Instance.onWaveStatusUpdated += OnWaveStatusUpdated;
        PlayerController.Instance.Health.onDeadStatusUpdated += OnDeadStatusUpdated;
        AnnouncementController.Instance.Schedule($"Game Started");
        foreach (var module in ModulesController.Instance.Modules) {
            module.Sensor.onTriggerEnter += collider => {
                if (collider.gameObject != PlayerController.Instance.gameObject)
                    return;
                AnnouncementController.Instance.Schedule($"{module.Name} module", $"{module.Description}");
            };
        }
        yield return null; //hack to skip modules initial state announcements
        foreach (var module in ModulesController.Instance.Modules) {
            module.Health.onDeadStatusUpdated += dead => {
                if (dead)
                    AnnouncementController.Instance.Schedule($"{module.Name} module destroyed!");
                else
                    AnnouncementController.Instance.Schedule($"{module.Name} module repaired!");
            };
        }
    }

    private void OnWaveStatusUpdated(bool active) {
        if (!active)
            AnnouncementController.Instance.Schedule($"Wave complete");
    }

    private void OnDeadStatusUpdated(bool dead) {
        //if (!dead)
        //    AnnouncementController.Instance.Schedule("Player respawned!");
    }

    private void OnWaveSpawnStarted(int wave) {
        Debug.Log($"next wave {wave + 1}");
        AnnouncementController.Instance.Schedule($"Wave: {wave + 1}");
    }
}
