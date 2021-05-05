using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SecurityBot : MonoBehaviour {
    public List<Transform> Route { get; set; }

    public float StoppingDistance;
    public int TargetPointIndex;
    public NavMeshAgent NavMeshAgent;
    public Animator Animator;

    private void Start() {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        NavMeshAgent.autoBraking = false;
        NavMeshAgent.SetDestination(Route[TargetPointIndex].position);
        Animator.SetFloat("Speed", 1f); //todo: kek
        GetComponent<BulletSpawner>().enabled = false;
    }

    private void Update() {
        if ((Route[TargetPointIndex].position - transform.position).sqrMagnitude > StoppingDistance * StoppingDistance)
            return;
        TargetPointIndex++;
        if (TargetPointIndex == Route.Count)
            TargetPointIndex = 0;
        NavMeshAgent.SetDestination(Route[TargetPointIndex].position);
        Animator.SetFloat("Speed", 1f); //todo: kek
    }
}