using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityModule : Module {
    private Coroutine _SpawnCoroutine;
    public SecurityBot SecurityBotPrefab;
    public List<Transform> Route;
    public Transform SpawnPoint;
    public float SpawnRate = 10f;

    protected override void UpdateEffects(bool active) {
        if (active) {
            _SpawnCoroutine = StartCoroutine(SpawnSecurityBotCoroutine());
        }
        else {
            if (_SpawnCoroutine != null)
                StopCoroutine(_SpawnCoroutine);
        }
    }

    private IEnumerator SpawnSecurityBotCoroutine() {
        while (true) {
            yield return new WaitForSecondsRealtime(SpawnRate);
            var securityBot = Instantiate(SecurityBotPrefab, SpawnPoint.position, Quaternion.identity);
            securityBot.Route = Route;
        }
    }
}
