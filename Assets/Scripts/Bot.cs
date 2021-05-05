using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;

public class Bot : MonoBehaviour {
    public BulletSpawner BulletSpawner;
    public Animator Animator;
    public LayerMask EnemyLayer;
    public NavMeshAgent NavMeshAgent;
    public float Speed = 2.5f;
    public float DetectionRadius = 7f;
    public float AttackRadius = 4f;
    private static readonly int _Speed = Animator.StringToHash("Speed");
    public GameObject TargetPlayer;
    public List<Module> TargetModules;
    public Health Health;
    public GameObject MoveToTarget;

    private void Start() {
        TargetPlayer = PlayerController.Instance.gameObject;
        TargetModules = ModulesController.Instance._Modules.ToList();
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
            return;
        }

        var targetColliders = Physics.OverlapSphere(transform.position, AttackRadius, EnemyLayer);

        var target = targetColliders
            .Where(_ =>  _.GetComponent<Bot>() == null)
            .OrderBy(_ => Vector3.Distance(_.transform.position, transform.position))
            .FirstOrDefault();
        Debug.LogError(target);
        if (target != null) {
            transform.LookAt(target.transform);
            BulletSpawner.enabled = true;
            NavMeshAgent.ResetPath();
            Animator.SetFloat("Speed", 0f);
            return;
        }

        //foreach(var i in TargetModules)
        //    Debug.LogError(i);
        var RepairedModules = TargetModules
            .Where(_ => _.GetComponent<Module>().Repaired)
            .ToList();
        //foreach (var i in RepairedModules)
        //    Debug.LogError(i);

        if (RepairedModules != null) {
            MoveToTarget = RepairedModules
           .OrderBy(_ => Vector3.Distance(_.transform.position, transform.position))
           .FirstOrDefault().gameObject;
        }
        else
            MoveToTarget = TargetPlayer;


        var direction = MoveToTarget.transform.position - transform.position;
        if (MoveToTarget.GetComponent<Health>().HealthCurrent > 0 && direction.magnitude < DetectionRadius && direction.magnitude > AttackRadius) {
            NavMeshAgent.speed = Speed;
            NavMeshAgent.Move(direction.normalized * Speed * Time.deltaTime);
            //transform.position += ;
            transform.forward = direction;
            Animator.SetFloat(_Speed, Speed);
        }
        else {
            Animator.SetFloat(_Speed, 0f);
        }
        if (MoveToTarget.GetComponent<Health>().HealthCurrent > 0 && direction.magnitude < AttackRadius) {
            BulletSpawner.enabled = true;
            transform.forward = direction;
        }
        else {
            BulletSpawner.enabled = false;
        }
    }
}
