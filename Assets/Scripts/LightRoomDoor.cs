using DG.Tweening;
using UnityEngine;

public class LightRoomDoor : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform cameraTransform;
    [SerializeField] Renderer doorRenderer;
    Material doorMaterial;

    [SerializeField] float maxAlpha = 0.5f;
    [SerializeField] float reductionStartDistance = 15f;
    [SerializeField] float reductionEndDistance = 2f;
    [SerializeField] Light pointLight;
    [SerializeField] float pointLightMaxIntensity;
    [SerializeField] float pointLightDoIntensityTime;

    void Start()
    {
        doorMaterial = doorRenderer.material;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        float cameraDistance = Vector3.Distance(transform.position, cameraTransform.position);
        if (cameraDistance < distance) distance = cameraDistance;
        
        if (distance > reductionStartDistance) return;

        Color color = doorMaterial.color;

        if (playerTransform.position.z > transform.position.z)
        {
            color.a = 0f;
            doorMaterial.color = color;
            pointLight.DOIntensity(0f, pointLightDoIntensityTime);
            return;
        }
        
        float clampedDistance = Mathf.Clamp(distance - reductionEndDistance, 0f, reductionStartDistance);
        float normalizedDistance = clampedDistance / reductionStartDistance;

        color.a = maxAlpha * normalizedDistance;
        doorMaterial.color = color;
        
        pointLight.intensity = pointLightMaxIntensity * normalizedDistance;
    }
}
