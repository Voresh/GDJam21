using UnityEngine;

public class GameController : JamBase<GameController> {
    public void Start() {
        PlayerController.Instance.onDied += OnPlayerDied;
    }

    private void OnPlayerDied() {
        Debug.Log("end game");
        BotSpawner.Instance.enabled = false;
    }
}
