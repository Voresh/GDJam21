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

    private bool _Dead;
    
    private void Start() {
        Target = PlayerController.Instance.gameObject;
        Health.onHealthUpdated += OnHealthUpdated;
    }

    private void OnHealthUpdated(int health) {
        if (_Dead)
            return;
        if (health > 0)
            return;
        Animator.SetTrigger("Died");
        _Dead = true;
    }

    private void Update() {
        if (Health.HealthCurrent <= 0)
            return;
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
