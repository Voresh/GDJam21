using System;
using UnityEngine;

public class Bot : MonoBehaviour {
    public BulletSpawner BulletSpawner;
    public Animator Animator;
    public float DetectionRadius = 7f;
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
        GetComponent<Collider>().enabled = false;
        _Dead = true;
    }

    private void Update() {
        if (_Dead) {
            BulletSpawner.enabled = false;
            return;
        }
        var direction = Target.transform.position - transform.position;
        if (Target.GetComponent<Health>().HealthCurrent > 0 && direction.magnitude < DetectionRadius) {
            BulletSpawner.enabled = true;
            transform.forward = direction;
        }
        else {
            BulletSpawner.enabled = false;
        }
    }
}
