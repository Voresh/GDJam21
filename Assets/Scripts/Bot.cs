using System;
using UnityEngine;

public class Bot : MonoBehaviour {
    public BulletSpawner BulletSpawner;
    public Animator Animator;
    public float Speed = 2.5f;
    public float DetectionRadius = 7f;
    public float AttackRadius = 4f;
    private static readonly int _Speed = Animator.StringToHash("Speed");
    public GameObject Target;
    public Health Health;

    private void Start() {
        Target = PlayerController.Instance.gameObject;
        Health.onDeadStatusUpdated += OnDeadStatusUpdated;
    }

    private void OnDeadStatusUpdated(bool dead) {
        if (dead) {
            Animator.SetTrigger("Died");
            GetComponent<Collider>().enabled = false;
        }
    }

    private void Update() {
        if (Health.Dead) {
            BulletSpawner.enabled = false;
            return;
        }
        var direction = Target.transform.position - transform.position;
        if (Target.GetComponent<Health>().HealthCurrent > 0 && direction.magnitude < DetectionRadius && direction.magnitude > AttackRadius) {
            transform.position += direction.normalized * Speed * Time.deltaTime;
            transform.forward = direction;
            Animator.SetFloat(_Speed, Speed);
        }
        else {
            Animator.SetFloat(_Speed, 0f);
        }
        if (Target.GetComponent<Health>().HealthCurrent > 0 && direction.magnitude < AttackRadius) {
            BulletSpawner.enabled = true;
            transform.forward = direction;
        }
        else {
            BulletSpawner.enabled = false;
        }
    }
}
