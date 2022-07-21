using System.Collections.Generic;
using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using static CameraWorks.InputManager;

namespace CameraWorks
{
    public class CameraWorksManager : MelonMod
    {
        private static CameraWorksManager instance;
        public static CameraWorksManager Instance {
            get => instance;
            private set => instance = value;
        }

        #region FIELDS

        public static float lastTimeScale = 1.0f;

        public static Transform entityRoot;
        public static Transform avatarRoot;

        // UI.
        public GameObject hudUI;
        public GameObject uidUI;
        public GameObject hpUI;
        public GameObject damageUI;

        // Camera.
        public static GameObject mainCamera;
        public static Camera mainCameraCam;
        public static GameObject freeCamera;
        public static Camera freeCameraCam;
        public static FreecamController freecamController;
        public static FreecamUI freecamUI;

        // Current avatar.
        public static AvatarController avatarController;
        private static Transform prevAvatarTr;
        private static Vector3 avatarSavedPos = Vector3.zero;
        private static Quaternion avatarSavedRot = Quaternion.identity;
        private static bool isUsingAvatarLodLoader = false;

        #endregion

        #region MONOBEHAVIOUR

        public override void OnApplicationStart()
        {
            ClassInjector.RegisterTypeInIl2Cpp<MotionController>();
            ClassInjector.RegisterTypeInIl2Cpp<FlightMotionController>();
            ClassInjector.RegisterTypeInIl2Cpp<FreecamController>();
            ClassInjector.RegisterTypeInIl2Cpp<FreecamUI>();

            InputManager.InitializePrefs();
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(keyInit)) {
                Init();
            }

            if (Input.GetKeyDown(keyFreecam)) {
                if (freeCamera) {
                    if (mainCamera.activeInHierarchy == true) {
                        ActivateFreecam();
                    }
                    else {
                        DeactivateFreecam();
                    }
                }
                else {
                    LoggerInstance.Msg("Free camera not found. Please inject it by pressing F9.");
                }
            }

            #region Input

            if (Input.GetKeyDown(keyRes4k)) {
                Screen.SetResolution(3840, 2160, false);
                mainCameraCam.rect = new Rect(0, 0, 3840, 2160);
            }
            if (Input.GetKeyDown(keyRes1080p)) {
                Screen.SetResolution(1920, 1080, false);
                mainCameraCam.rect = new Rect(0, 0, 1920, 1080);
            }
            if (Input.GetKeyDown(keyHUD)) {
                if (hudUI.activeInHierarchy == true)
                    hudUI.SetActive(false);
                else
                    hudUI.SetActive(true);
                if (uidUI.activeInHierarchy == true)
                    uidUI.SetActive(false);
                else
                    uidUI.SetActive(true);

            }
            if (Input.GetKeyDown(keyHP)) {
                hpUI = GameObject.Find(Constants.HP_CANVAS_PATH);
                do {
                    hpUI.SetActive(false);
                    hpUI = GameObject.Find(Constants.HP_CANVAS_PATH);
                } while (hpUI != null);
            }
            if (Input.GetKeyDown(keyDamage)) {
                if (damageUI) {
                    if (damageUI.activeInHierarchy == true)
                        damageUI.SetActive(false);
                    else
                        damageUI.SetActive(true);
                }
                else {
                    damageUI = GameObject.Find("/Canvas/Pages/InLevelMainPage/GrpMainPage/ParticleDamageTextContainer");
                    if (damageUI.activeInHierarchy == true)
                        damageUI.SetActive(false);
                    else
                        damageUI.SetActive(true);
                }
            }
            if (Input.GetKeyDown(keyTimePause)) {
                if (Time.timeScale != 0.0f)
                    Time.timeScale = 0.0f;
                else
                    Time.timeScale = lastTimeScale;
            }
            if (Input.GetKeyDown(keyTimeReset)) {
                Time.timeScale = 1.0f;
                lastTimeScale = Time.timeScale;
            }
            if (Input.GetKeyDown(keyTimeToggle5)) {
                if (Time.timeScale != 5.0f)
                    Time.timeScale = 5.0f;
                else
                    Time.timeScale = lastTimeScale;
            }
            if (Input.GetKeyDown(keyTimeAdd1)) {
                Time.timeScale += 0.1f;
                lastTimeScale = Time.timeScale;
            }
            if (Input.GetKeyDown(keyTimeSub1)) {
                Time.timeScale -= 0.1f;
                lastTimeScale = Time.timeScale;
            }
            if (Input.GetKeyDown(keyTimeSub5)) {
                Time.timeScale -= 0.5f;
                lastTimeScale = Time.timeScale;
            }
            if (Input.GetKeyDown(keyTimeAdd5)) {
                Time.timeScale += 0.5f;
                lastTimeScale = Time.timeScale;
            }
            if (Time.timeScale < 0)
                Time.timeScale = 0;

