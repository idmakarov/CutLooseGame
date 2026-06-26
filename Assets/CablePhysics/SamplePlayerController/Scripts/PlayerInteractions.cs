using UnityEngine.InputSystem;
using UnityEngine;
using HInteractions;
using System;

namespace HPlayer
{
    public class PlayerInteractions : MonoBehaviour, IObjectHolder
    {
        [Header("Select")]
        [SerializeField] private Transform playerCamera;
        [SerializeField] private float selectRange = 10f;
        [SerializeField] private LayerMask selectLayer;
        [field: SerializeField] public Interactable SelectedObject { get; private set; }

        [Header("Hold")]
        [SerializeField] private Transform handTransform;
        [SerializeField, Min(1)] private float holdingForce = 0.5f;
        [SerializeField] private int heldObjectLayer;
        [SerializeField, Range(0f, 90f)] private float heldClamXRotation = 45f;
        [field: SerializeField] public Liftable HeldObject { get; private set; }

        [field: Header("Input")]
        [field: SerializeField] public bool Interacting { get; private set; }

        public event Action OnSelect;
        public event Action OnDeselect;

        public event Action OnInteractionStart;
        public event Action OnInteractionEnd;

        private InputAction interactAction;

        private void OnEnable()
        {
            OnInteractionStart += ChangeHeldObject;
            PlayerController.OnPlayerEnterPortal += CheckHeldObjectOnTeleport;

            RegisterInputs();
        }

        private void OnDisable()
        {
            OnInteractionStart -= ChangeHeldObject;
            PlayerController.OnPlayerEnterPortal -= CheckHeldObjectOnTeleport;

            interactAction?.Disable();
            interactAction?.Dispose();
        }

        private void RegisterInputs()
        {
            var map = new InputActionMap("Player Interactions");

            interactAction = map.AddAction("Interact");
            interactAction.AddBinding("<Mouse>/leftButton");
            interactAction.AddBinding("<Gamepad>/xButton");

            interactAction.Enable();
        }

        private void Update()
        {
            UpdateInput();
            UpdateSelectedObject();

            if (HeldObject)
                UpdateHeldObjectPosition();
        }

        #region Input

        private void UpdateInput()
        {
            bool interacting = interactAction?.IsPressed() ?? false;

            if (interacting == Interacting)
                return;

            Interacting = interacting;

            if (interacting)
                OnInteractionStart?.Invoke();
            else
                OnInteractionEnd?.Invoke();
        }

        #endregion

        #region Selected Object

        private void UpdateSelectedObject()
        {
            Interactable foundInteractable = null;

            if (Physics.SphereCast(
                    playerCamera.position,
                    0.2f,
                    playerCamera.forward,
                    out RaycastHit hit,
                    selectRange,
                    selectLayer))
            {
                foundInteractable = hit.collider.GetComponent<Interactable>();
            }

            if (SelectedObject == foundInteractable)
                return;

            if (SelectedObject)
            {
                SelectedObject.Deselect();
                OnDeselect?.Invoke();
            }

            SelectedObject = foundInteractable;

            if (foundInteractable && foundInteractable.enabled)
            {
                foundInteractable.Select();
                OnSelect?.Invoke();
            }
        }

        #endregion

        #region Held Object

        private void UpdateHeldObjectPosition()
        {
            HeldObject.Rigidbody.linearVelocity =
                (handTransform.position - HeldObject.transform.position) * holdingForce;

            //HeldObject.Rigidbody.Move(handTransform.position, Quaternion.identity);

            Vector3 handRot = handTransform.rotation.eulerAngles;

            if (handRot.x > 180f)
                handRot.x -= 360f;

            handRot.x = Mathf.Clamp(
                handRot.x,
                -heldClamXRotation,
                heldClamXRotation);

            HeldObject.transform.rotation =
                Quaternion.Euler(handRot + HeldObject.LiftDirectionOffset);
        }

        private void ChangeHeldObject()
        {
            if (HeldObject)
                DropObject(HeldObject);
            else if (SelectedObject is Liftable liftable)
                PickUpObject(liftable);
        }

        private void PickUpObject(Liftable obj)
        {
            if (obj == null)
            {
                Debug.LogWarning($"{nameof(PlayerInteractions)}: Attempted to pick up null object!");
                return;
            }

            HeldObject = obj;
            obj.PickUp(this, heldObjectLayer);
        }

        public void DropObject(Liftable obj)
        {
            if (obj == null)
            {
                Debug.LogWarning($"{nameof(PlayerInteractions)}: Attempted to drop null object!");
                return;
            }

            HeldObject = null;
            obj.Drop();
        }

        private void CheckHeldObjectOnTeleport()
        {
            if (HeldObject != null)
                DropObject(HeldObject);
        }

        #endregion
    }
}