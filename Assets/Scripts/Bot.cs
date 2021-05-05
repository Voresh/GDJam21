﻿using System;
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
    private static readonly int _Speed = Animator.StringToHash("Speed");
    public List<Module> TargetModules;
    public Health Health;
    public LayerMask LevelLayer;
    
    private void Start() {
        TargetModules = ModulesController.Instance.Modules.ToList();
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
        if (repairedModule != null && Vector3.Distance(repairedModule.transform.position, transform.position) < AttackRadius) {
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
                transform.forward = direction;
                Animator.SetFloat(_Speed, Speed);
                _Debug = "movint to target";
                return;
            }
        }
        repairedModule = TargetModules
            .Where(_ => _.GetComponent<Module>().Repaired)
            .OrderBy(_ => Vector3.Distance(_.transform.position, transform.position))
            .FirstOrDefault();
        if (repairedModule != null) {
            NavMeshAgent.speed = Speed;
            NavMeshAgent.SetDestination(repairedModule.transform.position);
            Animator.SetFloat(_Speed, Speed);
            _Debug = "movint to module";
            return;
        }
        _Debug = "idle";
        Animator.SetFloat(_Speed, 0f);
    }

    private string _Debug;

    private void OnDrawGizmos() {
        Handles.Label(transform.position, _Debug);
    }

    private bool AttackAvailable(Transform _) {
        var position = transform.position + Vector3.up;
        var direction = Vector3.ProjectOnPlane((_.position - transform.position).normalized, Vector3.up);
        var distance = Mathf.Min((_.position - transform.position).magnitude, AttackRadius);
        Debug.DrawRay(position, direction * distance, Color.red);
        return !Physics.Raycast(position, direction, distance, LevelLayer);
    }

    private void Attack(Transform target) {
        transform.LookAt(target);
        BulletSpawner.enabled = true;
        NavMeshAgent.ResetPath();
        Animator.SetFloat("Speed", 0f);
    }
}
