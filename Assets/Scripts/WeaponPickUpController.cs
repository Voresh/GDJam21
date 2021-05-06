using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeaponPickUpController : MonoBehaviour {
    public float WShootRate = 0.5f;
    public Bullet wBulletPrefab;
    public GameObject ModelWeapon;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject != PlayerController.Instance.gameObject)
            return;
        if (PlayerController.Instance.EquippedWeapon != null)
            Destroy(PlayerController.Instance.EquippedWeapon);
        var wBulletSpawn = other.GetComponent<BulletSpawner>();
        wBulletSpawn.BulletPrefab = wBulletPrefab;
        wBulletSpawn.ShootRate = WShootRate;
        var weapon = Instantiate(ModelWeapon, PlayerController.Instance.WeaponHand);
        //weapon.transform.localPosition = Vector3.zero;
        //weapon.transform.localRotation = Quaternion.identity;
        PlayerController.Instance.EquippedWeapon = weapon;
        Destroy(gameObject);
    }
}