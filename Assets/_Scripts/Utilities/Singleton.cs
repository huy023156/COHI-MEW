using UnityEngine;

public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    protected virtual void Awake() => Instance = this as T;

    private void OnApplicationQuit()
    {
        Instance = null;
        Destroy(gameObject);
    }
}

public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one " + nameof(T) + "! " + transform + " - " + name);
            Destroy(gameObject);
            return;
        }
        base.Awake();
    }
}

// Will survive through scene loads
public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();

        Transform rootTransform = transform;
        // Find root transform if gameobject is not a root object
        while (rootTransform.parent != null)
        {
            rootTransform = rootTransform.parent;
        }
        DontDestroyOnLoad(rootTransform.gameObject);
    }
}
