using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace GameManagement
{
    public class ApplicationBehavior : MonoBehaviour
    {
        [SerializeField] private ScreenFader fader;
        public void StartGame(int sceneToLoad)
        {
            StartCoroutine(StartGameCoroutine(sceneToLoad));
        }
        private IEnumerator StartGameCoroutine(int sceneToLoad)
        {
            if (fader != null) yield return StartCoroutine(fader.FadeIn());
            SceneManager.LoadScene(sceneToLoad);
            Time.timeScale = 1f;
        }
        public void StartGame(string sceneToLoad)
        {
            StartCoroutine(StartGameCoroutine(sceneToLoad));
        }
        private IEnumerator StartGameCoroutine(string sceneToLoad)
        {
            if (fader != null) yield return StartCoroutine(fader.FadeIn());
            SceneManager.LoadScene(sceneToLoad);
            Time.timeScale = 1f;
        }
        public void QuitApplication()
        {
            Application.Quit();
// #if UNITY_EDITOR
//             UnityEditor.EditorApplication.isPlaying = false;
// #else
// 		    Application.Quit();
//             SteamClient.Shutdown();
//             
// #endif
        }
    }
}
