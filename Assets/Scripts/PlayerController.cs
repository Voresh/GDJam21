using System;
using UnityEngine;

public class PlayerController : JamBase<PlayerController> {
    public Animator Animator;
    public float Speed = 4f;
    public Health Health;
    public BulletSpawner BulletSpawner;
    private Transform _Transform;
    private static readonly int _Speed = Animator.StringToHash("Speed");
    public Vector3 Position => _Transform.position;

    public event Action onDied = () => { };
    
    protected override void Awake() {
        base.Awake();
        _Transform = transform;
        Health.onDeadStatusUpdated += OnDeadStatusUpdated;
    }

    private void OnDeadStatusUpdated(bool dead) {
        if (dead) {
            Animator.SetTrigger("Died");
            BulletSpawner.enabled = false;
            GetComponent<Collider>().enabled = false;
            onDied.Invoke();
        }
        else {
            //todo: resp
        }
    }

    private void Update() {
        if (Health.Dead)
            return;
        var direction = Vector3.zero;

        if (JoystickWidget.Instance.HasTouch) {
            var joystickInput = JoystickWidget.Instance.Value;
            direction = new Vector3(joystickInput.x, 0f, joystickInput.y);
        }

        if (Input.GetKey(KeyCode.W))
            direction += Vector3.forward;
        if (Input.GetKey(KeyCode.A))
            direction += Vector3.left;
        if (Input.GetKey(KeyCode.S))
            direction += Vector3.back;
        if (Input.GetKey(KeyCode.D))
            direction += Vector3.right;
        if (direction != Vector3.zero) {
            _Transform.position += direction.normalized * Speed * Time.deltaTime;
            _Transform.forward = direction;
            Animator.SetFloat(_Speed, Speed);
        }
        else {
            Animator.SetFloat(_Speed, 0f);
        }
    }
}
