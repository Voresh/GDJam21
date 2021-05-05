using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BotSpawner : JamBase<BotSpawner> {
    public Bot BotPrefab;
    [FormerlySerializedAs("SpawnRate")]
    public float StartSpawnRate = 10f;
    public List<Collider> SpawnPoints;
    public int CurrentWave = -1;
    public int BotsCount => Fibonacci(CurrentWave + 1) * 5;

    public bool FirstWaveSpawned => CurrentWave != -1;
    public List<Bot> CurrentWaveBots = new List<Bot>();
    public float SpawnRate => StartSpawnRate + Math.Max(0, CurrentWave * 5);
    public float NextWaveTime => SpawnRate - (Time.time - _LastWaveTime);
    public event Action<int> onWaveSpawnStarted = _ => { };
    public event Action<Bot> onBotSpawned = _ => { };
    public Vector3 LastSpawnPoint { get; private set; }

    private float _LastWaveTime;

    private void Start() {
        _LastWaveTime = Time.time;
    }

    private static int Fibonacci(int n) {
        if (n <= 0)
            return 0;
        if (n == 1)
            return 1;
        return Fibonacci(n - 1) + Fibonacci(n - 2);
    }

    private void Update() {
        if (NextWaveTime > 0f)
            return;
        CurrentWave++;
        var bounds = SpawnPoints[UnityEngine.Random.Range(0, SpawnPoints.Count)].bounds;
        CurrentWaveBots.Clear();
        var botsCount = BotsCount;
        Debug.Log(botsCount);
        for (var i = 0; i < botsCount; i++) {
            var position = new Vector3(UnityEngine.Random.Range(bounds.min.x, bounds.max.x), UnityEngine.Random.Range(bounds.min.y, bounds.max.y), UnityEngine.Random.Range(bounds.min.z, bounds.max.z));
            var botInstance = Instantiate(BotPrefab, position, Quaternion.identity);
            var botHealth = botInstance.GetComponent<Health>();
            botHealth.HealthMax = 10;
            botHealth.HealthCurrent = 10;
            CurrentWaveBots.Add(botInstance);
            onBotSpawned.Invoke(botInstance);
        }
        _LastWaveTime = Time.time;
        LastSpawnPoint = bounds.center;
        onWaveSpawnStarted.Invoke(CurrentWave);
    }
}