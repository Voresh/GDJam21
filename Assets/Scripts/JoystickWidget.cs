using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class JoystickWidget : JamBase<JoystickWidget>, IPointerDownHandler, IPointerUpHandler, IDragHandler {
    public float _MaxOffsetRadius = 100;
    public RectTransform _HandleRoot;
    public RectTransform _JoystickContainer;
    public bool _MoveContainer;
    public bool _DisableJoystickWhenInactive;
    public Canvas _Canvas;

    public Vector2 Value { get; private set; }
    public bool HasTouch { get; private set; }

    public Vector3 StartJoystickPosition { get; set; }

    private float MaxOffsetRadius => _MaxOffsetRadius * _Canvas.scaleFactor;
    private Vector3 JoystickWorldCenter {
        get => _JoystickContainer.position;
        set {
            if (_MoveContainer)
                _JoystickContainer.position = value;
        }
    }

    private int _PointerId;

    private int CurrentPointerTouchIndex {
        get {
            for (var i = 0; i < Input.touchCount; i++) {
                var touch = Input.GetTouch(i);
                if (touch.fingerId == _PointerId)
                    return i;
            }
            return 0;
        }
    }

    protected override void Awake() {
        base.Awake();
        StartJoystickPosition = JoystickWorldCenter;
        if (_DisableJoystickWhenInactive)
            _JoystickContainer.gameObject.SetActive(false);
    }

    private void OnDisable() {
        ResetHandlePosition();
        HasTouch = false;
        _PointerId = 0;
    }

    public void ResetJoystickPosition() {
        _JoystickContainer.position = StartJoystickPosition;
        _HandleRoot.position = StartJoystickPosition;
        Value = Vector2.zero;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData) {
        if (!HasTouch) {
            if (_DisableJoystickWhenInactive)
                _JoystickContainer.gameObject.SetActive(true);
            HasTouch = true;
            _PointerId = eventData.pointerId;
            JoystickWorldCenter = eventData.position;
            SetHandlePosition(eventData.position);
        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData) {
        if (HasTouch && _PointerId == eventData.pointerId) {
            HasTouch = false;
            JoystickWorldCenter = StartJoystickPosition;
            ResetHandlePosition();
            if (_DisableJoystickWhenInactive)
                _JoystickContainer.gameObject.SetActive(false);
        }
    }

    // TODO: Check drag threshold on devices with low dpi, might need adjustments in InputModule.
    // If drag threshold conflicts with other interfaces, might need custom realization via Update instead of OnDrag

    void IDragHandler.OnDrag(PointerEventData eventData) {
        if (HasTouch && _PointerId == eventData.pointerId)
            SetHandlePosition(eventData.position);
    }

    private void Update() {
#if !UNITY_EDITOR
            if (HasTouch)
                SetHandlePosition(Input.GetTouch(CurrentPointerTouchIndex).position);
#else
        if (HasTouch)
            SetHandlePosition(Input.mousePosition);
#endif
    }

    private void ResetHandlePosition() {
        SetHandlePosition(JoystickWorldCenter);
        Value = Vector2.zero;
    }

    private void SetHandlePosition(Vector2 position) {
        var offset = ((Vector3) position - JoystickWorldCenter) / MaxOffsetRadius;
        if (offset.magnitude > 1)
            offset = offset.normalized;
        Value = offset;
        _HandleRoot.position = JoystickWorldCenter + offset * MaxOffsetRadius;
    }
}
