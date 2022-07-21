using System;
using UnhollowerRuntimeLib;
using UnityEngine;
using static CameraWorks.InputManager;

namespace CameraWorks
{
    public class FlightMotionController : MotionController
    {
        public FlightMotionController(IntPtr ptr) : base(ptr) { }
        public FlightMotionController() : base(ClassInjector.DerivedConstructorPointer<FlightMotionController>())
        {
            ClassInjector.DerivedConstructorBody(this);
        }

        protected const float ROLL_MOUSE_SPEED_SCALE = 1f;

        public override bool LookByWorldPlane {
            get { return false; }
        }

        #region MONOBHAVIOUR

        private void Awake()
        {
            Reset();
        }

        private void Reset()
        {
            _forwardMoveSpeed = 3f;
            _forwardMoveSmooth = 2.5f;
            _sidewaysMoveSpeed = 3f;
            _sidewaysMoveSmooth = 2.5f;
            _verticalMoveSpeed = 4f;
            _verticalMoveSmooth = 2f;

            _lookSpeed = 1f;
            _lookSmooth = 2f;
            _rollSpeed = 2f;
            _rollSmooth = 2.5f;

            _pitchKeySpeed = 3f;
            _pitchKeySmooth = 2f;
            _yawKeySpeed = 3f;
            _yawKeySmooth = 2f;
            _rollKeySpeed = 3f;
            _rollKeySmooth = 2f;

            _moveByWorldPlane = false; // If true, movement will be aligned to world plane.
            _lookByWorldPlane = false; // If true, pitch and yaw will be unaffected by roll.
        }

        #endregion

        #region METHODS

        #region Handle Inputs

        protected override void HandleRotationInput()
        {
            Vector3 input = CalculateDesiredRotationInput();

            _smoothedDeltaPitch = Mathf.Lerp(_smoothedDeltaPitch, input.x, Time.unscaledDeltaTime * LookSmooth);
            _smoothedDeltaYaw = Mathf.Lerp(_smoothedDeltaYaw, input.y, Time.unscaledDeltaTime * LookSmooth);
            _smoothedDeltaRoll = Mathf.Lerp(_smoothedDeltaRoll, input.z, Time.unscaledDeltaTime * RollSmooth);

            var smoothedInput = new Vector3(_smoothedDeltaPitch, _smoothedDeltaYaw, _smoothedDeltaRoll);
            if (LookByWorldPlane) // Pitch and yaw aligned to world plane, unaffected by roll.
            {
                _targetEuler += smoothedInput;
                _targetRotation = Quaternion.Euler(_targetEuler);
            }
            else // Free pitch, yaw, and roll.
            {
                _targetRotation *= Quaternion.Euler(smoothedInput); // Will smooth toward this.
            }
        }

        #endregion

        protected override void UpdateRotation()
        {
            // Set to target rotation.
            transform.localRotation = _targetRotation;
        }

        #region Calculations

        protected override Vector3 CalculateDesiredRotationInput()
        {
            Vector2 lookInput = GetLookInput();

            float targetPitchKeyInput = GetPitchKeyInput() * PitchKeySpeed * PITCH_KEY_SPEED_SCALE;
            _smoothedPitchKeyInput = Mathf.Lerp(_smoothedRollKeyInput, targetPitchKeyInput, PitchKeySmooth * Time.unscaledDeltaTime);

            float targetYawKeyInput = GetYawKeyInput() * YawKeySpeed * YAW_KEY_SPEED_SCALE;
            _smoothedYawKeyInput = Mathf.Lerp(_smoothedYawKeyInput, targetYawKeyInput, YawKeySmooth * Time.unscaledDeltaTime);

            float deltaPitch = lookInput.y * LookSpeed * LOOK_SPEED_SCALE
                + Time.unscaledDeltaTime * _smoothedPitchKeyInput;
            float deltaYaw = Time.unscaledDeltaTime * _smoothedYawKeyInput;
            float deltaRoll = -lookInput.x * RollSpeed * ROLL_MOUSE_SPEED_SCALE;

            return new Vector3(deltaPitch, deltaYaw, deltaRoll);
        }


        #endregion

        #region Get Inputs

        private float GetYawKeyInput()
        {
            return -GetRollKeyInput();
        }

        #endregion

        #endregion
    }
}
