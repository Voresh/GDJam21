using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public class Bot : MonoBehaviour {
    public BulletSpawner BulletSpawner;
    public Animator Animator;
    public LayerMask EnemyLayer;
    public NavMeshAgent NavMeshAgent;
    public float Speed = 2.5f;
    public float DetectionRadius = 7f;
    public float AttackRadius = 4f;
    public float ModuleAttackRadius = 3f;
    private static readonly int _Speed = Animator.StringToHash("Speed");
    public List<Module> TargetModules;
    public GameObject TargetPlayer;
    public GameObject MoveToTarget;
    public Health Health;
    public LayerMask LevelLayer;
    public float RotationDamping;
    private void Start() {
        TargetModules = ModulesController.Instance.Modules.ToList();
        TargetPlayer = PlayerController.Instance.gameObject;
        NavMeshAgent = GetComponent<NavMeshAgent>();
        Health.onDeadStatusUpdated += OnDeadStatusUpdated;
        Health.RestoreHealth();
    }

    private void OnDeadStatusUpdated(bool dead) {
        if (dead) {
            Animator.SetBool("Died", true);
            GetComponent<Collider>().enabled = false;
            NavMeshAgent.enabled = false;
        }
    }

    private void Update() {
        if (Health.Dead) {
            BulletSpawner.enabled = false;
            _Debug = "dead";
            return;
        }
        var targetColliders = Physics.OverlapSphere(transform.position, AttackRadius, EnemyLayer);
        var target = targetColliders
            .Where(_ =>  _.GetComponent<Bot>() == null && AttackAvailable(_.transform))
            .OrderBy(_ => Vector3.Distance(_.transform.position, transform.position))
            .FirstOrDefault();
        if (target != null && Vector3.Distance(target.transform.position, transform.position) < AttackRadius) {
            Attack(target.transform);
            _Debug = "atakin target";
            return;
        }
        var repairedModule = TargetModules
            .Where(_ => _.GetComponent<Module>().Repaired && AttackAvailable(_.transform))
            .OrderBy(_ => Vector3.Distance(_.transform.position, transform.position))
            .FirstOrDefault();
        if (repairedModule != null && Vector3.Distance(repairedModule.transform.position, transform.position) < ModuleAttackRadius) {
            Attack(repairedModule.transform);
            _Debug = "atakin module";
            return;
        }
        BulletSpawner.enabled = false;
        if (target != null) {
            var direction = target.transform.position - transform.position;
            if (direction.magnitude < DetectionRadius) {
                NavMeshAgent.speed = Speed;
                NavMeshAgent.Move(direction.normalized * Speed * Time.deltaTime);
                NavMeshAgent.updateRotation = false;
                transform.forward = Vector3.Lerp(transform.forward, direction, Time.deltaTime * RotationDamping);
                Animator.SetFloat(_Speed, Speed);
                _Debug = "movint to target";
                return;
            }
        }
        repairedModule = TargetModules
            .Where(_ => _.GetComponent<Module>().Repaired)
            .OrderBy(_ => Vector3.Distance(_.transform.position, transform.position))
            .FirstOrDefault();
        if (repairedModule != null)
            MoveToTarget = repairedModule.gameObject;
        else
            MoveToTarget = TargetPlayer;

        NavMeshAgent.speed = Speed;
        NavMeshAgent.SetDestination(MoveToTarget.transform.position);
        NavMeshAgent.updateRotation = true;
        //var rotationDirection = MoveToTarget.transform.position - transform.position;
        //transform.forward = Vector3.Lerp(transform.forward, rotationDirection, Time.deltaTime * RotationDamping);
        Animator.SetFloat(_Speed, Speed);
        _Debug = "movint to module";
    }

    private string _Debug;

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        Handles.Label(transform.position, _Debug);
    }
#endif
    
    private bool AttackAvailable(Transform target) {
        var position = BulletSpawner.ShootPosition.position;
        var direction = Vector3.ProjectOnPlane((target.position - position).normalized, Vector3.up);
        var distance = Mathf.Min((target.position - position).magnitude, AttackRadius);
        var result = !Physics.Raycast(position, direction, distance, LevelLayer);
        //Debug.DrawRay(position, direction * distance,result ?  Color.green : Color.red);
        return result;
    }

    private void Attack(Transform target) {
        var direction = target.transform.position - transform.position;
        NavMeshAgent.updateRotation = false;
        transform.forward = Vector3.Lerp(transform.forward, direction, Time.deltaTime * RotationDamping);
        Debug.DrawRay(transform.position, direction.normalized,Color.green);
        //transform.LookAt(target);
        BulletSpawner.ShootTargetPosition = target.position;
        BulletSpawner.enabled = true;
        NavMeshAgent.ResetPath();
        Animator.SetFloat("Speed", 0f);
    }
}
