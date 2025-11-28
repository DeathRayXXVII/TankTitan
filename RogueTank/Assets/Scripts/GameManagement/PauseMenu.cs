using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace GameManagement
{
    public class PauseMenu : MonoBehaviour
    {
        public bool gameIsPaused;
        public bool delayPause;
        public bool bypassDelay;
        public float delay = 1f;
        [SerializeField] private PlayerInput playerInput;
        //[SerializeField] private InputActionReference pauseAction;
        public UnityEvent startEvent, pauseEvent, resumeEvent;

        private bool GameIsPaused
        {
            get => gameIsPaused;
            set => gameIsPaused = value;
        }
        private void Start()
        {
            startEvent.Invoke();
            DelayPause();
        }

        private void Update()
        {
            if (!bypassDelay) return;
            //yield return new WaitForSeconds(delay);
            delayPause = false;
            GameIsPaused = false;
            Time.timeScale = 1f;
        }
        
        public void OnPause()
        {
            if (GameIsPaused)
            {
                playerInput.SwitchCurrentActionMap("Player");
                resumeEvent.Invoke();
                StartResume();
            }
            else
            {
                playerInput.SwitchCurrentActionMap("UI");
                pauseEvent.Invoke();
                StartPause();
            }
        }

        public void StartResume()
        {
            GameIsPaused = false;
            Time.timeScale = 1f;
        }

        public void StartPause()
        {
            GameIsPaused = true;
            Time.timeScale = 0f;
        }
        public void BypassDelay()
        {
            bypassDelay = true;
        }
        
        public void BypassDelayOff()
        {
            bypassDelay = false;
        }
        
        private void StartDelayPause()
        {
            delayPause = true;
            if (delayPause)
            {
                StartCoroutine(StartDelay());
            }
        }

        private void DelayPause()
        {
            if (delayPause)
            {
                StartCoroutine(StartDelay());
            }
        }

        private IEnumerator StartDelay()
        {
            yield return new WaitForSeconds(delay);
            delayPause = false;
            GameIsPaused = true;
            Time.timeScale = 0f;
        }

    }
}
