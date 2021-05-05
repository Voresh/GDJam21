using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUpController : MonoBehaviour {
    public float WShootRate = 0.5f;
    public Bullet wBulletPrefab;

    void OnTriggerStay(Collider other) {
        if (other.gameObject != PlayerController.Instance.gameObject)
            return;
        var wBulletSpawn = other.GetComponent<BulletSpawner>();
        wBulletSpawn.BulletPrefab = wBulletPrefab;
        wBulletSpawn.ShootRate = WShootRate;

        Destroy(gameObject);
    }
}