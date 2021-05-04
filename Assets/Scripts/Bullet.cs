using System;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float Speed = 10f;
    public float Lifetime = 10f;
    public GameObject Owner;
    private float _SpawnTime;

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
        Destroy(gameObject);
    }
}