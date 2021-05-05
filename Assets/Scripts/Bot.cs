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
        }
    }

    private void Update() {
        if (Health.Dead) {
            BulletSpawner.enabled = false;
            return;
        }

        var targetColliders = Physics.OverlapSphere(transform.position, AttackRadius, EnemyLayer);
        var target = targetColliders
            .Where(_ => _.gameObject != gameObject)
            .OrderBy(_ => Vector3.Distance(_.transform.position, transform.position))
            .FirstOrDefault();
        if (target != null) {
            transform.LookAt(target.transform);
            BulletSpawner.enabled = true;
            NavMeshAgent.ResetPath();
            Animator.SetFloat("Speed", 0f);
            return;
        }

        Debug.LogError(TargetModules.ToList());
        var RepairedModules = TargetModules
            .Where(_ => _.GetComponent<Module>().Repaired)
            .ToList();
        Debug.LogError(RepairedModules);
        var NearbyTargetModules = TargetModules
            .OrderBy(_ => Vector3.Distance(_.transform.position, transform.position))
            .FirstOrDefault();


        var direction = TargetPlayer.transform.position - transform.position;
        if (TargetPlayer.GetComponent<Health>().HealthCurrent > 0 && direction.magnitude < DetectionRadius && direction.magnitude > AttackRadius) {
            NavMeshAgent.speed = Speed;
            NavMeshAgent.Move(direction.normalized * Speed * Time.deltaTime);
            //transform.position += ;
            transform.forward = direction;
            Animator.SetFloat(_Speed, Speed);
        }
        else {
            Animator.SetFloat(_Speed, 0f);
        }
        if (TargetPlayer.GetComponent<Health>().HealthCurrent > 0 && direction.magnitude < AttackRadius) {
            BulletSpawner.enabled = true;
            transform.forward = direction;
        }
        else {
            BulletSpawner.enabled = false;
        }
    }
}
