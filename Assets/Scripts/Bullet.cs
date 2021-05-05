using System;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float Speed = 10f;
    public float Lifetime = 10f;
    public GameObject Owner;
    private float _SpawnTime;
    public int Damage = 7;
    
    private void Start() {
        _SpawnTime = Time.time;
    }

    private void Update() {
        transform.position += transform.forward * Speed * Time.deltaTime;
        if (Physics.Raycast(transform.position, transform.forward, out var hit, 0.25f)) {
            var target = hit.transform.gameObject;
            if (Owner == target.gameObject)
                return;
            if (Owner.GetComponent<Bot>() != null && target.GetComponent<Bot>() != null)
                return; //hack
            if (Owner.GetComponent<SecurityBot>() != null && target.GetComponent<SecurityBot>() != null 
                || Owner.GetComponent<SecurityBot>() != null && target.gameObject == PlayerController.Instance.gameObject)
                return; //hack 2
            if (Owner == PlayerController.Instance.gameObject && target.GetComponent<Module>() != null)
                return; //hack 3
            var health = target.GetComponent<Health>();
            if (health != null)
                health.HealthCurrent -= Damage;
            Destroy(gameObject);
        }
        if (Time.time - _SpawnTime > Lifetime)
            Destroy(gameObject);
    }
}