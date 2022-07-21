using System;
using UnhollowerRuntimeLib;
using UnityEngine;
using static CameraWorks.Constants;

namespace CameraWorks
{
    public class AvatarController
    {

        #region FIELDS

        // Avatar movement transform.
        private Transform root;
        private Rigidbody rb;

        // Avatar under OffsetDummy and its notable skinned mesh renderers.
        private Transform dummy;
        private Animator animator;
        private Renderer body;
        private Renderer brow;
        private Renderer face;
        private Renderer faceEye;

        #endregion

        #region PROPERTIES

        public Transform Root { get => root; }
        public Rigidbody Rb { get => rb;  }
        public Transform Dummy { get => dummy; }
        public Animator AvatarAnimator { get => animator; }
        public Vector3 Position {
            get => root.transform.position;
            set => root.transform.position = value;
        }
        public Quaternion Rotation
        {
            get => root.transform.rotation;
            set => root.transform.rotation = value;
        }
        public Vector3 RbPosition
        {
            get => rb.position;
            set => rb.position = value;
        }
        public Quaternion RbRotation
        {
            get => rb.rotation;
            set => rb.rotation = value;
        }
        public Vector3 SavedPos { get; set; }
        public Quaternion SavedRot { get; set; }


        #endregion


        public AvatarController(Transform rootTr)
        {
            this.root = rootTr;
            rb = this.root.GetComponent<Rigidbody>();
            dummy = FindDummy();
            if (dummy) {
                animator = dummy.GetComponent<Animator>();
                SetupAvatarRenderers(this.dummy); 
            }
        }

        public AvatarController(AvatarController controller)
        {
            this.root = controller.Root;
            this.rb = controller.Rb;
            this.dummy = controller.Dummy;
            animator = controller.AvatarAnimator;
            SetupAvatarRenderers(this.dummy);
        }

        #region METHODS

        // Sync avatar transform with target transform.
        public void Follow(Transform target, Vector3 bias)
        {
            root.position = target.position + bias;
            root.rotation = Quaternion.identity;
        }

        public void SetParent(Transform parent, bool worldPositionStays = true)
        {
            root.transform.SetParent(parent, worldPositionStays);
        }

        // Show/hide avatar skinned mesh renderers.
        public void ToggleRender(bool activeState)
        {
            if (body) body.enabled = activeState;
            if (brow) brow.enabled = activeState;
            if (face) face.enabled = activeState;
            if (faceEye) faceEye.enabled = activeState;
            dummy.transform.GetChild(0).gameObject.SetActive(activeState);
        }

        public void ToggleAnimator(bool activeState)
        {
            if (animator) animator.enabled = activeState;
        }

        private void SetupAvatarRenderers(Transform dummy)
        {
            body = dummy.FindChild(RD_BODY_NAME).GetComponent<Renderer>();
            brow = dummy.FindChild(RD_BROW_NAME).GetComponent<Renderer>();
            face = dummy.FindChild(RD_FACE_NAME).GetComponent<Renderer>();
            faceEye = dummy.FindChild(RD_FACEEYE_NAME).GetComponent<Renderer>();
        }

        private void UnsetAvatarRenderers()
        {
            body = null;
            brow = null;
            face = null;
            faceEye = null;
        }

        private Transform FindDummy()
        {
            return root.Find("OffsetDummy").GetChild(0);
        }

        private Transform FindRig()
        {
            if (!dummy) return null;
            return dummy.transform.GetChild(0);
        }

        #endregion
    }
}