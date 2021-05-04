using System;
using UnityEngine;
using UnityEngine.UI;

public class PointsWidget : MonoBehaviour {
    public Text Text;
    public float PointsUpdateAnimationDuration = 0.5f;
    
    private float _LastPointsUpdateTime;

    private void Start() {
        Text.text = PointsController.Instance.Points.ToString();
        PointsController.Instance.onPointsUpdated += OnPointsUpdated;
    }

    private void OnPointsUpdated(int points) {
        Text.text = points.ToString();
        _LastPointsUpdateTime = Time.time;
    }

    private void Update() {
        var pastTime = Time.time - _LastPointsUpdateTime;
        if (pastTime < PointsUpdateAnimationDuration) {
            Text.transform.localScale = Vector3.one * Mathf.Lerp(1f, 1.25f, 1 - pastTime / PointsUpdateAnimationDuration);
        }
    }
}
