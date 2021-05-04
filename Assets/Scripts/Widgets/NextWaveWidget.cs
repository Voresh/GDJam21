using UnityEngine;
using UnityEngine.UI;

public class NextWaveWidget : MonoBehaviour {
    public Text Text;
    
    private void Update() {
        Text.text = $"Next wave in: {Mathf.RoundToInt(BotSpawner.Instance.NextWaveTime)}";
    }
}
