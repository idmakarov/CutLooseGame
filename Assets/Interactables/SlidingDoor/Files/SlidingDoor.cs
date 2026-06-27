using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class SlidingDoor : MonoBehaviour
{
    [SerializeField] string boxTag = "MovableBox";
    public bool open;

    [SerializeField] Renderer doorRenderer;
    
    [Header("Movement Settings")]
    [SerializeField] Axis movementAxis = Axis.Y;
    [SerializeField] float slideDistance = 1f;
    [SerializeField] float animationDuration = 0.3f;
    [SerializeField] Ease openEase = Ease.OutBack;
    [SerializeField] Ease closeEase = Ease.InBack;

    Vector3 normalLocalPosition;
    Vector3 openLocalPosition;

    public enum Axis
    {
        X,
        Y,
        Z
    }

    void Start()
    {
        normalLocalPosition = doorRenderer.transform.localPosition;
        CalculateOpenPosition();

        if (open)
        {
            open = false;
            Open();
        }
    }

    void CalculateOpenPosition()
    {
        openLocalPosition = normalLocalPosition;
        
        switch (movementAxis)
        {
            case Axis.X:
                openLocalPosition.x += slideDistance;
                break;
            case Axis.Y:
                openLocalPosition.y += slideDistance;
                break;
            case Axis.Z:
                openLocalPosition.z += slideDistance;
                break;
        }
    }

    public void Toggle()
    {
        if (open)
            Close();
        else
            Open();
    }

    public void Open()
    {
        if (open) return;
        
        doorRenderer.transform.DOKill();
        doorRenderer.transform.DOLocalMove(openLocalPosition, animationDuration)
            .SetEase(openEase);
        open = true;
    }

    public void Close()
    {
        if (!open) return;
        
        doorRenderer.transform.DOKill();
        doorRenderer.transform.DOLocalMove(normalLocalPosition, animationDuration)
            .SetEase(closeEase);
        open = false;
    }

    void OnValidate()
    {
        if (doorRenderer != null && Application.isPlaying)
        {
            CalculateOpenPosition();
        }
    }

    void OnDrawGizmosSelected()
    {
        if (doorRenderer == null) return;

        // Calculate positions if we haven't started yet
        Vector3 closedPos = doorRenderer.transform.localPosition;
        Vector3 openedPos = closedPos;
        
        switch (movementAxis)
        {
            case Axis.X:
                openedPos.x += slideDistance;
                break;
            case Axis.Y:
                openedPos.y += slideDistance;
                break;
            case Axis.Z:
                openedPos.z += slideDistance;
                break;
        }

#if UNITY_EDITOR
        // Convert local positions to world space for drawing
        Vector3 closedWorldPos = transform.TransformPoint(closedPos);
        Vector3 openedWorldPos = transform.TransformPoint(openedPos);

        // Get the door bounds for visualization
        Bounds bounds = doorRenderer.bounds;
        Vector3 doorSize = bounds.size;

        // Draw closed position
        //Gizmos.color = new Color(0, 1, 0, 0.3f); // Green, semi-transparent
        //DrawDoorGizmo(closedWorldPos, doorSize);
        
        // Draw open position
        Gizmos.color = new Color(0, 0.5f, 1, 0.3f); // Blue, semi-transparent
        DrawDoorGizmo(openedWorldPos, doorSize);

        // Draw movement arrow
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(closedWorldPos, openedWorldPos);
        
        // Draw arrowhead
        Vector3 direction = (openedWorldPos - closedWorldPos).normalized;
        Vector3 arrowTip = openedWorldPos;
        Vector3 arrowRight = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + 20, 0) * Vector3.forward * 0.1f;
        Vector3 arrowLeft = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - 20, 0) * Vector3.forward * 0.1f;
        
        Gizmos.DrawLine(arrowTip, arrowTip + arrowRight);
        Gizmos.DrawLine(arrowTip, arrowTip + arrowLeft);

        // Draw spheres at start and end points
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(closedWorldPos, 0.05f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(openedWorldPos, 0.05f);

        // Draw labels in scene view
        UnityEditor.Handles.BeginGUI();
        UnityEditor.Handles.Label(closedWorldPos + Vector3.up * 0.1f, "Closed");
        UnityEditor.Handles.Label(openedWorldPos + Vector3.up * 0.1f, "Open");
        UnityEditor.Handles.EndGUI();
#endif
    }

    void DrawDoorGizmo(Vector3 position, Vector3 size)
    {
        // Draw wireframe cube representing the door
        Gizmos.DrawWireCube(position, size);
        
        // Draw semi-transparent cube
        Gizmos.DrawCube(position, size);
    }
}