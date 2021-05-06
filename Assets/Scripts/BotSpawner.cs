using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class BotSpawner : JamBase<BotSpawner> {
    public Bot BotPrefab;
    [FormerlySerializedAs("SpawnRate")]
    public float StartSpawnRate = 10f;
    public List<Collider> SpawnPoints;
    public int CurrentWave = -1;
    public float FirstWaveDelay = 10;
    public int BotsCount => Fibonacci(CurrentWave + 1) * 3;

    public bool FirstWaveSpawned => CurrentWave != -1;
    public List<Bot> CurrentWaveBots = new List<Bot>();
    public float SpawnRateBuff;
    public float SpawnRate => FirstWave ? FirstWaveDelay : StartSpawnRate + SpawnRateBuff /*+ Math.Max(0, CurrentWave * 5)*/;
    public float NextWaveTime => SpawnRate - (Time.time - _LastWaveCompleteTime);
    public event Action<int> onWaveSpawnStarted = _ => { };
    public event Action<Bot> onBotSpawned = _ => { };
    public event Action<bool> onWaveStatusUpdated = _ => { };
    public Vector3 LastSpawnPoint { get; private set; }

    private float _LastWaveCompleteTime;
    public bool WaveInProgress { get; private set; }

    public bool FirstWave = true;
    
    private void Start() {
        _LastWaveCompleteTime = Time.time;
    }

    private static int Fibonacci(int n) {
        if (n <= 0)
            return 0;
        if (n == 1)
            return 1;
        return Fibonacci(n - 1) + Fibonacci(n - 2);
    }

    private void Update() {
        if (WaveInProgress) {
            if (CurrentWaveBots.All(_ => _.Health.Dead)) {
                _LastWaveCompleteTime = Time.time;
                WaveInProgress = false;
                onWaveStatusUpdated.Invoke(WaveInProgress);
            }
            return;
        }
        if (NextWaveTime > 0f)
            return;
        CurrentWave++;
        var bounds = SpawnPoints[UnityEngine.Random.Range(0, SpawnPoints.Count)].bounds;
        CurrentWaveBots.Clear();
        var botsCount = BotsCount;
        FirstWave = false;
        Debug.Log(botsCount);
        for (var i = 0; i < botsCount; i++) {
            var position = new Vector3(UnityEngine.Random.Range(bounds.min.x, bounds.max.x), UnityEngine.Random.Range(bounds.min.y, bounds.max.y), UnityEngine.Random.Range(bounds.min.z, bounds.max.z));
            var botInstance = Instantiate(BotPrefab, position, Quaternion.identity);
            var botHealth = botInstance.GetComponent<Health>();
            CurrentWaveBots.Add(botInstance);
            onBotSpawned.Invoke(botInstance);
        }
        LastSpawnPoint = bounds.center;
        WaveInProgress = true;
        onWaveStatusUpdated.Invoke(WaveInProgress);
        onWaveSpawnStarted.Invoke(CurrentWave);
    }
}