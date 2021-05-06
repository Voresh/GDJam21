using System;
using UnityEngine;

public class DeathWidgetController : JamBase<DeathWidgetController> {
    public CanvasGroup CanvasGroup;

    private float TargetAlpha = 0f;
    public bool Active {
        set {
            TargetAlpha = value ? 1f : 0f;
            CanvasGroup.blocksRaycasts = value;
        }
    }

    private void Start() {
        Active = false;
        CanvasGroup.alpha = 0f;
    }

    private void Update() {
        CanvasGroup.alpha = Mathf.Lerp(CanvasGroup.alpha, TargetAlpha, Time.time * 10f);
    }
}
