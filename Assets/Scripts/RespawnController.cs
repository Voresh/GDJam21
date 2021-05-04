using UnityEngine;

public class RespawnController : JamBase<RespawnController> {
    public Transform SpawnPoint;
    
    private void Start() {
        PlayerController.Instance.onDied += OnPlayerDied;
    }

    private void OnPlayerDied() {
        PlayerController.Instance.transform.position = SpawnPoint.position;
    }
}
