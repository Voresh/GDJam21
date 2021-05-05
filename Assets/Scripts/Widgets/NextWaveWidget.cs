using System;
using UnityEngine;
using UnityEngine.UI;

public class NextWaveWidget : MonoBehaviour {
    public Text Text;

    private void Start() {
        BotSpawner.Instance.onWaveStatusUpdated += OnWaveStatusUpdated;
    }

    private void OnWaveStatusUpdated(bool active) {
        Text.gameObject.SetActive(!active);
    }

    private void Update() {
        Text.text = $"Next wave in: {Mathf.RoundToInt(BotSpawner.Instance.NextWaveTime)}";
    }
}
