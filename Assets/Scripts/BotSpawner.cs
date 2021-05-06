﻿using System;
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
    
    public List<int> BotsCountHardcode = new List<int> {
        5, 
        5, 
        10, 
        10, 
        15, 
        15, 
        17,
        17,
        20, 
        20,
        23,
        23,
        25,
        25,
        27,
        27,
        30,
        30,
        30,
        30,
        30,
        30,
        30,
        30,
        30,
        30,
        30,
        30,
        30,
        30,
        30,
        30,
        30,
        30,
        30,
        30,
        30,
        30,
        30,
        30,
        30,
        30,
        30,
        30,
        30,
    };
    
    public int BotsCount => BotsCountHardcode[CurrentWave]; /*Fibonacci(CurrentWave + 1) * 5;*/

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
        var spawnPoints = SpawnPoints
            .Where(_ => _.gameObject.activeSelf)
            .ToList();
        var botsOnPoints = spawnPoints.ToDictionary(_ => _, _ => 0);
        const int desiredMaxOnPoint = 5;
        var spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
        CurrentWaveBots.Clear();
        var botsCount = BotsCount;
        FirstWave = false;
        Debug.Log(botsCount);
        for (var i = 0; i < botsCount; i++) {
            if (botsOnPoints[spawnPoint] == desiredMaxOnPoint) {
                spawnPoints = spawnPoints
                    .Where(_ => botsOnPoints[_] < desiredMaxOnPoint)
                    .ToList();
                if (spawnPoints.Count != 0)
                    spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
            }
            var position = new Vector3(UnityEngine.Random.Range(spawnPoint.bounds.min.x, spawnPoint.bounds.max.x), UnityEngine.Random.Range(spawnPoint.bounds.min.y, spawnPoint.bounds.max.y), UnityEngine.Random.Range(spawnPoint.bounds.min.z, spawnPoint.bounds.max.z));
            var botInstance = Instantiate(BotPrefab, position, Quaternion.identity);
            var botHealth = botInstance.GetComponent<Health>();
            CurrentWaveBots.Add(botInstance);
            botsOnPoints[spawnPoint] = botsOnPoints[spawnPoint] + 1;
            onBotSpawned.Invoke(botInstance);
        }
        LastSpawnPoint = spawnPoint.bounds.center;
        WaveInProgress = true;
        onWaveStatusUpdated.Invoke(WaveInProgress);
        onWaveSpawnStarted.Invoke(CurrentWave);
    }
}