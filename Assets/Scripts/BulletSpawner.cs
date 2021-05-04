using UnityEngine;

public class BulletSpawner : MonoBehaviour {
    public float ShootRate = 0.5f;
    public Bullet Bullet;
    public Transform ShootPosition;
    public Transform Shooter;

    private float _LastShotTime;
    
    private void Update() {
        if (Time.time - _LastShotTime < ShootRate)
            return;
        var bulletInstance = Instantiate(Bullet);
        bulletInstance.transform.position = ShootPosition.position;
        bulletInstance.transform.forward = Shooter.forward;
        bulletInstance.Owner = Shooter.gameObject;
        _LastShotTime = Time.time;
    }
}
