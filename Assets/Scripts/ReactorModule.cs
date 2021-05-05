using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReactorModule : Module {
    protected override void AddStaticEffects() {
        PlayerController.Instance.Health.onDeadStatusUpdated += OnDeadStatusUpdated;
    }

    private void OnDeadStatusUpdated(bool dead) {
        if (Health.Dead) {
            AnnouncementController.Instance.Schedule("Game over!");
            Time.timeScale = 0f;
            StartCoroutine(Delay(() => {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }, 2f));
        }    
    }

    private IEnumerator Delay(Action action, float delay) {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }
}
