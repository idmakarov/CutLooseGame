using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    void LateUpdate()
    {
        transform.LookAt(GameManager.Instance.mainCameraT);
    }
}
