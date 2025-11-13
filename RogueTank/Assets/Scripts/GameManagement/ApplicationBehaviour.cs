using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagement
{
    public class ApplicationBehavior : MonoBehaviour
    {
        public int sceneToLoad;

        public void StartGame()
        {
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
