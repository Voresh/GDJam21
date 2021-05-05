using System;
using UnityEngine;

public class Sensor : MonoBehaviour {
    public event Action<Collider> onTriggerEnter = _ => { };
    
    private void OnTriggerEnter(Collider other) {
        onTriggerEnter.Invoke(other);
    }
}
