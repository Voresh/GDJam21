using System;
using UnityEngine;
using UnityEngine.UI;

public class NextWaveWidget : MonoBehaviour {
    public Text Text;

    
    
    private void Start() {
        BotSpawner.Instance.onWaveStatusUpdated += OnWaveStatusUpdated;
    }

    private void OnWaveStatusUpdated(bool active) {
        // Text.gameObject.SetActive(!active);
    }

    private void Update() {
        Text.text = BotSpawner.Instance.WaveInProgress
            ? $"Wave {BotSpawner.Instance.CurrentWave + 1} in progress" 
            : $"Wave {BotSpawner.Instance.CurrentWave + 2} in: <b>{Mathf.RoundToInt(BotSpawner.Instance.NextWaveTime)}</b>";
    }
}
