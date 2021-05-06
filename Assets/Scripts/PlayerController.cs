using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class PlayerController : JamBase<PlayerController> {
    public Animator Animator;
    public NavMeshAgent NavMeshAgent;
    public float Speed = 4f;
    public Health Health;
    public BulletSpawner BulletSpawner;
    public Transform WeaponHand;
    public GameObject EquippedWeapon;
    private Transform _Transform;
    private static readonly int _Speed = Animator.StringToHash("Speed");
    public Vector3 Position => _Transform.position;

    public float AttackRadius = 7f;
    public LayerMask EnemyLayer;

    public float SpeedBuff;

    public float ResultSpeed => Speed + SpeedBuff;

    public Vector3 TargetDirection;

    public LayerMask LevelLayer;

    public Collider NearbyTarget;
    protected override void Awake() {
        base.Awake();
        Health.onDeadStatusUpdated += OnDeadStatusUpdated;
    }

    private void Start() {
        _Transform = transform;
        NavMeshAgent = GetComponent<NavMeshAgent>();
        NavMeshAgent.avoidancePriority = 0;
        NavMeshAgent.updateRotation = false;
        TargetDirection = transform.forward;
        Health.RestoreHealth();
    }

    private void OnDeadStatusUpdated(bool dead) {
        if (dead) {
            Animator.SetBool("Died", true);
            BulletSpawner.enabled = false;
            GetComponent<Collider>().enabled = false;
        }
        else {
            Animator.SetBool("Died", false);
            GetComponent<Collider>().enabled = true;
        }
    }

    private bool AttackAvailable(Transform target) {
        var position = BulletSpawner.ShootPosition.position;
        var direction = Vector3.ProjectOnPlane((target.position - position).normalized, Vector3.up);
        var distance = Mathf.Min((target.position - position).magnitude, AttackRadius);
        Debug.DrawRay(position, direction * distance, Color.red);
        return !Physics.Raycast(position, direction, distance, LevelLayer);
    }

    private void Update() {
        if (Health.Dead)
            return;
        var direction = Vector3.zero;

        Collider[] TargetColliders = Physics.OverlapSphere(transform.position, AttackRadius, EnemyLayer);
        if (TargetColliders.Length != 0)
        {
            var targets = TargetColliders
                .Where(t => t.gameObject != gameObject 
                    && t.GetComponent<SecurityBot>() == null 
                    && AttackAvailable(t.transform))
                .OrderBy(t => Vector3.Distance(t.transform.position, transform.position));
            NearbyTarget = targets.FirstOrDefault();

            if (NearbyTarget == null) {
                BulletSpawner.enabled = false;
            }
            else
            {
                // transform.LookAt(NearbyTarget.transform);
                TargetDirection = (NearbyTarget.transform.position - transform.position).normalized;
                BulletSpawner.ShootTargetPosition = NearbyTarget.transform.position;
                BulletSpawner.enabled = true;
            }
        }

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
            NavMeshAgent.speed = ResultSpeed;
            NavMeshAgent.Move(direction.normalized * ResultSpeed * Time.deltaTime);
            //_Transform.position += ;
            if (NearbyTarget == null)
                TargetDirection = direction;
                // _Transform.forward = direction;
            Animator.SetFloat(_Speed, ResultSpeed);
        }
        else {
            Animator.SetFloat(_Speed, 0f);
        }
        transform.forward = Vector3.Slerp(transform.forward, TargetDirection, Time.deltaTime * 24f);
    }
}
