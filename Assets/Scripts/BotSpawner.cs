using System;
using UnityEngine;

public class BotSpawner : JamBase<BotSpawner> {
    public Bot BotPrefab;
    public float SpawnRate = 20f;
    public Collider TriggerCollider;
    public int CurrentWave = -1;
    public float NextWaveTime => SpawnRate - (Time.time - _LastWaveTime);
        
    public event Action<int> onWaveSpawnStarted = _ => { };
    
    private float _LastWaveTime;

    private void Start() {
        _LastWaveTime = Time.time;
    }

    private void Update() {
        if (Time.time - _LastWaveTime < SpawnRate)
            return;
        CurrentWave++;
        var bounds = TriggerCollider.bounds;
        var botInstance = Instantiate(BotPrefab, new Vector3(UnityEngine.Random.Range(bounds.min.x, bounds.max.x), UnityEngine.Random.Range(bounds.min.y, bounds.max.y), UnityEngine.Random.Range(bounds.min.z, bounds.max.z)), Quaternion.identity);
        var botHealth = botInstance.GetComponent<Health>();
        botHealth.HealthMax = 10;
        botHealth.HealthCurrent = 10;
        _LastWaveTime = Time.time;
        onWaveSpawnStarted.Invoke(CurrentWave);
    }
}