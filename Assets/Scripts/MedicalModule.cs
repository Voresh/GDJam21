using System.Collections;
using UnityEngine;

public class MedicalModule : Module {
    private Coroutine _HealCoroutine;
    public float HealRate = 2f;
    public int HealAmount = 2;
    public Transform SpawnPoint;

    protected override void AddStaticEffects() {
        PlayerController.Instance.Health.onDeadStatusUpdated += OnDeadStatusUpdated;
    }

    private void OnDeadStatusUpdated(bool dead) {
        if (!dead)
            return;
        PlayerController.Instance.transform.position = SpawnPoint.position;
        PlayerController.Instance.Health.RestoreHealth();
    }

    protected override void UpdateEffects(bool active) {
        if (active) {
            _HealCoroutine = StartCoroutine(HealPlayerCoroutine());
        }
        else {
            if (_HealCoroutine != null)
                StopCoroutine(_HealCoroutine);
        }
    }

    private IEnumerator HealPlayerCoroutine() {
        while (true) {
            yield return new WaitForSecondsRealtime(HealRate);
            PlayerController.Instance.Health.HealthCurrent += HealAmount;
        }
    }
}
