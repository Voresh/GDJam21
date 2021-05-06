using UnityEngine;

public class BulletSplash : Bullet {
    public LayerMask EnemyLayer;
    public float Range = 2f;
    public float SplashDamageMultiplier = 0.5f;

    protected override void DamageTarget(GameObject target, float multiplier) {
        var targetColliders = Physics.OverlapSphere(transform.position, Range, EnemyLayer);
        foreach (var collider in targetColliders) {
            if (CheckTargetValid(collider.gameObject))
                base.DamageTarget(collider.gameObject, collider.gameObject == target ? multiplier : SplashDamageMultiplier);
        }
    }
}
