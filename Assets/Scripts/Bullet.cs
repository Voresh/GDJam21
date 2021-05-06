﻿using System;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public LayerMask Layers;
    public float Speed = 10f;
    public float Lifetime = 10f;
    public GameObject Owner;
    private float _SpawnTime;
    public int StartDamage = 1;
    public int Damage => StartDamage + Mathf.RoundToInt(StartDamage * GlobalDamageBuff);
    public float GlobalDamageBuff { get; set; }
    
    private void Start() {
        _SpawnTime = Time.time;
    }

    private void Update() {
        var resultSpeed = Speed * Time.deltaTime;
        if (Physics.Raycast(transform.position, transform.forward, out var hit, resultSpeed * 1.25f, Layers, QueryTriggerInteraction.Collide)) {
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
        transform.position += transform.forward * resultSpeed;
        if (Time.time - _SpawnTime > Lifetime)
            Destroy(gameObject);
    }
}