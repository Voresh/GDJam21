using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SecurityBot : MonoBehaviour {
    public LayerMask EnemyLayer;
    public LayerMask LevelLayer;
    public float AttackRadius = 5f;
    public List<Transform> Route { get; set; }
    public float StoppingDistance;
    public int TargetPointIndex;
    public NavMeshAgent NavMeshAgent;
    public Animator Animator;
    public BulletSpawner BulletSpawner;

    private bool _WasShooting;
    
    private void Start() {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        BulletSpawner = GetComponent<BulletSpawner>();
        NavMeshAgent.autoBraking = false;
        NavMeshAgent.SetDestination(Route[TargetPointIndex].position);
        Animator.SetFloat("Speed", 1f);
        GetComponent<BulletSpawner>().enabled = false;
    }

    private void Update() {
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
            BulletSpawner.enabled = true;
            NavMeshAgent.ResetPath();
            Animator.SetFloat("Speed", 0f);
            _WasShooting = true;
            return;
        }
        
        if (_WasShooting) {
            BulletSpawner.enabled = false;
            NavMeshAgent.SetDestination(Route[TargetPointIndex].position);
            Animator.SetFloat("Speed", 1f);
            _WasShooting = false;
        }

        if ((Route[TargetPointIndex].position - transform.position).sqrMagnitude > StoppingDistance * StoppingDistance)
            return;
        TargetPointIndex++;
        if (TargetPointIndex == Route.Count)
            TargetPointIndex = 0;
        NavMeshAgent.SetDestination(Route[TargetPointIndex].position);
        Animator.SetFloat("Speed", 1f);
    }

    private bool AttackAvailable(Transform _) {
        var position = transform.position + Vector3.up;
        var direction = Vector3.ProjectOnPlane((_.position - transform.position).normalized, Vector3.up);
        var distance = Mathf.Min((_.position - transform.position).magnitude, AttackRadius);
        Debug.DrawRay(position, direction * distance, Color.red);
        return !Physics.Raycast(position, direction, distance, LevelLayer);
    }
}