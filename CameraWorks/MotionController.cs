using System;
using UnhollowerRuntimeLib;
using UnityEngine;
using static CameraWorks.InputManager;

namespace CameraWorks
{
    public class MotionController : MonoBehaviour
    {
        public MotionController(IntPtr ptr) : base(ptr) { }
        public MotionController() : base(ClassInjector.DerivedConstructorPointer<MotionController>())
        {
            ClassInjector.DerivedConstructorBody(this);
        }

        protected const float MOVE_SPEED_SCALE = 1f;
        protected const float LOOK_SPEED_SCALE = 1f;
        protected const float PITCH_KEY_SPEED_SCALE = 3f;
        protected const float YAW_KEY_SPEED_SCALE = 3f;
        protected const float ROLL_KEY_SPEED_SCALE = 3f;

        #region FIELDS

        #region Motion Stats
        protected float _moveSpeed = 3f;
        protected float _moveSmooth = 5f;
        protected float _forwardMoveSpeed = 3f;
        protected float _forwardMoveSmooth = 5f;
        protected float _sidewaysMoveSpeed = 3f;
        protected float _sidewaysMoveSmooth = 5f;
        protected float _verticalMoveSpeed = 3f;
        protected float _verticalMoveSmooth = 5f;
        protected float _moveSpeedMult = 1f;
        protected float _fastMoveMult = 2f;
        protected float _slowMoveMult = 0.3f;

        protected float _lookSpeed = 1f;
        protected float _lookSmooth = 6f;
        protected float _rollSpeed = 2f;
        protected float _rollSmooth = 10f;

        protected float _pitchKeySpeed = 3f;
        protected float _pitchKeySmooth = 3f;
        protected float _yawKeySpeed = 3f;
        protected float _yawKeySmooth = 3f;
        protected float _rollKeySpeed = 3f;
        protected float _rollKeySmooth = 3f;

        protected bool _moveByWorldPlane = false; // If true, movement will be aligned to world plane.
        protected bool _lookByWorldPlane = true; // If true, pitch and yaw will be unaffected by roll.
        #endregion

        protected Vector3 _targetPosition;
        protected Quaternion _targetRotation;
        protected Vector3 _targetEuler;
        protected Vector3 _smoothedEuler;

        protected Vector3 _smoothedPosition;
        protected float _smoothedSidewaysInput;
        protected float _smoothedForwardInput;
        protected float _smoothedVerticalInput;
        protected Quaternion _smoothedRotation;
        protected float _smoothedDeltaPitch;
        protected float _smoothedDeltaYaw;
        protected float _smoothedDeltaRoll;
        protected float _smoothedPitchKeyInput;
        protected float _smoothedYawKeyInput;
        protected float _smoothedRollKeyInput;

        #endregion

        #region PROPERTIES

        public Vector3 TargetPosition { get { return _targetPosition; } set { _targetPosition = value; } }
        public Quaternion TargetRotation { get { return _targetRotation; } set { _targetRotation = value; } }
        public Vector3 TargetEuler { get { return _targetEuler; } set { _targetEuler = value; } }

        public virtual float MoveSpeed {
            get { return _moveSpeed; }
            set { _moveSpeed = _sidewaysMoveSpeed = _verticalMoveSpeed = _forwardMoveSpeed = Mathf.Clamp(value, 0f, 20f); }
        }
        public virtual float MoveSmooth {
            get { return _moveSmooth; }
            set { _moveSmooth = _sidewaysMoveSmooth = _verticalMoveSmooth = _forwardMoveSmooth = Mathf.Clamp(value, 0.1f, 50f); }
        }
        public virtual float SidewaysMoveSpeed {
            get { return _sidewaysMoveSpeed; }
            set { _sidewaysMoveSpeed = Mathf.Clamp(value, 0f, 20f); }
        }
        public virtual float SidewaysMoveSmooth {
            get { return _forwardMoveSmooth; }
            set { _forwardMoveSmooth = Mathf.Clamp(value, 0.1f, 50f); }
        }
        public virtual float VerticalMoveSpeed {
            get { return _verticalMoveSpeed; }
            set { _verticalMoveSpeed = Mathf.Clamp(value, 0f, 20f); }
        }
        public virtual float VerticalMoveSmooth {
            get { return _verticalMoveSmooth; }
            set { _verticalMoveSmooth = Mathf.Clamp(value, 0.1f, 50f); }
        }
        public virtual float ForwardMoveSpeed {
            get { return _forwardMoveSpeed; }
            set { _forwardMoveSpeed = Mathf.Clamp(value, 0f, 20f); }
        }
        public virtual float ForwardMoveSmooth {
            get { return _forwardMoveSmooth; }
            set { _forwardMoveSmooth = Mathf.Clamp(value, 0.1f, 50f); }
        }

