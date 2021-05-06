﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SecurityBot : MonoBehaviour {
    public LayerMask EnemyLayer;
    public LayerMask LevelLayer;
    public float AttackRadius = 5f;
    public Health Health;
    public List<SecurityRoute> Route { get; set; }
    public List<SecurityRoute> VisitedRoutePoints { get; set; } = new List<SecurityRoute>();
    public float StoppingDistance;
    [NonSerialized]
    public SecurityRoute SecurityRoute;
    public NavMeshAgent NavMeshAgent;
    public Animator Animator;
    public BulletSpawner BulletSpawner;

    private bool _WasShooting;
    
    private void Start() {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        BulletSpawner = GetComponent<BulletSpawner>();
        Health = GetComponent<Health>();
        Health.RestoreHealth();
        NavMeshAgent.autoBraking = false;
        Animator.SetFloat("Speed", 1f);
        GetComponent<BulletSpawner>().enabled = false;
        SecurityRoute = null;
        UpdateDestination();
        Health.onDeadStatusUpdated += OnDeadStatusUpdated;
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
            .Where(_ => _.gameObject != gameObject 
                && _.gameObject != PlayerController.Instance.gameObject 
                && _.GetComponent<SecurityBot>() == null
                && AttackAvailable(_.transform))
            .OrderBy(_ => Vector3.Distance(_.transform.position, transform.position))
            .FirstOrDefault();
        if (target != null) { //todo: check raycast too
            transform.LookAt(target.transform);
            BulletSpawner.ShootTargetPosition = target.transform.position;
            BulletSpawner.enabled = true;
            NavMeshAgent.ResetPath();
            Animator.SetFloat("Speed", 0f);
            _WasShooting = true;
            return;
        }
        
        if (_WasShooting) {
            BulletSpawner.enabled = false;
            UpdateDestination();
            Animator.SetFloat("Speed", 1f);
            _WasShooting = false;
        }
        UpdateDestination();
    }

    private void UpdateDestination() {
        if (SecurityRoute == null) {
            SecurityRoute = Route
                .OrderBy(_ => (_.Transform.position - transform.position).sqrMagnitude)
                .FirstOrDefault(AcceptableRoute);
            if (SecurityRoute == null) {
                VisitedRoutePoints.Clear();
                SecurityRoute = Route
                    .OrderBy(_ => (_.Transform.position - transform.position).sqrMagnitude)
                    .First(AcceptableRoute);
            }
        }
        if ((SecurityRoute.Transform.position - transform.position).sqrMagnitude > StoppingDistance * StoppingDistance) {
            NavMeshAgent.SetDestination(SecurityRoute.Transform.position);
        }
        else {
            VisitedRoutePoints.Add(SecurityRoute);
            SecurityRoute = null;
            UpdateDestination();
        }
    }

    private bool AcceptableRoute(SecurityRoute route) {
        return route.Module.Unlocked && !VisitedRoutePoints.Contains(route);
    }

    private bool AttackAvailable(Transform target) {
        var position = BulletSpawner.ShootPosition.position;
        var direction = Vector3.ProjectOnPlane((target.position - position).normalized, Vector3.up);
        var distance = Mathf.Min((target.position - position).magnitude, AttackRadius);
        Debug.DrawRay(position, direction * distance, Color.red);
        return !Physics.Raycast(position, direction, distance, LevelLayer);
    }
}