            #endregion
        }

        #endregion

        #region METHODS

        #region Setup

        private void Init()
        {
            if (freeCamera) {
                LoggerInstance.Msg("Free camera is already injected.");
            }
            else {
                hudUI = GameObject.Find(Constants.HUD_CAMERA_PATH);
                uidUI = GameObject.Find(Constants.ID_CANVAS_PATH);
                SetupFreecam();
                SetupEntity();

                Instance = this;
                LoggerInstance.Msg("Free camera initialized.");
            }
        }

        private static void SetupFreecam()
        {
            mainCamera = GameObject.Find(Constants.MAIN_CAMERA_PATH);
            mainCameraCam = GameObject.Find(Constants.MAIN_CAMERA_PATH).GetComponent<Camera>();
            freeCamera = GameObject.Instantiate(mainCamera);
            mainCamera.SetActive(false);
            mainCamera.SetActive(true);

            freecamController = freeCamera.AddComponent<FreecamController>();
            freecamUI = new GameObject().AddComponent<FreecamUI>();

            freeCameraCam = freeCamera.GetComponent<Camera>();
            freecamController.Cam = freeCameraCam;
            freecamUI.c = freecamController;
            freecamController.ui = freecamUI;
            //GameObject.Destroy(freecamera.GetComponent<CinemachineBrain>());
            //GameObject.Destroy(freecamera.GetComponent<CinemachineExternalCamera>());

            freeCamera.SetActive(false);
        }

        private void SetupEntity()
        {
            entityRoot = GameObject.Find(Constants.ENTITY_ROOT_PATH).transform;
            avatarRoot = GameObject.Find(Constants.AVATAR_ROOT_PATH).transform;
            avatarController = CreateAvatarController(FindAvatarByActiveState(true));
        }

        #endregion

        /// <summary>
        /// Setup LOD loading functionality.
        /// </summary>
        public void SetupLodLoader()
        {
            avatarController = FindOrCreateAvatarController();
            if (avatarController == null) return;

            avatarSavedPos = avatarController.Position;
            avatarSavedRot = avatarController.Rotation;
            avatarController.SetParent(freeCamera.transform);

            avatarController.Root.gameObject.SetActive(false);
            avatarController.ToggleAnimator(false);
            avatarController.ToggleRender(false);

            LoggerInstance.Msg("LOD loader ready.");
            isUsingAvatarLodLoader = true;
        }


        /// <summary>
        /// Unset LOD loading functionality.
        /// </summary>
        public void UnsetLodLoader()
        {
            if (!isUsingAvatarLodLoader) return;

            RestoreAvatarPosition();
            avatarController.SetParent(avatarRoot);

            avatarController.ToggleRender(true);
            avatarController.ToggleAnimator(true);
            avatarController.Root.gameObject.SetActive(true);

            LoggerInstance.Msg("LOD loader unset.");
            isUsingAvatarLodLoader = false;
        }

        public static void LODFollow()
        {
            if (!isUsingAvatarLodLoader) return;
            if(avatarController != null) avatarController.Follow(freeCamera.transform, freeCamera.transform.forward * freecamController.LODBias);
        }

