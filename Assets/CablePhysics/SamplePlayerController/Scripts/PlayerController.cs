#define USE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using System;
using UnityEngine;

namespace HPlayer
{
    public class PlayerController : MonoBehaviour
    {
        public enum Mods { None, Default, Sprint }

        #region Camera

        [Header("Camera Control")]
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float lookSpeedMouse = 4f;
        [SerializeField] private float lookSpeedController = 120f;
        [SerializeField, Range(-90f, 0f)] private float minPitch = -90f;
        [SerializeField, Range(0f, 90f)] private float maxPitch = 90f;

        private const float MouseSensitivityMultiplier = 0.01f;

        private float yaw;
        private float pitch;

        [field: SerializeField]
        public bool FreezCamera { get; set; }

        #endregion

        #region Movement

        [Header("Default")]
        [SerializeField, Min(1f)] private float defaultSpeed = 5f;
        [SerializeField, Min(0.5f)] private float defaultHeight = 1.9f;
        [SerializeField, Min(0f)] private float smoothMoveTime = 0.1f;

        [SerializeField] private LayerMask groundMask;
        [SerializeField] private float gravity = -18f;

        [SerializeField] private bool canJump = true;
        [SerializeField, Min(1f)] private float defaultJumpHeight = 1.2f;

        [Header("Sprint")]
        [SerializeField] private bool canSprint = true;
        [SerializeField, Min(1f)] private float sprintSpeed = 9f;
        [SerializeField, Min(1f)] private float sprintJumpHeight = 1.4f;

        [Header("Inputs")]
        [SerializeField] private bool inputJump;
        [SerializeField] private bool inputSprint;
        [SerializeField] private bool inputCrouch;
        [SerializeField] private Vector2 inputMove;
        [SerializeField] private Vector2 inputLook;

        [Header("States")]
        [SerializeField] private bool isGrounded;
        [SerializeField] private Mods currentMod = Mods.None;
        [SerializeField] private float currentSpeed;
        [SerializeField] private float currentJumpHeight;

        private Vector3 smoothV;
        [SerializeField] private Vector3 velocity;
        [SerializeField] private float verticalVelocity;

        [field: SerializeField]
        public bool FreezMovement { get; set; }

        #endregion

#if USE_INPUT_SYSTEM
        private InputAction lookAction;
        private InputAction moveAction;
        private InputAction jumpAction;
        private InputAction sprintAction;
        private InputAction crouchAction;
#endif

        private CharacterController controller;

        public static Action OnPlayerEnterPortal;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
#if USE_INPUT_SYSTEM
            RegisterInputs();
#endif
        }

#if USE_INPUT_SYSTEM
        private void RegisterInputs()
        {
            var map = new InputActionMap("Player");

            lookAction = map.AddAction("look", binding: "<Mouse>/delta");

            lookAction.AddBinding("<Gamepad>/rightStick")
                .WithProcessor("scaleVector2(x=15,y=15)");

            moveAction = map.AddAction("move", binding: "<Gamepad>/leftStick");

            moveAction.AddCompositeBinding("Dpad")
                .With("Up", "<Keyboard>/w")
                .With("Up", "<Keyboard>/upArrow")
                .With("Down", "<Keyboard>/s")
                .With("Down", "<Keyboard>/downArrow")
                .With("Left", "<Keyboard>/a")
                .With("Left", "<Keyboard>/leftArrow")
                .With("Right", "<Keyboard>/d")
                .With("Right", "<Keyboard>/rightArrow");

            jumpAction = map.AddAction("jump");
            jumpAction.AddBinding("<Keyboard>/space");
            jumpAction.AddBinding("<Gamepad>/buttonSouth");

            sprintAction = map.AddAction("sprint");
            sprintAction.AddBinding("<Keyboard>/leftShift");
            sprintAction.AddBinding("<Gamepad>/leftStickPress");

            crouchAction = map.AddAction("crouch");
            crouchAction.AddBinding("<Keyboard>/leftCtrl");
            crouchAction.AddBinding("<Gamepad>/rightStickPress");

            map.Enable();
        }

        private void OnDisable()
        {
            lookAction?.Disable();
            moveAction?.Disable();
            jumpAction?.Disable();
            sprintAction?.Disable();
            crouchAction?.Disable();
        }
#endif

        private void Start()
        {
            SetMode(Mods.Default);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            yaw = transform.eulerAngles.y;

            pitch = playerCamera.transform.localEulerAngles.x;
            if (pitch > 180f)
                pitch -= 360f;
        }

