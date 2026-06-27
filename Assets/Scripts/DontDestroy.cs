using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    static DontDestroy firstLoadedScript;

    void Awake()
    {
        if (firstLoadedScript != null)
        {
            if (firstLoadedScript.gameObject != null)
            {
                Destroy(gameObject);
                return;
            }
        }

        firstLoadedScript = this;
        DontDestroyOnLoad(gameObject);
    }
}
