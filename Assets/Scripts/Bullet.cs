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
        if (Time.time - _SpawnTime > Lifetime)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        if (Owner == other.gameObject)
            return;
        if (Owner.GetComponent<Bot>() != null && other.GetComponent<Bot>() != null)
            return; //hack
        var health = other.GetComponent<Health>();
        if (health != null)
            health.HealthCurrent -= Damage;
        Destroy(gameObject);
    }
}