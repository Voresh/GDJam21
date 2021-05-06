using System.Collections;
using System.Linq;
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
        if (!ModulesController.Instance.Modules.First(_ => _.GetComponent<ReactorModule>() != null).Repaired) {
            Debug.Log("medical module not repaired - not restoring player");
            return;
        }
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn() {
        PlayerController.Instance.enabled = false;

        DeathWidgetController.Instance.Active = true;
        yield return new WaitForSeconds(2f);
        DeathWidgetController.Instance.Active = false;
        
        PlayerController.Instance.NavMeshAgent.enabled = false;
        PlayerController.Instance.transform.position = SpawnPoint.position;
        PlayerController.Instance.NavMeshAgent.enabled = true;
        
        //yield return new WaitForSeconds(1f);
        
        PlayerController.Instance.enabled = true;
        
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
            yield return new WaitForSeconds(HealRate);
            if (!PlayerController.Instance.Health.Dead)
                PlayerController.Instance.Health.HealthCurrent += HealAmount;
        }
    }
}
