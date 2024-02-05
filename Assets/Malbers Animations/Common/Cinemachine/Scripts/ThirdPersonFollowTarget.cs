using Cinemachine;
using MalbersAnimations.Scriptables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MalbersAnimations
{
    [AddComponentMenu("Malbers/Camera/Third Person Follow Target (Cinemachine)")]
    [DefaultExecutionOrder(-500)]
    public class ThirdPersonFollowTarget : MonoBehaviour
    {
        [Tooltip("Cinemachine Brain Camera")]
        public CinemachineBrain Brain;

        [Tooltip("Update mode for the Aim Logic")]
        public UpdateType updateMode = UpdateType.FixedUpdate;

        /// <summary> List of all the scene Third Person Follow Cameras (using the same brain)! </summary>
        public static HashSet<ThirdPersonFollowTarget> TPFCameras;

        /// <summary>  Active Camera using the same Cinemachine Brain </summary>
        public ThirdPersonFollowTarget ActiveCamera { get; set; }

        [Tooltip("What object to follow")]
        public TransformReference Target;

        public Transform CamPivot;

        [Tooltip("Camera Input Values (Look X:Horizontal, Look Y: Vertical)")]
        public Vector2Reference look = new();


        [Tooltip("Invert X Axis of the Look Vector")]
        public BoolReference invertX = new();
        [Tooltip("Invert Y Axis of the Look Vector")]
        public BoolReference invertY = new();

        [Tooltip("Default Camera Distance set to the Third Person Cinemachine Camera")]
        public FloatReference CameraDistance = new(6);

        [Tooltip("Multiplier to rotate the X Axis")]
        public FloatReference XMultiplier = new(1);
        [Tooltip("Multiplier to rotate the Y Axis")]
        public FloatReference YMultiplier = new(1);

        [Tooltip("How far in degrees can you move the camera up")]
        public FloatReference TopClamp = new(70.0f);

        [Tooltip("How far in degrees can you move the camera down")]
        public FloatReference BottomClamp = new(-30.0f);

        [Tooltip("Lerp Rotation to smooth out the movement of the camera while rotating.")]
        public FloatReference LerpRotation = new(15f);

        private float InvertX => invertX.Value ? -1 : 1;
        private float InvertY => invertY.Value ? 1 : -1;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;
        private const float _threshold = 0.00001f;
        //  private bool isActive;

        private ICinemachineCamera Cam;


        // Start is called before the first frame update
        void Awake()
        {
            if (CamPivot == null)
            {
                CamPivot = new GameObject("CamPivot").transform;
                CamPivot.parent = transform;
                CamPivot.ResetLocal();
            }
            CamPivot.parent = null;

            CamPivot.hideFlags = HideFlags.HideInHierarchy;

            //Find the Cinemachine camera
            if (TryGetComponent(out Cam))
                Cam.Follow = CamPivot.transform;

            if (Brain == null) Brain = FindObjectOfType<CinemachineBrain>();

            var TPF = this.FindComponent<Cinemachine3rdPersonFollow>();
            if (TPF != null)
                TPF.CameraDistance = CameraDistance;


            transform.position = CamPivot.position;

            //transform.position = Vector3.zero;

        }


        private void OnEnable()
        {
            Brain.m_CameraActivatedEvent.AddListener(CameraChanged);
            CameraPosition();
            StartCameraLogic();

            if (TPFCameras == null) TPFCameras = new(); //Initialize the Cameras
            TPFCameras.Add(this);

          //  Debug.Log($"TPF Camera Count, {TPFCameras.Count}");
        }

        private void OnDisable()
        {
            Brain.m_CameraActivatedEvent.RemoveListener(CameraChanged);
            StopAllCoroutines();

            transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            TPFCameras.Remove(this);
        }


        private void StartCameraLogic()
        {
            if (ActiveCamera == this)
            {
                if (updateMode == UpdateType.FixedUpdate)
                    StartCoroutine(AfterPhysics());
                else
                {
                    StartCoroutine(AfterLateUpdate());
                }
            }
        }

        readonly WaitForFixedUpdate mWaitForFixedUpdate = new();
        readonly WaitForEndOfFrame mWaitForLateUpdate = new();

        IEnumerator AfterPhysics()
        {
            while (true)
            {
                CameraLogic(Time.fixedDeltaTime);
                yield return mWaitForFixedUpdate;
            }
        }

        IEnumerator AfterLateUpdate()
        {
            while (true)
            {
                CameraLogic(Time.deltaTime);
                yield return mWaitForLateUpdate;
            }
        }

        private void CameraLogic(float deltaTime)
        {
            CameraPosition();

            if (ActiveCamera == this)
            {
                CameraRotation(deltaTime);
            }
            else
            {
                CheckRotation();
            }

            //Update all the Sleep  cameras in the scene
            foreach (var c in TPFCameras)
            {
                if (c == ActiveCamera || c.Brain != Brain) continue; //Skip the ones using the same brain

                c.CamPivot.SetPositionAndRotation(ActiveCamera.CamPivot.position, ActiveCamera.CamPivot.rotation);
                c._cinemachineTargetYaw = ActiveCamera._cinemachineTargetYaw;
                c._cinemachineTargetPitch = ActiveCamera._cinemachineTargetPitch;
            }
        }


        private void CameraChanged(ICinemachineCamera newCam, ICinemachineCamera exit)
        {
            var isActive = Cam == newCam;

          //  Debug.Log($"This <B>[{name}]</B> Camera Changed: Brain{Brain.name}, {newCam.Name}", newCam.VirtualCameraGameObject);


            //TPFCameras.Add(IsThirdPersonFollow);

            if (isActive)
            {
                ActiveCamera = this;
                StartCameraLogic();
            }
            else
            {
                //Find the current active Third Person Follow
            //var IsThirdPersonFollow = newCam.VirtualCameraGameObject.GetComponent<ThirdPersonFollowTarget>();
               // ActiveCamera = IsThirdPersonFollow;
                StopAllCoroutines();
            }

           // Debug.Log($"TPF Camera Count, {TPFCameras.Count}");
        }

        private void CheckRotation()
        {
            CameraPosition();
            var EulerAngles = Brain.transform.eulerAngles; //Get the Brain Rotation to save the movement 

            _cinemachineTargetYaw = ClampAngle(EulerAngles.y, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = EulerAngles.x > 180 ? EulerAngles.x - 360 : EulerAngles.x; //HACK!!!
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            CamPivot.rotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);
        }
       



        public void SetLookX(float x) => look.x = x;
        public void SetLookY(float y) => look.y = y;
        public void SetLook(Vector2 look) => this.look.Value = look;

        private void CameraPosition()
        {
            if (Target.Value)
            {
                CamPivot.transform.position = Target.position;
            }
        }

        private void CameraRotation(float deltaTime)
        {
            // if there is an input and camera position is not fixed
            if (look.Value.sqrMagnitude >= _threshold)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = 100 * deltaTime;

                _cinemachineTargetYaw += look.x * deltaTimeMultiplier * InvertX * XMultiplier;
                _cinemachineTargetPitch += look.y * deltaTimeMultiplier * InvertY * YMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            var TargetRotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);
            CamPivot.rotation = Quaternion.Lerp(CamPivot.rotation, TargetRotation, deltaTime * LerpRotation); //NEEDED FOR SMOOTH CAMERA MOVEMENT
        }

        public void SetTarget(Transform target) => Target.Value = target;

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

#if UNITY_EDITOR
        private void Reset()
        {
            Target.UseConstant = false;
            Target.Variable = MTools.GetInstance<TransformVar>("Camera Target");


            if (CamPivot == null)
            {
                CamPivot = new GameObject("Pivot").transform;
                CamPivot.parent = transform;
                CamPivot.ResetLocal();
            }
        }
#endif
    }
}
