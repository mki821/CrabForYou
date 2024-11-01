using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance = null;
    private static object _lockObj = new object();
    protected bool _dontDestroyOnLoad = false;

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

            if(_dontDestroyOnLoad) 
                DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    protected virtual void OnDestroy() {
        if(_instance == this) _instance = null;
    }
}
