using UnityEngine;
using UnityEngine.Serialization;

public class BulletSpawner : MonoBehaviour {
    public float ShootRate = 0.5f;
    [FormerlySerializedAs("Bullet")]
    public Bullet BulletPrefab;
    public Transform ShootPosition;
    public Transform Shooter;

    private float _LastShotTime;
    
    private void Update() {
        if (Time.time - _LastShotTime < ShootRate)
            return;
        var bulletInstance = Instantiate(BulletPrefab);
        bulletInstance.transform.position = ShootPosition.position;
        bulletInstance.transform.forward = Shooter.forward;
        bulletInstance.Owner = Shooter.gameObject;
        _LastShotTime = Time.time;
    }
}
