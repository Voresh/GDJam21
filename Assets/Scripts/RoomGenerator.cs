using System;
using UnityEngine;

public class RoomGenerator : JamBase<RoomGenerator> {
    public Bot BotPrefab;
    public float SpawnRate = 2f;
    public Collider TriggerCollider;

    private float _LastSpawnTime;
    
    private void Update() {
        if (Time.time - _LastSpawnTime < SpawnRate)
            return;
        var bounds = TriggerCollider.bounds;
        var botInstance = Instantiate(BotPrefab, new Vector3(UnityEngine.Random.Range(bounds.min.x, bounds.max.x), UnityEngine.Random.Range(bounds.min.y, bounds.max.y), UnityEngine.Random.Range(bounds.min.z, bounds.max.z)), Quaternion.identity);
        var botHealth = botInstance.GetComponent<Health>();
        botHealth.HealthMax = 10;
        botHealth.HealthCurrent = 10;
        _LastSpawnTime = Time.time;
    }
}