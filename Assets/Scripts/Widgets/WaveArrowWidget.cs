using UnityEngine;

namespace Widgets {
    public class WaveArrowWidget : MonoBehaviour {
        public Transform Arrow;
        
        private void Update() {
            if (!BotSpawner.Instance.FirstWaveSpawned) {
                Arrow.gameObject.SetActive(false);
                return;
            }
            Arrow.gameObject.SetActive(true);
            var worldDirection = BotSpawner.Instance.LastSpawnPoint - PlayerController.Instance.Position;
            var rotation = Quaternion.LookRotation(worldDirection, Vector3.up);
            var y = rotation.eulerAngles.y;
            Arrow.rotation = Quaternion.Euler(0f, 0f, -y);
        }
    }
}
