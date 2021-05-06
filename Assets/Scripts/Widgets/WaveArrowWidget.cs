using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Widgets {
    public class WaveArrowWidget : MonoBehaviour {
        public Transform Arrow;

        private void Update() {
            if (!BotSpawner.Instance.FirstWaveSpawned 
                || BotSpawner.Instance.CurrentWaveBots.Count == 0 
                || BotSpawner.Instance.CurrentWaveBots.All(_ => _.Health.Dead)) { //todo: remake to wave status upd
                Arrow.gameObject.SetActive(false);
                return;
            }
            Arrow.gameObject.SetActive(true);
            var targetPosition = BotSpawner.Instance.CurrentWaveBots.First(_ => !_.Health.Dead).transform.position;
            var targetRotation = GetRouteDirection(targetPosition);
            Arrow.rotation = Quaternion.Slerp(Arrow.rotation, targetRotation, 7f * Time.deltaTime);
        }

        public static Quaternion GetRouteDirection(Vector3 targetPosition) {
            var path = new NavMeshPath();
            var pathSuccess = NavMesh.CalculatePath(PlayerController.Instance.Position, targetPosition, NavMesh.AllAreas, path) && path.corners.Length > 1;
            var worldDirection = pathSuccess
                ? path.corners[1] - PlayerController.Instance.Position
                : targetPosition - PlayerController.Instance.Position;
            //Debug.DrawRay(path.corners[1], Vector3.up * 10, Color.green, 2f);
            //Debug.DrawRay(transform.position, worldDirection, Color.green, 2f);
            //Debug.LogError($"{pathSuccess} {path.corners.Length}");
            var rotation = Quaternion.LookRotation(worldDirection, Vector3.up);
            return Quaternion.Euler(0f, 0f, -rotation.eulerAngles.y);
        }
    }
}
