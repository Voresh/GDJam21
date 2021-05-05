using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnnouncementController : JamBase<AnnouncementController> {
    public Text Text;
    public CanvasGroup Group;
    public float AnnouncementDuration = 1.5f;
    public AnimationCurve AlphaCurve;
    
    private Queue<string> _Scheduled = new Queue<string>();
    private bool _InProgress;
    private string _CurrentText;
    private float _LastAnnouncementTime;

    private void Start() {
        Text.text = string.Empty;
        Group.alpha = 0f;
    }

    public void Schedule(string text) {
        _Scheduled.Enqueue(text);
    }

    private void Update() {
        var timePast = Time.time - _LastAnnouncementTime;
        if (timePast < AnnouncementDuration) {
            if (_InProgress) {
                Text.text = _CurrentText;
                Group.alpha = AlphaCurve.Evaluate(timePast / AnnouncementDuration);
            }
        }
        else {
            if (_Scheduled.Count > 0) {
                _CurrentText = _Scheduled.Dequeue();
                _InProgress = true;
                _LastAnnouncementTime = Time.time;
            }
            else {
                _InProgress = false;
            }
        }
    }
}
