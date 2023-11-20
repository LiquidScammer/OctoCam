using UnityEngine;
namespace OctoCam
{
    public class ButtonHandler : MonoBehaviour
    {
        //ty stricker elmishh(cyn) and kyle
        public string button;
        private float touchTime = 0f;
        private const float debounceTime = 0.25f;
        Main smth = null;
        void Start()
        {
            button = transform.name;
            smth = GameObject.Find("OctoCam").GetComponent<Main>();
        }
        void OnTriggerEnter(Collider other)
        {
            if (touchTime + debounceTime >= Time.time) return;
            if (other.TryGetComponent(out GorillaTriggerColliderHandIndicator component))
            {
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(211, component.isLeftHand, 0.12f);
                GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
                if (button == "First Person")
                {
                    Debug.Log("fp button pressed");
                    
                    smth.FirstPerson();
                }
                if (button == "Lock Movement")
                {
                    Debug.Log("lock movemnt button pressed");
                    Main.moveLock = !Main.moveLock;
                }
                if (button == "Lock To Player")
                {
                    Debug.Log("Lock to player button pressed");
                    smth.lockToMonke();
                }
                if (button == "Look At Player")
                {
                    Debug.Log("look at player button pressed buddy");
                    smth.LookAtPlayer();
                }
                if (button == "Third Person")
                {
                    Debug.Log("im going insane but third person buthfdhfdjkfsdhsdfjkh opreseed");
                    smth.ThirdPerson();
                }
            }
        }
    }
}
//im in a call with a kid called soggy bread and they wanted to be in this code so here you go soggy bread  