using UnityEngine;

public class PlayerController : JamBase<PlayerController> {
    private Transform _Transform;
    public Vector3 Position => _Transform.position;

    protected override void Awake() {
        base.Awake();
        _Transform = transform;
    }
}
