using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BotSpawner : JamBase<BotSpawner> {
    public Bot BotPrefab;
    public float SpawnRate = 20f;
    public List<Collider> SpawnPoints;
    public int CurrentWave = -1;
    public int BotsCount = 3; //todo: moar complicated logic

    public bool FirstWaveSpawned => CurrentWave != -1;
    public float NextWaveTime => SpawnRate - (Time.time - _LastWaveTime);
    public event Action<int> onWaveSpawnStarted = _ => { };
    public Vector3 LastSpawnPoint { get; private set; }

    private float _LastWaveTime;

    private void Start() {
        _LastWaveTime = Time.time;
    }

    private void Update() {
        if (Time.time - _LastWaveTime < SpawnRate)
            return;
        CurrentWave++;
        var bounds = SpawnPoints[UnityEngine.Random.Range(0, SpawnPoints.Count)].bounds;
        for (var i = 0; i < BotsCount; i++) {
            var position = new Vector3(UnityEngine.Random.Range(bounds.min.x, bounds.max.x), UnityEngine.Random.Range(bounds.min.y, bounds.max.y), UnityEngine.Random.Range(bounds.min.z, bounds.max.z));
            var botInstance = Instantiate(BotPrefab, position, Quaternion.identity);
            var botHealth = botInstance.GetComponent<Health>();
            botHealth.HealthMax = 10;
            botHealth.HealthCurrent = 10;
        }
        _LastWaveTime = Time.time;
        LastSpawnPoint = bounds.center;
        onWaveSpawnStarted.Invoke(CurrentWave);
    }
}