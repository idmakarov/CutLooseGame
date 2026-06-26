using HPhysic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CableController : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    private InputAction _extendAction, _retractAction, _connectAction;
    [SerializeField] int minSegmentCount, maxSegmentCount;

    [SerializeField] PhysicCable physicCable;
    [SerializeField] ForcedConnect forcedConnectMain;
    [SerializeField] ForcedConnect forcedConnectOptional;

    bool cableConnected;
    float lastRopeTime;
    [SerializeField] float ropeChangeDelay;

    private void Start()
    {
        physicCable.transform.SetParent(null, true);

        if (_playerInput == null)
        {
            Debug.LogWarning("You should assign PlayerInput! Using .Find() for now.");
            _playerInput = GameObject.Find("PlayerInput").GetComponent<PlayerInput>();
        }

        _extendAction = _playerInput.actions.FindAction("CableExtend");
        _retractAction = _playerInput.actions.FindAction("CableRetract");
        _connectAction = _playerInput.actions.FindAction("CableConnect");

        forcedConnectMain.ForceConnectCables();
    }

    private void Update()
    {
        if (_retractAction.IsPressed())
        {
            if (Time.time - lastRopeTime > ropeChangeDelay)
            {
                lastRopeTime = Time.time;
                if (physicCable.points.Count > minSegmentCount)
                    physicCable.RemovePoint();
            }
        }
        else if (_extendAction.IsPressed())
        {
            if (Time.time - lastRopeTime > ropeChangeDelay)
            {
                lastRopeTime = Time.time;
                if (physicCable.points.Count < maxSegmentCount)
                    physicCable.AddPoint();
            }
        }

        if (_connectAction.WasPressedThisFrame())
        {
            if (cableConnected)
            {
                forcedConnectOptional.ForceDisconnectCables();
                cableConnected = false;        
            }
            else
            {
                forcedConnectOptional.ForceConnectCables();
                cableConnected = true;
            }
        }
    }
}
