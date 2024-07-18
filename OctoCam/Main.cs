using BepInEx;
using GorillaNetworking;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
namespace OctoCam
{
    [BepInPlugin("octoburr.octocam", "OctoCam", "1.0.0")]
    public class Main : BaseUnityPlugin
    {
        public static GameObject CameraModel;
        Transform TPC;      
        GameObject buttons;
        public GameObject firstPersonButton;
        public GameObject thirdPersonButton;
        public GameObject lockToPlayer;
        public GameObject lockMovment;
        public GameObject lookAtPlayer;
        bool GUIEnabled = false;
        //bool DebugEnabled = false;
        public static bool moveLock = false;
        bool isFirstPerson = false;
        bool isThirdPerson = false;
        bool isLooking;
        void Start() => Utilla.Events.GameInitialized += OnGameInitialized;
        void OnGameInitialized(object sender, EventArgs e)
        {
            AssetBundle bundle = LoadAssetBundle("OctoCam.Assets.camera");
            GameObject asset = bundle.LoadAsset<GameObject>("camera");
            CameraModel = Instantiate(asset);

            TPC = GorillaTagger.Instance.thirdPersonCamera.transform.GetChild(0);
            GameObject.Destroy(TPC.GetChild(0).gameObject);
            TPC.transform.parent = CameraModel.transform;
            TPC.localPosition = new Vector3(-0.0007f, -0.0003f, 0.0007f);
            TPC.localRotation = Quaternion.identity;
            TPC.GetComponent<Camera>().fieldOfView = 90;
            TPC.GetComponent<Camera>().nearClipPlane = 0.01f;

            CameraModel.transform.position = new Vector3(-65, 12, -83);
            CameraModel.transform.localRotation = Quaternion.Euler(0, 90, 0);
            CameraModel.name = "OctoCam";

            buttons = CameraModel.transform.GetChild(0).transform.GetChild(0).gameObject;

            firstPersonButton = buttons.transform.GetChild(0).gameObject;
            lockMovment = buttons.transform.GetChild(1).gameObject;
            lockToPlayer = buttons.transform.GetChild(2).gameObject;
            lookAtPlayer = buttons.transform.GetChild(3).gameObject;
            thirdPersonButton = buttons.transform.GetChild(4).gameObject;

            firstPersonButton.layer = 18;
            lockMovment.layer = 18;
            lockToPlayer.layer = 18;
            lookAtPlayer.layer = 18;
            thirdPersonButton.layer = 18;

            firstPersonButton.AddComponent<ButtonHandler>();
            lockMovment.AddComponent<ButtonHandler>();
            lockToPlayer.AddComponent<ButtonHandler>();
            lookAtPlayer.AddComponent<ButtonHandler>();
            thirdPersonButton.AddComponent<ButtonHandler>();

            var smth = CameraModel.AddComponent<Main>();
            var Grab = CameraModel.AddComponent<GrabHandler>();

            PhotonNetworkController.Instance.disableAFKKick = true;
        }
        void Update()
        {
            if (ControllerInputPoller.instance.leftControllerPrimaryButton)
            {
                onMove();
                outOfInvis();
                isLooking = false;
            }
            if(Keyboard.current.tabKey.wasPressedThisFrame)
            {
                GUIEnabled = !GUIEnabled;
            }
            /*if (Keyboard.current.ctrlKey.wasPressedThisFrame)
            {
                DebugEnabled = !DebugEnabled;
            }*/
            if (isLooking)
            {
                CameraModel.transform.LookAt(Camera.main.transform);
            }
        }
        public void FirstPerson()
        {
            Invis();
            CameraModel.transform.SetParent(Camera.main.transform);
            CameraModel.transform.localPosition = Vector3.zero;
            CameraModel.transform.localRotation = Quaternion.identity;
            isFirstPerson = true;
        }
        void outOfInvis()
        {
            CameraModel.transform.GetChild(0).gameObject.SetActive(true);
        }
        void Invis()
        {
            CameraModel.transform.GetChild(0).gameObject.SetActive(false);
        }
        public void ThirdPerson()
        {
            Invis();
            CameraModel.transform.SetParent(Camera.main.transform);
            CameraModel.transform.localPosition = new Vector3(0.2f, 0.1618f, -1.2727f);
            CameraModel.transform.localRotation = Quaternion.identity;
            isThirdPerson = true;
        }
        public void lockToMonke()
        {
            CameraModel.transform.SetParent(Camera.main.transform);
            Invis();
        }
        public void LookAtPlayer()
        {
            //ty kyle for helping
            CameraModel.transform.parent = null;
            isLooking = !isLooking;
        }
                public static void SetNightTime()
        {
            BetterDayNightManager.instance.SetTimeOfDay(0);
        }