        public virtual float LookSpeed {
            get { return _lookSpeed; }
            set { _lookSpeed = Mathf.Clamp(value, 0f, 10f); }
        }
        public virtual float LookSmooth {
            get { return _lookSmooth; }
            set { _lookSmooth = Mathf.Clamp(value, 0.1f, 25f); }
        }
        public virtual float RollSpeed {
            get { return _rollSpeed; }
            set { _rollSpeed = Mathf.Clamp(value, 0f, 10f); }
        }
        public virtual float RollSmooth {
            get { return _rollSmooth; }
            set { _rollSmooth = Mathf.Clamp(value, 0.1f, 50f); }
        }

        public virtual float PitchKeySpeed {
            get { return _pitchKeySpeed; }
            set { _pitchKeySpeed = Mathf.Clamp(value, 0f, 30f); }
        }
        public virtual float PitchKeySmooth {
            get { return _pitchKeySmooth; }
            set { _pitchKeySmooth = Mathf.Clamp(value, 0.1f, 50f); }
        }
        public virtual float YawKeySpeed {
            get { return _yawKeySpeed; }
            set { _yawKeySpeed = Mathf.Clamp(value, 0f, 30f); }
        }
        public virtual float YawKeySmooth {
            get { return _yawKeySmooth; }
            set { _yawKeySmooth = Mathf.Clamp(value, 0.1f, 50f); }
        }
        public virtual float RollKeySpeed {
            get { return _rollKeySpeed; }
            set { _rollKeySpeed = Mathf.Clamp(value, 0f, 30f); }
        }
        public virtual float RollKeySmooth {
            get { return _rollKeySmooth; }
            set { _rollKeySmooth = Mathf.Clamp(value, 0.1f, 50f); }
        }

        public virtual bool MoveByWorldPlane {
            get { return _moveByWorldPlane; }
            set { _moveByWorldPlane = value; }
        }
        public virtual bool LookByWorldPlane {
            get { return _lookByWorldPlane; }
            set {
                _lookByWorldPlane = value;
                if (value) _targetEuler = transform.rotation.eulerAngles; // Update this as we start using it again.
            }
        }

        #endregion

        #region MONOBEHAVIOUR

        public void OnEnable()
        {
            InitMotionTargets();
        }

        public void Update()
        {
            HandleInput();
        }

        #endregion

        #region METHODS

        public void InitMotionTargets()
        {
            _targetRotation = transform.rotation;
            _targetEuler = _targetRotation.eulerAngles;
            _targetPosition = transform.position;
        }

        #region Handle Inputs

        protected virtual void HandleInput()
        {
            HandleRotationInput();
            UpdateRotation();

            HandleMoveSpeedMultInput();
            HandleMoveInput();
            UpdateMovement();
        }

        protected virtual void HandleRotationInput()
        {
            Vector3 input = CalculateDesiredRotationInput();

            if (LookByWorldPlane) // Pitch and yaw aligned to world plane, unaffected by roll.
            {
                _targetEuler += input;
                _targetRotation = Quaternion.Euler(_targetEuler);
            }
            else // Free pitch, yaw, and roll.
            {
                _targetRotation *= Quaternion.Euler(input);
            }

            if (Input.GetKey(keyRollReset)) ResetRoll();
        }

        protected virtual void HandleMoveInput()
        {
            _targetPosition += Time.unscaledDeltaTime * MOVE_SPEED_SCALE * CalculateDesiredMoveVelocity();
        }

        protected virtual void ResetRoll()
        {
            _targetEuler = new Vector3(_targetEuler.x, _targetEuler.y, 0f);

            var targetRotationEuler = _targetRotation.eulerAngles;
            _targetRotation = Quaternion.Euler(targetRotationEuler.x, targetRotationEuler.y, 0f);
        }

        protected virtual void HandleMoveSpeedMultInput()
        {
            if (Input.GetKey(keyFast)) _moveSpeedMult = _fastMoveMult;
            else if (Input.GetKey(keySlow)) _moveSpeedMult = _slowMoveMult;
            else _moveSpeedMult = 1f;
        }

        #endregion

        #region Calculations

