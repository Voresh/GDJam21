using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AidKitController : MonoBehaviour
{
    public int HealSize = 35;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject != PlayerController.Instance.gameObject)
            return;
        var Health = other.GetComponent<Health>();
        if (Health.HealthCurrent >= Health.HealthMax)
            return;
            
        Health.HealthCurrent += HealSize;
        Destroy(gameObject);
    }
}
