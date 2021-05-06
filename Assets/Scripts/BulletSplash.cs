using UnityEngine;

public class BulletSplash : Bullet {
    public LayerMask EnemyLayer;
    public float Range = 2f;

    protected override void DamageTarget(GameObject target) {
        var targetColliders = Physics.OverlapSphere(transform.position, Range, EnemyLayer);
        foreach (var collider in targetColliders) {
            if (CheckTargetValid(collider.gameObject))
                base.DamageTarget(collider.gameObject);
        }
    }
}
