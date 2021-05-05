using UnityEngine;

public class ReactorModule : Module {
    protected override void AddStaticEffects() {
        PlayerController.Instance.Health.onDeadStatusUpdated += OnDeadStatusUpdated;
    }

    private void OnDeadStatusUpdated(bool dead) {
        if (Health.Dead) {
            AnnouncementController.Instance.Schedule("Game over!");
            Time.timeScale = 0f;
        }    
    }
}
