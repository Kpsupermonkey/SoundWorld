using UnityEngine;
using FMOD. Studio;
using FMODUnity;

namespace Audio
{


    public class Footsteps : MonoBehaviour
    {
        [Header("FMOD")] public string footstepEvent = "event:/Footstep";

        [Header("Settings")] public float walkSoundDelay;
        public float runSoundDelay;
        public Transform groundCheck;
        public float groundCheckRadius = 0.3f;
        public LayerMask groundMask;

        private string currentGroundTag = "Default";

        private float _timer;

        public bool isMoving;
        private bool isRunning;
        private bool countdownNeeded;

        private void Update()
        {
            Debug.Log("Timer = " + _timer);
            Debug.Log("Tryin tae move? = " + isMoving);
            Debug.Log("Countdown needed? = " + countdownNeeded);

            if (!isMoving) return;
            
            if (countdownNeeded)
            {
                _timer += Time.deltaTime;
                switch (isRunning)
                {
                    case false when _timer >= walkSoundDelay:
                    case true when _timer >= runSoundDelay:
                        countdownNeeded = false;
                        break;
                }
            }

            else
            {
                PlayFootstep();
                
                countdownNeeded = true;
                _timer = 0;
            }
        }

        private void PlayFootstep()
        {
            Collider[] hits = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, (int) groundMask);
           
            if (hits.Length > 0)
            {
                currentGroundTag = hits[0].tag;
            }
            else
            {
                currentGroundTag = "Default";
            }
            
            EventInstance footstep = RuntimeManager.CreateInstance(footstepEvent);

            // simpler logic - it's either road or it's not
            if (currentGroundTag == "Default") footstep.setParameterByName("SurfaceType", 0);
            else footstep.setParameterByName("SurfaceType", value: Getgroundvalue(currentGroundTag));
            
            RuntimeManager.AttachInstanceToGameObject(footstep, transform, rigidBody:GetComponent<Rigidbody>());
            
            footstep. start();
            footstep.release();
            
            _timer = 0;
        }

        private float Getgroundvalue(string tag)
        {
            switch (tag)
            {
                case "Staircase":return 0f;

                case "Tile": return 1f;

                default: return 0f;
            }
        }
    }
}
