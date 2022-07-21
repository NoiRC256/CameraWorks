using System;
using UnhollowerRuntimeLib;
using UnityEngine;
using static CameraWorks.CameraWorksManager;

namespace CameraWorks
{
    public class FreecamUI : MonoBehaviour
    {
        public FreecamUI(IntPtr ptr) : base(ptr) { }
        public FreecamUI() : base(ClassInjector.DerivedConstructorPointer<FreecamUI>())
        {
            ClassInjector.DerivedConstructorBody(this);
        }

        #region FIELDS

        public FreecamController c;
        private MotionController motor => c.MotionCtrl;
        public bool panelVisible = false;

        #region IMGUI Settings (Legacy)
        public Rect CamMenuRect;
        public Rect MotionMenuRect;
        private int smallSpace = 8;
        private int mediumSpace = 10;
        private int largeSpace = 20;
        private GUILayoutOption[] buttonStyle = new GUILayoutOption[] { GUILayout.Width(60) };
        private GUILayoutOption[] buttonStyle2 = new GUILayoutOption[] { GUILayout.Width(25) };
        private GUILayoutOption[] halfPanel = new GUILayoutOption[] { GUILayout.Width(290) };
        private GUILayoutOption[] fullPanel = new GUILayoutOption[] { GUILayout.Width(580) };

        private float speedIncrement = 0.5f;
        private float smoothIncrement = 0.5f;

        private bool movementSettingsSeparateAxis = false;
        private bool rotationExtraSettings = false;
        private bool rotationExpertExtraSettings = false;
        #endregion

        #endregion

        #region IMGUI
        private void Awake()
        {
            CamMenuRect = new Rect(Screen.width - 400, Screen.height / 3, 330, 20);
            MotionMenuRect = new Rect(20, Screen.height / 3, 580, 20);
        }

        public void OnGUI()
        {
            if (panelVisible == true) {
                CamMenuRect = GUILayout.Window(0, CamMenuRect, (GUI.WindowFunction)CameraWindow, "noirccc CameraWorks", new GUILayoutOption[0]);
                MotionMenuRect = GUILayout.Window(1, MotionMenuRect, (GUI.WindowFunction)MotionWindow, "noirccc CameraWorks - Motion Control", new GUILayoutOption[0]);
            }
        }

        public void CameraWindow(int id)
        {
            if (id == 0) {
                GUILayout.Space(mediumSpace);

                #region Fov

                // FOV.
                c.TargetFov = DrawStatSlider(c.TargetFov, "FoV",
                    min: 1f, max: 160f, defaultValue: 45f, 1f, "F1");

                GUILayout.Space(smallSpace);

                // FOV speed
                c.FovSpeed = DrawStatSlider(c.FovSpeed, "FoV Speed",
                    min: 0.1f, max: 10f, defaultValue: 3f, speedIncrement, "F2");

                GUILayout.Space(smallSpace);

                // FOV damping.
                c.FovSmooth = DrawStatSlider(c.FovSmooth, "FoV Damping",
                    min: 0.1f, max: 10f, defaultValue: 3f, smoothIncrement, "F2");

                GUILayout.Space(smallSpace);

                #endregion

                #region Other

                // Game speed

                Time.timeScale = DrawStatSlider(Time.timeScale, "Game Speed",
                    min: 0f, max: 10f, defaultValue: 2f, 0.1f, "F2");

                GUILayout.Space(smallSpace);

                //
                c.RememberPos = GUILayout.Toggle(c.RememberPos, "Remember Last Position", new GUILayoutOption[0]);

                GUILayout.Space(smallSpace);

                //
                c.EnableLOD = GUILayout.Toggle(c.EnableLOD, "Enable LOD Loading (Hides Player)", new GUILayoutOption[0]);

                GUILayout.Space(smallSpace);

                if (c.EnableLOD) {
                    // LOD forward bias.
                    c.LODBias = DrawStatSlider(c.LODBias, "LOD Bias",
                        min: -10f, max: 200f, defaultValue: 1f, 1f, "F1");

                    GUILayout.Space(smallSpace);
                }

                // 
                c.ResetAvatarPosOnExit = GUILayout.Toggle(c.ResetAvatarPosOnExit, "Reset LOD position on Exit", new GUILayoutOption[0]);

                GUILayout.Space(smallSpace);

                #endregion

                GUILayout.Space(mediumSpace);

                GUI.DragWindow();
            }
        }


