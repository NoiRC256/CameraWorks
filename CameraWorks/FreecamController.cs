using System;
using UnhollowerRuntimeLib;
using UnityEngine;
using static CameraWorks.CameraWorksManager;
using static CameraWorks.InputManager;

namespace CameraWorks
{
    public class FreecamController : MonoBehaviour
    {
        public FreecamController(IntPtr ptr) : base(ptr) { }
        public FreecamController() : base(ClassInjector.DerivedConstructorPointer<FreecamController>())
        {
            ClassInjector.DerivedConstructorBody(this);
        }

        private const float FOV_SPEED_SCALE = 5f;

        #region FIELDS
        public FreecamUI ui;
        public bool unfocusOnEnableUI = true;

        private static bool active = false;
        private bool _isUIActive = false;
        private MotionController _motionController;
        private MotionController _standardMotionController;
        private MotionController _expertMotionController;

        #region Freecam stats

        private float _targetFov = 45f;
        private float _fovSpeed = 3f;
        private float _fovSmooth = 3f;

        private bool _rememberPos = false;
        private static bool _syncAvatarRoot = false;
        private float _avatarRootBias = 1f;
        private bool _hideAvatar = false;
        private bool _resetAvatarPosOnExit = true;
        private bool _isExpertMode = false; // Standard Mode or Expert Mode controls.

        #endregion

        private float _smoothedFov;

        private Vector3 _savedPosition;
        private Quaternion _savedRotation;
        private float _lastmoveSpeed;
        private float _lastlookSensitivity;
        private float _lastrollSpeed;
        private float _lastfov;
        #endregion

        #region PROPERTIES

        public bool Active {
            get { return active; }
            set {
                active = value;
                EnableLOD = _syncAvatarRoot; // Refresh set LOD loader property.
                if (value == false) IsUIActive = false;
            }
        }
        public bool IsUIActive {
            get => _isUIActive;
            set {
                if (value == true) {
                    if (Active) {
                        _isUIActive = value;
                        ui.panelVisible = value;
                        if (unfocusOnEnableUI) Focused = false;
                    }
                }
                else {
                    _isUIActive = value;
                    ui.panelVisible = value;
                    Focused = true;
                }
            }
        }
        public bool Focused {
            get => Cursor.lockState == CursorLockMode.Locked;
            set {
                Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
                Cursor.visible = !value;
                MotionCtrl.enabled = value;
            }
        }

        public Camera Cam { get; set; }
        public MotionController MotionCtrl { get { return _motionController; } set { _motionController = value; } }
        public Vector3 TargetPosition { get { return MotionCtrl.TargetPosition; } set { MotionCtrl.TargetPosition = value; } }
        public Quaternion TargetRotation { get { return MotionCtrl.TargetRotation; } set { MotionCtrl.TargetRotation = value; } }

        public float TargetFov { get { return _targetFov; } set { _targetFov = Mathf.Clamp(value, 1f, 160f); } }
        public float FovSpeed { get { return _fovSpeed; } set { _fovSpeed = Mathf.Clamp(value, 0f, 10f); } }
        public float FovSmooth { get { return _fovSmooth; } set { _fovSmooth = Mathf.Clamp(value, 0.1f, 10f); } }

        /// <summary>
        /// Refresh syncAvatarRoot. This should not be set constantly (i.e. on update), use EnableLOD instead.
        /// </summary>
        public bool EnableLOD {
            get { return _syncAvatarRoot; }
            set {
                if (active) { // If false -> true, setup LOD loader, and vise versa.
                    if (!_syncAvatarRoot && value) {
                        CameraWorksManager.Instance.SetupLodLoader();
                    }
                    else if (_syncAvatarRoot && !value) {
                        CameraWorksManager.Instance.UnsetLodLoader();
                    }
                }
                else {
                    CameraWorksManager.Instance.UnsetLodLoader();
                }

                _syncAvatarRoot = value;
            }
        }
        public float LODBias {
            get { return _avatarRootBias; }
            set { _avatarRootBias = Mathf.Clamp(value, -10f, 200f); }
        }
        public bool HideAvatar { get { return _hideAvatar; } set { _hideAvatar = value; } }
        public bool RememberPos { get { return _rememberPos; } set { _rememberPos = value; } }
        public bool ResetAvatarPosOnExit { get { return _resetAvatarPosOnExit; } set { _resetAvatarPosOnExit = value; } }

        public bool IsExpertMode {
            get { return _isExpertMode; }
            set {
                _isExpertMode = value;
                if (value) SwitchMotionController(_expertMotionController);
                else SwitchMotionController(_standardMotionController);
            }
        }

        #endregion

        #region MONOBEHAVIOUR

        public void Awake()
        {
            if (_standardMotionController == null)
                _standardMotionController = (MotionController)gameObject.AddComponent<MotionController>();

            if (_expertMotionController == null)
                _expertMotionController = (MotionController)gameObject.AddComponent<FlightMotionController>();

            SetMotionController(_standardMotionController);
            _expertMotionController.enabled = false;
            _standardMotionController.enabled = true;
        }

        public void OnEnable()
        {
            Focused = true;

            if (_rememberPos) {
                transform.localRotation = _savedRotation;
                transform.position = _savedPosition;
            }
            else {
                transform.rotation = mainCameraCam.transform.rotation;
                transform.position = mainCamera.transform.position;
            }

            MotionCtrl.InitMotionTargets();
        }

        public void OnDisable()
        {
            _savedRotation = transform.rotation;
            _savedPosition = transform.position;
        }

        public void Update()
        {
            HandleInput();
            if (Input.GetKeyDown(keyFocus)) {
                if (Focused == false) Focused = true;
                else Focused = false;
            }
            if (Input.GetKeyDown(keyGUI)) ToggleUI();
        }

        void LateUpdate()
        {
            if (!Active) return;
            if (EnableLOD) CameraWorksManager.LODFollow();
        }

        #endregion

        #region METHODS

        #region Handle Inputs

        public void HandleInput()
        {
            HandleFovInput();
            UpdateFov();
        }

        private void HandleFovInput()
        {
            if (Input.GetKey(keyFovDec)) _targetFov -= FovSpeed * Time.unscaledDeltaTime * FOV_SPEED_SCALE;
            if (Input.GetKey(keyFovInc)) _targetFov += FovSpeed * Time.unscaledDeltaTime * FOV_SPEED_SCALE;
            if (Input.GetKeyDown(keyFovReset)) _targetFov = 45f;
        }

        #endregion

        #region Updates

        private void UpdateFov()
        {
            _smoothedFov = Mathf.Lerp(Cam.fieldOfView, _targetFov, FovSmooth * Time.unscaledDeltaTime);
            Cam.fieldOfView = _smoothedFov;
        }

        #endregion

        public void ToggleUI()
        {
            IsUIActive = !IsUIActive;
        }

        private void SwitchMotionController(MotionController motionController)
        {
            if (MotionCtrl == motionController) return;

            MotionCtrl.enabled = false;
            SetMotionController(motionController);
            if (!Focused) MotionCtrl.enabled = true;
        }

        private void SetMotionController(MotionController motionController)
        {
            MotionCtrl = motionController;
        }

        #endregion
    }
}