        private void Update()
        {
            float radius = controller.radius * 0.9f;

            Vector3 groundCheck =
                controller.bounds.center -
                (controller.bounds.extents.y - radius + 0.2f) * Vector3.up;

            isGrounded = Physics.CheckSphere(
                groundCheck,
                radius,
                groundMask
            );

            UpdateInputs();

            MoveCamera();
            ChoseMode();
            MovePlayer();
        }

        private void UpdateInputs()
        {
#if USE_INPUT_SYSTEM

            inputMove = moveAction.ReadValue<Vector2>();

            Vector2 lookDelta = lookAction.ReadValue<Vector2>();

            if (Mouse.current != null)
            {
                inputLook.x = lookDelta.x * lookSpeedMouse * MouseSensitivityMultiplier;
                inputLook.y = lookDelta.y * lookSpeedMouse * MouseSensitivityMultiplier;
            }
            else
            {
                inputLook.x = lookDelta.x * lookSpeedController * MouseSensitivityMultiplier * Time.deltaTime;
                inputLook.y = lookDelta.y * lookSpeedController * MouseSensitivityMultiplier * Time.deltaTime;
            }

            inputJump = jumpAction.IsPressed();
            inputSprint = sprintAction.IsPressed();
            inputCrouch = crouchAction.IsPressed();

#endif
        }

        private void SetMode(Mods mod)
        {
            if (currentMod == mod)
                return;

            switch (mod)
            {
                case Mods.Default:
                    currentSpeed = defaultSpeed;
                    currentJumpHeight = defaultJumpHeight;
                    break;

                case Mods.Sprint:
                    currentSpeed = sprintSpeed;
                    currentJumpHeight = sprintJumpHeight;
                    break;

                default:
                    currentSpeed = 0f;
                    currentJumpHeight = 0f;
                    break;
            }

            currentMod = mod;
        }

        private void ChoseMode()
        {
            if (canSprint && inputSprint)
                SetMode(Mods.Sprint);
            else
                SetMode(Mods.Default);
        }

        private void MoveCamera()
        {
            if (FreezCamera)
                return;

            yaw += inputLook.x;
            pitch -= inputLook.y;

            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            transform.rotation = Quaternion.Euler(0f, yaw, 0f);
            playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }

        private void MovePlayer()
        {
            if (FreezMovement)
                return;

            Vector3 inputDir = new Vector3(
                inputMove.x,
                0f,
                inputMove.y
            ).normalized;

            Vector3 worldInputDir = transform.TransformDirection(inputDir);

            Vector3 targetVelocity = worldInputDir * currentSpeed;

            velocity = Vector3.SmoothDamp(
                velocity,
                targetVelocity,
                ref smoothV,
                smoothMoveTime
            );

            if (isGrounded)
            {
                if (verticalVelocity < 0f)
                    verticalVelocity = -2f;

                if (canJump && inputJump && verticalVelocity < 1f)
                {
                    verticalVelocity =
                        Mathf.Sqrt(currentJumpHeight * -2f * gravity);
                }
            }

            verticalVelocity += gravity * Time.deltaTime;

            velocity = new Vector3(
                velocity.x,
                verticalVelocity,
                velocity.z
            );

            controller.Move(velocity * Time.deltaTime);
        }

        public void SetPosition(Vector3 position)
        {
            controller.enabled = false;

            transform.position = position;

            velocity = Vector3.zero;
            smoothV = Vector3.zero;
            verticalVelocity = 0f;

            controller.enabled = true;
        }

        public void SetRotation(float rotX, float rotY)
        {
            if (rotX > 180f)
                rotX -= 360f;

            pitch = rotX;
            yaw = rotY;

            playerCamera.transform.localRotation =
                Quaternion.Euler(rotX, 0f, 0f);

            transform.rotation =
                Quaternion.Euler(0f, rotY, 0f);
        }

        public void SetCameraBackgroundOnSkyBox(float viewRange)
        {
            playerCamera.clearFlags = CameraClearFlags.Skybox;
            playerCamera.farClipPlane = viewRange;
        }

        public void SetCameraBackgroundOnSolidColor(Color color, float viewRange)
        {
            playerCamera.clearFlags = CameraClearFlags.SolidColor;
            playerCamera.backgroundColor = color;
            playerCamera.farClipPlane = viewRange;
        }

        private void SetEnable() => SetEnable(true);
        private void SetDisable() => SetEnable(false);

        public void SetEnable(bool enableState)
        {
            if (enableState)
            {
                FreezCamera = true;
                enabled = false;
            }
            else
            {
                FreezCamera = false;
                enabled = true;
            }
        }
    }
}