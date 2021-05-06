using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeaponPickUpController : MonoBehaviour {
    public float WShootRate = 0.5f;
    public Bullet wBulletPrefab;
    public GameObject ModelWeapon;
    public LayerMask WeaponLayer;

    void OnTriggerStay(Collider other) {
        if (other.gameObject != PlayerController.Instance.gameObject)
            return;
        var EquipdWeapon = other.gameObject.GetComponent<PlayerController>().EquipdWeapon;
        //if (EquipdWeapon != null)
        //    Destroy(EquipdWeapon);
        var wBulletSpawn = other.GetComponent<BulletSpawner>();
        wBulletSpawn.BulletPrefab = wBulletPrefab;
        wBulletSpawn.ShootRate = WShootRate;
        var weapon = Instantiate(ModelWeapon, other.transform);
        EquipdWeapon = weapon;
        Destroy(gameObject);
    }
}