using System.Collections;
using UnityEngine;

public class OxygenModule : Module {
    private Coroutine _DamageCoroutine;
    public float DamageRate = 1f;
    public int DamageAmount = 1;

    protected override void UpdateEffects(bool active) {
        if (active) {
            if (_DamageCoroutine != null)
                StopCoroutine(_DamageCoroutine);
            PlayerController.Instance.SpeedBuff = 0f;
        }
        else {
            _DamageCoroutine = StartCoroutine(DamagePlayerCoroutine());
            PlayerController.Instance.SpeedBuff = -PlayerController.Instance.Speed * 0.2f;
        }
    }

    private IEnumerator DamagePlayerCoroutine() {
        while (true) {
            yield return new WaitForSeconds(DamageRate);
            PlayerController.Instance.Health.HealthCurrent -= DamageAmount;
        }
    }
}