        public static void SetEveningTime()
        {
            BetterDayNightManager.instance.SetTimeOfDay(7);
        }

        public static void SetMorningTime()
        {
            BetterDayNightManager.instance.SetTimeOfDay(1);
        }

        public static void SetDayTime()
        {
            BetterDayNightManager.instance.SetTimeOfDay(3);
        }

        public static void SetRain()
        {
            for (int i = 1; i < BetterDayNightManager.instance.weatherCycle.Length; i++)
            {
                BetterDayNightManager.instance.weatherCycle[i] = BetterDayNightManager.WeatherType.Raining;
            }
        }

        public static void SetNoRain()
        {
            for (int i = 1; i < BetterDayNightManager.instance.weatherCycle.Length; i++)
            {
                BetterDayNightManager.instance.weatherCycle[i] = BetterDayNightManager.WeatherType.None;
            }
        }
        private void OnGUI()
        {
            if (GUIEnabled)
            {
                GUI.Box(new Rect(10, 10, 100, 195), "Camera UI");
                if (GUI.Button(new Rect(15, 40, 90, 20), "First Person"))
                {
                    FirstPerson();
                }
                if (GUI.Button(new Rect(15, 70, 90, 20), "Third Person"))
                {
                    ThirdPerson();
                }
                if (GUI.Button(new Rect(15, 100, 90, 35), "Look At\nPlayer"))
                {
                    LookAtPlayer();

                }
                if (GUI.Button(new Rect(15, 145, 90, 35), "Lock to\nPlayer"))
                {
                    lockToMonke();
                }
                moveLock = GUI.Toggle(new Rect(15, 180, 90, 35), moveLock, "Move Lock");
                                if (GUI.Button(new Rect(15, 300, 140, 40), "Set Morning Time"))
                {
                    SetMorningTime();
                }
                if (GUI.Button(new Rect(15, 360, 140, 40), "Set Day Time"))
                {
                    SetDayTime();
                }
                if (GUI.Button(new Rect(15, 420, 140, 40), "Set Evening Time"))
                {
                    SetEveningTime();

                }
                if (GUI.Button(new Rect(15, 480, 140, 40), "Set Night Time"))
                {
                    SetNightTime();
                }
                if (GUI.Button(new Rect(15, 540, 140, 40), "Set Rain"))
                {
                    SetRain();
                }
                if (GUI.Button(new Rect(15, 600, 140, 40), "Set No Rain"))
                {
                    SetNoRain();
                }
            }
            /*if (DebugEnabled)
            {
                //test stuff used for debuging
                GUI.Box(new Rect(150, 10, 100, 120), "Debug Menu");
                if (GUI.Button(new Rect(155, 40, 90, 20), "invis"))
                {
                    Invis();
                }
                if (GUI.Button(new Rect(155, 70, 90, 20), "visible"))
                {
                    outOfInvis();
                }
                if (GUI.Button(new Rect(155, 100, 90, 20), "move"))
                {
                    onMove();
                }
            }*/
        }
        void onMove()
        {
            if (!moveLock)
            {
                CameraModel.transform.SetParent(Camera.main.transform, true);
                CameraModel.transform.localPosition = new Vector3(0, 0, 1);
                CameraModel.transform.parent = null;
                outOfInvis();
            }
            if (isFirstPerson || isThirdPerson)
            {
                CameraModel.transform.SetParent(Camera.main.transform, true);
                CameraModel.transform.localPosition = new Vector3(0, 0, 1);
                CameraModel.transform.parent = null;
                outOfInvis();
            }
        }
        public AssetBundle LoadAssetBundle(string  path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            AssetBundle bundle = AssetBundle.LoadFromStream(stream);
            stream.Close();
            return bundle;
        }
    }
}
