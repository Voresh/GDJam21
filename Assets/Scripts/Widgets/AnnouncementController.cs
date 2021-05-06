using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnnouncementController : JamBase<AnnouncementController> {
    public Text Text;
    public Text AdditionalText;
    public CanvasGroup Group;
    public float AnnouncementDuration = 1f;
    public AnimationCurve AlphaCurve;
    
    private Queue<(string, string)> _Scheduled = new Queue<(string, string)>();
    private bool _InProgress;
    private string _CurrentText;
    private string _CurrentAdditionalText;
    private float _LastAnnouncementTime;

    protected override void Awake() {
        base.Awake();
        Text.text = string.Empty;
        AdditionalText.text = string.Empty;
        Group.alpha = 0f;
    }

    public void Schedule(string text, string additionalText = null) {
        _Scheduled.Enqueue((text, additionalText));
    }

    private void Update() {
        var timePast = Time.time - _LastAnnouncementTime;
        if (timePast < AnnouncementDuration) {
            if (_InProgress) {
                Text.text = _CurrentText;
                AdditionalText.text = _CurrentAdditionalText;
                Group.alpha = AlphaCurve.Evaluate(timePast / AnnouncementDuration);
            }
            else {
                Group.alpha = 0f;
            }
        }
        else {
            Group.alpha = 0f;
            if (_Scheduled.Count > 0) {
                var (text, additionalText) = _Scheduled.Dequeue();
                _CurrentText = text;
                _CurrentAdditionalText = additionalText;
                _InProgress = true;
                _LastAnnouncementTime = Time.time;
            }
            else {
                _InProgress = false;
            }
        }
    }
}
