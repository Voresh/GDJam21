using UnityEngine;

public class ShieldPickup : MonoBehaviour {
    public Shield ShieldPrefab;
    
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject != PlayerController.Instance.gameObject)
            return;
        var shield = Instantiate(ShieldPrefab);
        shield.Attachement = PlayerController.Instance.gameObject;
        shield.transform.localPosition = Vector3.zero;
        Destroy(gameObject);
    }
}
