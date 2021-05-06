using UnityEngine;
using UnityEngine.Serialization;

public class BulletSpawner : MonoBehaviour {
    public float ShootRate = 0.15f;
    [FormerlySerializedAs("Bullet")]
    public Bullet BulletPrefab;
    public Transform ShootPosition;
    public Transform Shooter;

    private float _LastShotTime;
    public float GlobalDamageBuff;
    public Vector3 ShootTargetPosition;

    private void Update() {
        if (Time.time - _LastShotTime < ShootRate)
            return;
        var bulletInstance = Instantiate(BulletPrefab);
        bulletInstance.transform.position = ShootPosition.position;
        bulletInstance.transform.forward = ShootTargetPosition - transform.position;
        bulletInstance.Owner = Shooter.gameObject;
        bulletInstance.GlobalDamageBuff = GlobalDamageBuff;
        _LastShotTime = Time.time;
    }
}