        private static void RestoreAvatarPosition()
        {
            Vector3 targetPos = new Vector3(avatarSavedPos.x, avatarSavedPos.y + 0.05f, avatarSavedPos.z);
            avatarController.RbPosition = targetPos;
            avatarController.RbRotation = avatarSavedRot;
            avatarController.Position = targetPos;
            avatarController.Rotation = avatarSavedRot;
        }

        /// <summary>
        /// Find first active or inactive avatar root transform.
        /// </summary>
        /// <param name="activeState">desired active state</param>
        /// <returns>root transform of the avatar</returns>
        public static Transform FindAvatarByActiveState(bool activeState = true)
        {
            if (avatarRoot == null) return null;

            for (int i = 0; i < avatarRoot.childCount; i++) {
                Transform avatarTr = avatarRoot.GetChild(i);
                if (avatarTr.gameObject.activeSelf == activeState) {
                    return avatarTr;
                }
            }
            return null;
        }

        private void ActivateFreecam()
        {
            freecamController.Active = true; // This will call methods such as SetupLodLoader() if necessary.
            freeCamera.SetActive(true);
            mainCamera.SetActive(false);
            //if (freecamController.SyncAvatarRoot) SetupLodLoader();
        }

        private void DeactivateFreecam()
        {
            freecamController.Active = false;
            mainCamera.SetActive(true);
            freeCamera.SetActive(false);
            //UnsetLodLoader();

        }

        /// <summary>
        ///  Returns current AvatarController for first active avatar found. 
        ///  Returns a new avatar controller if necessary.
        /// </summary>
        /// <returns></returns>
        private AvatarController FindOrCreateAvatarController()
        {
            var controller = avatarController;

            Transform avatarTr = FindAvatarByActiveState(true);
            LoggerInstance.Msg("Updating avatar...");

            // Create new if this is first or different controller.
            if (controller == null || avatarTr != prevAvatarTr) {
                controller = CreateAvatarController(avatarTr);
            }

            return controller;
        }

        /// <summary>
        /// Returns a new avatar controller based on provided avatar transform.
        /// Avatar controller is used as an avatar definition.
        /// </summary>
        /// <param name="avatarTr">root transform of the avatar</param>
        /// <returns></returns>
        private AvatarController CreateAvatarController(Transform avatarTr)
        {
            LoggerInstance.Msg("Creating AvatarController...");
            prevAvatarTr = avatarTr;
            return new AvatarController(avatarTr);
        }

        /*
        public static void ClearComponents(int exceptIdx, GameObject gameObject)
        {
            Component[] components = gameObject.GetComponents<Component>();
            int len = components.Length;
            if (exceptIdx < 0) exceptIdx = 0;
            if (exceptIdx > len-1) exceptIdx = len-1;

            for(int i=0; i<len; i++) {
                if(i != exceptIdx) {
                    GameObject.Destroy(components[i]);
                }
            }
        }
        */


        /*
        public static Component GetComponentAtIndex(int idx, GameObject gameObject)
        {
            Component[] components = gameObject.GetComponents<Component>();
            int maxIdx = components.Length - 1;
            if (idx < 0) idx = 0;
            if (idx > maxIdx) idx = maxIdx;
            return components[idx];
        }

        public static MonoBehaviour GetMonoAtIndex(int idx, GameObject gameObject)
        {
            MonoBehaviour[] components = gameObject.GetComponents<MonoBehaviour>();
            int maxIdx = components.Length - 1;
            if (idx < 0) idx = 0;
            if (idx > maxIdx) idx = maxIdx;
            return components[idx];
        }
        */

        /*
        public static Component GetComponentByName(string name, GameObject gameObject)
        {
            Component[] components = gameObject.GetComponents<Component>();
            for (int i = 1; i < components.Length; i++)
            {
                Component component = components[i];
                if (component.ToString() == name)
                {
                    return component;
                }
            }
            return null;
        }
        */
        #endregion
    }
}