        public void MotionWindow(int id)
        {
            if (c.IsExpertMode) ExpertMotionWindow();
            else StandardMotionWindow();

            GUI.DragWindow();
        }

        public void StandardMotionWindow()
        {
            GUILayout.BeginHorizontal(fullPanel);
            GUILayout.BeginVertical(halfPanel);

            GUILayout.Space(mediumSpace);

            // Standard Mode or Expert Mode controls.
            c.IsExpertMode = GUILayout.Toggle(c.IsExpertMode, " Flight Control Scheme", new GUILayoutOption[0]);

            DrawSmallDivider();

            #region Movement

            // Separately control movement axis stats.
            movementSettingsSeparateAxis = GUILayout.Toggle(movementSettingsSeparateAxis, " + Per-Axis Settings", new GUILayoutOption[0]);

            GUILayout.Space(smallSpace);

            if (!movementSettingsSeparateAxis) {
                // Movement speed.
                motor.MoveSpeed = DrawStatSlider(motor.MoveSpeed, "Movement Speed",
                    min: 0.01f, max: 20f, defaultValue: 3f, speedIncrement, "F2");

                GUILayout.Space(smallSpace);

                // Movement damping.
                motor.MoveSmooth = DrawStatSlider(motor.MoveSmooth, "Movement Damping",
                    min: 0.1f, max: 50f, defaultValue: 5f, smoothIncrement, "F2");
            }
            else {
                // Forward speed.
                motor.ForwardMoveSpeed = DrawStatSlider(motor.ForwardMoveSpeed, "Forward Movement Speed",
                    min: 0.01f, max: 20f, defaultValue: 3f, speedIncrement, "F2");

                GUILayout.Space(smallSpace);

                // Forward damping.
                motor.ForwardMoveSmooth = DrawStatSlider(motor.ForwardMoveSmooth, "Forward Movement Damping",
                    min: 0.1f, max: 50f, defaultValue: 5f, smoothIncrement, "F2");

                GUILayout.Space(smallSpace);

                // Sideways speed.
                motor.SidewaysMoveSpeed = DrawStatSlider(motor.SidewaysMoveSpeed, "Sideways Movement Speed",
                    min: 0.01f, max: 20f, defaultValue: 3f, speedIncrement, "F2");

                GUILayout.Space(smallSpace);

                // Sideways damping.
                motor.SidewaysMoveSmooth = DrawStatSlider(motor.SidewaysMoveSmooth, "Sideways Movement Damping",
                    min: 0.1f, max: 50f, defaultValue: 5f, smoothIncrement, "F2");

                GUILayout.Space(smallSpace);

                // Vertical speed.
                motor.VerticalMoveSpeed = DrawStatSlider(motor.VerticalMoveSpeed, "Vertical Movement Speed",
                    min: 0.01f, max: 20f, defaultValue: 3f, speedIncrement, "F2");

                GUILayout.Space(smallSpace);

                // Vertical damping.
                motor.VerticalMoveSmooth = DrawStatSlider(motor.VerticalMoveSmooth, "Vertical Movement Damping",
                    min: 0.1f, max: 50f, defaultValue: 5f, smoothIncrement, "F2");
            }
            #endregion

            GUILayout.EndVertical();

            GUILayout.BeginVertical(halfPanel);

            #region Rotation

            rotationExtraSettings = GUILayout.Toggle(rotationExtraSettings, " + Extra Rotation Settings", new GUILayoutOption[0]);

            GUILayout.Space(smallSpace);

            // Look speed.
            motor.LookSpeed = DrawStatSlider(motor.LookSpeed, "Look Speed",
                min: 0.1f, max: 10f, defaultValue: 3f, speedIncrement, "F1");

            GUILayout.Space(smallSpace);

            // Look damping.
            motor.LookSmooth = DrawStatSlider(motor.LookSmooth, "Look Damping",
                min: 0.1f, max: 25f, defaultValue: 15f, smoothIncrement, "F2");

            GUILayout.Space(smallSpace);

            // Roll key speed.
            motor.RollKeySpeed = DrawStatSlider(motor.RollKeySpeed, "Roll Key Speed",
                min: 0.1f, max: 20f, defaultValue: 3f, speedIncrement, "F1");

            GUILayout.Space(smallSpace);

            // Roll key damping.
            motor.RollKeySmooth = DrawStatSlider(motor.RollKeySmooth, "Roll Key Damping",
                min: 0.1f, max: 50f, defaultValue: 3f, smoothIncrement, "F2");

            GUILayout.Space(smallSpace);

            if (rotationExtraSettings) {

                // Pitch key speed.
                motor.PitchKeySpeed = DrawStatSlider(motor.PitchKeySpeed, "Pitch Key Speed",
                    min: 0.1f, max: 20f, defaultValue: 3f, speedIncrement, "F1");

                GUILayout.Space(smallSpace);

                // Pitch key damping.
                motor.PitchKeySmooth = DrawStatSlider(motor.PitchKeySmooth, "Pitch Key Damping",
                    min: 0.1f, max: 50f, defaultValue: 3f, smoothIncrement, "F2");
            }

            #endregion

            //
            motor.MoveByWorldPlane = GUILayout.Toggle(motor.MoveByWorldPlane,
                " Movement axis Unaffected by Roll", new GUILayoutOption[0]);

            GUILayout.Space(smallSpace);

            //
            motor.LookByWorldPlane = GUILayout.Toggle(motor.LookByWorldPlane,
                " Pitch and Yaw Unaffected by Roll", new GUILayoutOption[0]);

            GUILayout.Space(mediumSpace);

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        public void ExpertMotionWindow()
        {
            GUILayout.BeginHorizontal(fullPanel);
            GUILayout.BeginVertical(halfPanel);

            GUILayout.Space(mediumSpace);

            // Standard Mode or Expert Mode controls.
            c.IsExpertMode = GUILayout.Toggle(c.IsExpertMode, " Flight Control Scheme", new GUILayoutOption[0]);

            GUILayout.Label("<Flight Control Mode>", new GUILayoutOption[0]);

            DrawTinyDivider();

            #region Movement

            // Forward speed.
            motor.ForwardMoveSpeed = DrawStatSlider(motor.ForwardMoveSpeed, "Forward Movement Speed",
                min: 0.01f, max: 20f, defaultValue: 3f, speedIncrement, "F2");

            GUILayout.Space(smallSpace);

            // Forward damping.
            motor.ForwardMoveSmooth = DrawStatSlider(motor.ForwardMoveSmooth, "Forward Movement Damping",
                min: 0.1f, max: 50f, defaultValue: 2.5f, smoothIncrement, "F2");

            GUILayout.Space(smallSpace);

            // Sideways speed.
            motor.SidewaysMoveSpeed = DrawStatSlider(motor.SidewaysMoveSpeed, "Sideways Movement Speed",
                min: 0.01f, max: 20f, defaultValue: 3f, speedIncrement, "F2");

            GUILayout.Space(smallSpace);

            // Sideways damping.
            motor.SidewaysMoveSmooth = DrawStatSlider(motor.SidewaysMoveSmooth, "Sideways Movement Damping",
                min: 0.1f, max: 50f, defaultValue: 2.5f, smoothIncrement, "F2");

            GUILayout.Space(smallSpace);

            // Vertical speed.
            motor.VerticalMoveSpeed = DrawStatSlider(motor.VerticalMoveSpeed, "Vertical Movement Speed",
                min: 0.01f, max: 20f, defaultValue: 4f, speedIncrement, "F2");

            GUILayout.Space(smallSpace);

            // Vertical damping.
            motor.VerticalMoveSmooth = DrawStatSlider(motor.VerticalMoveSmooth, "Vertical Movement Damping",
                min: 0.1f, max: 50f, defaultValue: 2f, smoothIncrement, "F2");

            #endregion

            GUILayout.EndVertical();

            GUILayout.BeginVertical(halfPanel);

            #region Rotation

            // Separately control movement axis stats.
            rotationExpertExtraSettings = GUILayout.Toggle(rotationExpertExtraSettings, " + Extra Rotation Settings", new GUILayoutOption[0]);

            GUILayout.Space(smallSpace);

            // Look speed.
            motor.LookSpeed = DrawStatSlider(motor.LookSpeed, "Look Speed",
                min: 0.1f, max: 10f, defaultValue: 5f, speedIncrement, "F1");

            GUILayout.Space(smallSpace);

            // Look damping.
            motor.LookSmooth = DrawStatSlider(motor.LookSmooth, "Look Damping",
                min: 0.1f, max: 25f, defaultValue: 2f, smoothIncrement, "F2");

            GUILayout.Space(smallSpace);

            // Roll speed.
            motor.RollSpeed = DrawStatSlider(motor.RollSpeed, "Roll Speed",
                min: 0.1f, max: 20f, defaultValue: 5f, speedIncrement, "F1");

            GUILayout.Space(smallSpace);

            // Roll damping.
            motor.RollSmooth = DrawStatSlider(motor.RollSmooth, "Roll Damping",
                min: 0.1f, max: 50f, defaultValue: 3f, smoothIncrement, "F2");

            GUILayout.Space(smallSpace);

            if (rotationExpertExtraSettings) {
                // Pitch key speed.
                motor.PitchKeySpeed = DrawStatSlider(motor.PitchKeySpeed, "Pitch Key Speed",
                    min: 0.1f, max: 20f, defaultValue: 3f, speedIncrement, "F1");

                GUILayout.Space(smallSpace);

                // Pitch key damping.
                motor.PitchKeySmooth = DrawStatSlider(motor.PitchKeySmooth, "Pitch Key Damping",
                    min: 0.1f, max: 50f, defaultValue: 3f, smoothIncrement, "F2");

                GUILayout.Space(smallSpace);

                // Yaw key speed.
                motor.YawKeySpeed = DrawStatSlider(motor.YawKeySpeed, "Yaw Key Speed",
                    min: 0.1f, max: 20f, defaultValue: 6f, speedIncrement, "F1");

                GUILayout.Space(smallSpace);

                // Yaw key damping.
                motor.YawKeySmooth = DrawStatSlider(motor.YawKeySmooth, "Yaw Key Damping",
                    min: 0.1f, max: 50f, defaultValue: 3f, smoothIncrement, "F2");
            }

            #endregion;

            //
            motor.MoveByWorldPlane = GUILayout.Toggle(motor.MoveByWorldPlane,
                " Movement Axis Unaffected by Roll", new GUILayoutOption[0]);

            //GUILayout.Space(smallSpace);

            //
            //motor.LookByWorldPlane = GUILayout.Toggle(motor.LookByWorldPlane, " Pitch and Yaw Unaffected by Roll", new GUILayoutOption[0]);

            GUILayout.Space(mediumSpace);

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        private float DrawStatSlider(float value, string text = "Slider", float min = 0.1f, float max = 10f,
            float defaultValue = 3f, float increment = 0.5f, string format = "F2")
        {
            GUILayout.Label($"{text}: {value.ToString(format)}", new GUILayoutOption[0]);
            value = GUILayout.HorizontalSlider(value, min, max, new GUILayoutOption[0]);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (GUILayout.Button("Reset", buttonStyle))
                value = defaultValue;
            if (GUILayout.Button("-", buttonStyle2))
                value -= increment;
            if (GUILayout.Button("+", buttonStyle2))
                value += increment;
            GUILayout.EndHorizontal();

            return value;
        }

        private void DrawTinyDivider()
        {
            DrawHorizontalLine();
            GUILayout.Space(3);
        }
        private void DrawSmallDivider()
        {
            GUILayout.Space(smallSpace);
            DrawHorizontalLine();
            GUILayout.Space(3);
        }
        private void DrawMediumDivider()
        {
            GUILayout.Space(mediumSpace);
            DrawHorizontalLine();
            GUILayout.Space(mediumSpace);
        }

        private void DrawHorizontalLine()
        {
            /*
            GUIStyle st = new GUIStyle(GUI.skin.label);
            Vector2 coff = st.contentOffset;
            coff.y -= 5;
            st.contentOffset = coff;
            */
            GUILayout.Label("____________", new GUILayoutOption[0]);
        }

        #endregion
    }
}
