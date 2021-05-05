using System.Collections;
using UnityEngine;

public class MedicalModule : Module {
    private Coroutine _HealCoroutine;
    public float HealRate = 2f;
    public int HealAmount = 2;

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
