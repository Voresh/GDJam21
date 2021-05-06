using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SecurityRoute {
    public Transform Transform;
    public Module Module;
}

public class SecurityModule : Module {
    private Coroutine _SpawnCoroutine;
    public SecurityBot SecurityBotPrefab;
    public List<SecurityRoute> Route;
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
            yield return new WaitForSeconds(SpawnRate);
            var securityBot = Instantiate(SecurityBotPrefab, SpawnPoint.position, Quaternion.identity);
            securityBot.Route = Route;
        }
    }
}
