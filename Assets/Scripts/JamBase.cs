using UnityEngine;

public abstract class JamBase<T> : MonoBehaviour where T : MonoBehaviour {
    public static T Instance { private set; get; }

    protected virtual void Awake() {
        Instance = GetComponent<T>();
    }
}