        protected virtual Vector3 CalculateDesiredRotationInput()
        {
            Vector2 lookInput = GetLookInput();

            float targetPitchKeyInput = GetPitchKeyInput() * PitchKeySpeed * PITCH_KEY_SPEED_SCALE;
            _smoothedPitchKeyInput = Mathf.Lerp(_smoothedPitchKeyInput, targetPitchKeyInput, PitchKeySmooth * Time.unscaledDeltaTime);

            float targetRollKeyInput = GetRollKeyInput() * RollKeySpeed * ROLL_KEY_SPEED_SCALE;
            _smoothedRollKeyInput = Mathf.Lerp(_smoothedRollKeyInput, targetRollKeyInput, RollKeySmooth * Time.unscaledDeltaTime);

            float deltaPitch = lookInput.y * LookSpeed * LOOK_SPEED_SCALE
                + Time.unscaledDeltaTime * _smoothedPitchKeyInput;
            float deltaYaw = lookInput.x * LookSpeed * LOOK_SPEED_SCALE;
            float deltaRoll = Time.unscaledDeltaTime * _smoothedRollKeyInput;

            return new Vector3(deltaPitch, deltaYaw, deltaRoll);
        }

        protected virtual float CalculateDesiredMoveSpeed(float baseSpeed)
        {
            return baseSpeed * _moveSpeedMult * MOVE_SPEED_SCALE;
        }

        protected virtual Vector3 CalculateDesiredMoveVelocity()
        {
            Vector3 vel = Vector3.zero;
            Vector3 targetInput = GetMoveInput().normalized;

            _smoothedSidewaysInput = Mathf.Lerp(_smoothedSidewaysInput, targetInput.x, _sidewaysMoveSmooth * Time.unscaledDeltaTime);
            _smoothedVerticalInput = Mathf.Lerp(_smoothedVerticalInput, targetInput.y, _verticalMoveSmooth * Time.unscaledDeltaTime);
            _smoothedForwardInput = Mathf.Lerp(_smoothedForwardInput, targetInput.z, _forwardMoveSmooth * Time.unscaledDeltaTime);
            Vector3 smoothedInput = new Vector3(_smoothedSidewaysInput, _smoothedVerticalInput, _smoothedForwardInput);

            float sidewaysSpeed = CalculateDesiredMoveSpeed(_sidewaysMoveSpeed);
            float forwardSpeed = CalculateDesiredMoveSpeed(_forwardMoveSpeed);
            float verticalSpeed = CalculateDesiredMoveSpeed(_verticalMoveSpeed);

            vel += smoothedInput.x * sidewaysSpeed * transform.right;
            vel += smoothedInput.z * forwardSpeed * transform.forward;
            vel += smoothedInput.y * verticalSpeed * transform.up;

            if (MoveByWorldPlane)
            {
                vel = Vector3.ProjectOnPlane(vel, Vector3.up);
            }

            return vel;
        }

        #endregion

        #region Update Smoothing

        protected virtual void UpdateMovement()
        {
            _smoothedPosition = Vector3.Lerp(transform.position, _targetPosition, MoveSmooth * Time.unscaledDeltaTime);
            transform.position = _smoothedPosition;
        }

        protected virtual void UpdateRotation()
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, _targetRotation, LookSmooth * Time.unscaledDeltaTime);
            return;
        }

        #endregion

        #region Get Inputs

        protected Vector3 GetMoveInput()
        {
            float x = 0f;
            float y = 0f;
            float z = 0f;

            if (Input.GetKey(keyLeft)) x -= 1f;
            if (Input.GetKey(keyRight)) x += 1f;
            if (Input.GetKey(keyUp)) y += 1f;
            if (Input.GetKey(keyDown)) y -= 1f;
            if (Input.GetKey(keyForward)) z += 1f;
            if (Input.GetKey(keyBack)) z -= 1f;

            return new Vector3(x, y, z);
        }

        protected Vector2 GetLookInput()
        {
            return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * -1);
        }

        protected float GetPitchKeyInput()
        {
            float pitch = 0f;
            if (Input.GetKey(keyPitchUp)) pitch += 1f;
            if (Input.GetKey(keyPitchDown)) pitch -= 1f;
            return -pitch;
        }

        protected float GetRollKeyInput()
        {
            float roll = 0f;
            if (Input.GetKey(keyRollLeft)) roll += 1f;
            if (Input.GetKey(keyRollRight)) roll -= 1f;
            return roll;
        }

        #endregion

        #endregion
    }
}
