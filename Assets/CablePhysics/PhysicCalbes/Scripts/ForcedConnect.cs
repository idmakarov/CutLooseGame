using UnityEngine;
using HPhysic;
using HPlayer;
using HInteractions;

public class ForcedConnect : MonoBehaviour
{
    [SerializeField] private Connector connectorA;
    [SerializeField] private Connector connectorB;
    [SerializeField] private PlayerInteractions playerInteractions;

    [ContextMenu("Force Connect")]
    public void ForceConnectCables()
    {
        if (connectorA == null || connectorB == null)
        {
            Debug.LogError("Connectors are not assigned!");
            return;
        }

        // If one of them is held by the player, drop it first
        if (playerInteractions != null)
        {
            if (playerInteractions.HeldObject != null)
            {
                // Check if the held object is one of the connectors
                if (playerInteractions.HeldObject.TryGetComponent<Connector>(out var heldConnector))
                {
                    if (heldConnector == connectorA || heldConnector == connectorB)
                    {
                        playerInteractions.DropObject(playerInteractions.HeldObject);
                    }
                }
            }
        }

        // Perform the connection
        if (connectorA.CanConnect(connectorB))
        {
            connectorA.Connect(connectorB);
            Debug.Log("Cables connected successfully!");
        }
        else
        {
            Debug.LogWarning("Cannot connect these cables.");
        }
    }

    [ContextMenu("Force Disconnect")]
    public void ForceDisconnectCables()
    {
        if (connectorA == null || connectorB == null)
        {
            Debug.LogError("Connectors are not assigned!");
            return;
        }

        if (connectorA.ConnectedTo == connectorB)
        {
            connectorA.Disconnect();
            Debug.Log("Cables disconnected successfully!");
        }
        else
        {
            Debug.LogWarning("These cables are not connected to each other.");
        }
    }
}
