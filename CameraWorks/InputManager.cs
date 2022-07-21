using System.Collections.Generic;
using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace CameraWorks
{
    static class InputManager
    {
        #region CONTROLS

        #region Main
        // Main
        public static KeyCode keyInit;
        public static KeyCode keyTerminate;
        public static KeyCode keyFreecam;
        public static KeyCode keyRes4k;
        public static KeyCode keyRes1080p;
        public static KeyCode keyHUD;
        public static KeyCode keyHP;
        public static KeyCode keyDamage;
        public static KeyCode keyTimeAdd1;
        public static KeyCode keyTimeAdd5;
        public static KeyCode keyTimeSub1;
        public static KeyCode keyTimeSub5;
        public static KeyCode keyTimeToggle5;
        public static KeyCode keyTimePause;
        public static KeyCode keyTimeReset;
        #endregion

        #region Freecam
        // Freecam
        public static KeyCode keyGUI;
        public static KeyCode keyFocus;

        public static KeyCode keyForward;
        public static KeyCode keyBack;
        public static KeyCode keyLeft;
        public static KeyCode keyRight;
        public static KeyCode keyUp;
        public static KeyCode keyDown;

        public static KeyCode keyPitchUp;
        public static KeyCode keyPitchDown;
        public static KeyCode keyRollLeft;
        public static KeyCode keyRollRight;
        public static KeyCode keyRollReset;

        public static KeyCode keyFovInc;
        public static KeyCode keyFovDec;
        public static KeyCode keyFovReset;
        public static KeyCode keyFast;
        public static KeyCode keySlow;

        public static MelonPreferences_Category cameraWorksHotkeys;
        public static MelonPreferences_Entry<KeyCode> keyFreecampref;
        public static MelonPreferences_Entry<KeyCode> keyInitPref;
        public static MelonPreferences_Entry<KeyCode> keyTerminatePref;
        public static MelonPreferences_Entry<KeyCode> keyRes4kpref;
        public static MelonPreferences_Entry<KeyCode> keyRes1080ppref;
        public static MelonPreferences_Entry<KeyCode> keyHUDpref;
        public static MelonPreferences_Entry<KeyCode> keyHPpref;
        public static MelonPreferences_Entry<KeyCode> keyDamagepref;
        public static MelonPreferences_Entry<KeyCode> keyTimeAdd1pref;
        public static MelonPreferences_Entry<KeyCode> keyTimeAdd5pref;
        public static MelonPreferences_Entry<KeyCode> keyTimeSub1pref;
        public static MelonPreferences_Entry<KeyCode> keyTimeSub5pref;
        public static MelonPreferences_Entry<KeyCode> keyTimeToggle5pref;
        public static MelonPreferences_Entry<KeyCode> keyTimePausepref;
        public static MelonPreferences_Entry<KeyCode> keyTimeResetpref;

        public static MelonPreferences_Entry<KeyCode> keyGUIpref;
        public static MelonPreferences_Entry<KeyCode> keyFocuspref;

        public static MelonPreferences_Entry<KeyCode> keyForwardpref;
        public static MelonPreferences_Entry<KeyCode> keyBackpref;
        public static MelonPreferences_Entry<KeyCode> keyLeftpref;
        public static MelonPreferences_Entry<KeyCode> keyRightpref;
        public static MelonPreferences_Entry<KeyCode> keyUppref;
        public static MelonPreferences_Entry<KeyCode> keyDownpref;

        public static MelonPreferences_Entry<KeyCode> keyPitchUppref;
        public static MelonPreferences_Entry<KeyCode> keyPitchDownpref;
        public static MelonPreferences_Entry<KeyCode> keyRollLeftpref;
        public static MelonPreferences_Entry<KeyCode> keyRollRightpref;
        public static MelonPreferences_Entry<KeyCode> keyRollResetpref;

        public static MelonPreferences_Entry<KeyCode> keyFovIncpref;
        public static MelonPreferences_Entry<KeyCode> keyFovDecpref;
        public static MelonPreferences_Entry<KeyCode> keyFovResetpref;
        public static MelonPreferences_Entry<KeyCode> keyFastpref;
        public static MelonPreferences_Entry<KeyCode> keySlowpref;
        #endregion

        #endregion

        public static void InitializePrefs()
        {
            cameraWorksHotkeys = MelonPreferences.CreateCategory("CameraWorks");

            keyInitPref = cameraWorksHotkeys.CreateEntry("Initialize", KeyCode.F9);
            keyInit = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "Initialize");
            keyTerminatePref = cameraWorksHotkeys.CreateEntry("Terminate", KeyCode.F8);
            keyTerminate = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "Terminate");
            keyFreecampref = cameraWorksHotkeys.CreateEntry("ToggleFreecam", KeyCode.Insert);
            keyFreecam = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "ToggleFreecam");
            keyRes4kpref = cameraWorksHotkeys.CreateEntry("SetResolutionTo4K", KeyCode.KeypadPlus);
            keyRes4k = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "SetResolutionTo4K");
            keyRes1080ppref = cameraWorksHotkeys.CreateEntry("SetResolutionTo1080p", KeyCode.KeypadMinus);
            keyRes1080p = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "SetResolutionTo1080p");

            keyHUDpref = cameraWorksHotkeys.CreateEntry("ToggleGameHUD", KeyCode.PageDown);
            keyHUD = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "ToggleGameHUD");
            keyHPpref = cameraWorksHotkeys.CreateEntry("RemoveHealthHUD", KeyCode.LeftBracket);
            keyHP = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "RemoveHealthHUD");
            keyDamagepref = cameraWorksHotkeys.CreateEntry("ToggleDamageHUD", KeyCode.RightBracket);
            keyDamage = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "ToggleDamageHUD");

            keyTimeAdd1pref = cameraWorksHotkeys.CreateEntry("GameSpeedInc1", KeyCode.UpArrow);
            keyTimeAdd1 = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "GameSpeedInc1");
            keyTimeAdd5pref = cameraWorksHotkeys.CreateEntry("GameSpeedInc5", KeyCode.RightArrow);
            keyTimeAdd5 = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "GameSpeedInc5");
            keyTimeSub1pref = cameraWorksHotkeys.CreateEntry("GameSpeedDec1", KeyCode.DownArrow);
            keyTimeSub1 = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "GameSpeedDec1");
            keyTimeSub5pref = cameraWorksHotkeys.CreateEntry("GameSpeedDec5", KeyCode.LeftArrow);
            keyTimeSub5 = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "GameSpeedDec5");
            keyTimeToggle5pref = cameraWorksHotkeys.CreateEntry("ToggleGameSpeedTo5", KeyCode.CapsLock);
            keyTimeToggle5 = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "ToggleGameSpeedTo5");
            keyTimePausepref = cameraWorksHotkeys.CreateEntry("TogglePause", KeyCode.Delete);
            keyTimePause = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "TogglePause");
            keyTimeResetpref = cameraWorksHotkeys.CreateEntry("ResetSpeed", KeyCode.End);
            keyTimeReset = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "ResetSpeed");

            keyGUIpref = cameraWorksHotkeys.CreateEntry("ToggleGUI", KeyCode.F10);
            keyGUI = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "ToggleGUI");
            keyFocuspref = cameraWorksHotkeys.CreateEntry("ToggleCursorFocus", KeyCode.Quote);
            keyFocus = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "ToggleCursorFocus");

            keyForwardpref = cameraWorksHotkeys.CreateEntry("Forward", KeyCode.I);
            keyForward = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "Forward");
            keyBackpref = cameraWorksHotkeys.CreateEntry("Back", KeyCode.K);
            keyBack = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "Back");
            keyLeftpref = cameraWorksHotkeys.CreateEntry("Left", KeyCode.J);
            keyLeft = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "Left");
            keyRightpref = cameraWorksHotkeys.CreateEntry("Right", KeyCode.L);
            keyRight = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "Right");
            keyUppref = cameraWorksHotkeys.CreateEntry("Up", KeyCode.RightAlt);
            keyUp = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "Up");
            keyDownpref = cameraWorksHotkeys.CreateEntry("Down", KeyCode.B);
            keyDown = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "Down");

            keyPitchUppref = cameraWorksHotkeys.CreateEntry("PitchUp", KeyCode.P);
            keyPitchUp = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "PitchUp");
            keyPitchDownpref = cameraWorksHotkeys.CreateEntry("PitchDown", KeyCode.Semicolon);
            keyPitchDown = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "PitchDown");
            keyRollLeftpref = cameraWorksHotkeys.CreateEntry("RollLeft", KeyCode.U);
            keyRollLeft = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "RollLeft");
            keyRollRightpref = cameraWorksHotkeys.CreateEntry("RollRight", KeyCode.O);
            keyRollRight = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "RollRight");
            keyRollResetpref = cameraWorksHotkeys.CreateEntry("ResetRoll", KeyCode.Y);
            keyRollReset = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "ResetRoll");

            keyFovIncpref = cameraWorksHotkeys.CreateEntry("IncreaseFOV", KeyCode.Alpha9);
            keyFovInc = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "IncreaseFOV");
            keyFovDecpref = cameraWorksHotkeys.CreateEntry("DecreaseFOV", KeyCode.Alpha8);
            keyFovDec = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "DecreaseFOV");
            keyFovResetpref = cameraWorksHotkeys.CreateEntry("ResetFOV", KeyCode.Alpha0);
            keyFovReset = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "ResetFOV");
            keyFastpref = cameraWorksHotkeys.CreateEntry("FastMovement", KeyCode.G);
            keyFast = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "FastMovement");
            keySlowpref = cameraWorksHotkeys.CreateEntry("SlowMovement", KeyCode.V);
            keySlow = MelonPreferences.GetEntryValue<KeyCode>("CameraWorks", "SlowMovement");

            MelonPreferences.Save();
        }
    }
}
