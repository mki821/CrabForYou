using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance = null;
    private static object _lockObj = new object();

    public static T Instance {
        get {
            lock(_lockObj) {
                if(_instance == null) {
                    _instance = FindAnyObjectByType<T>();

                    if(_instance == null) {
                        _instance = new GameObject().AddComponent<T>();
                    }
                }

                return _instance;
            }
        }
    }

    protected virtual void Awake() {
        if(_instance == null) {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
}