using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ScreenFader : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        [SerializeField] private float duration;
        
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
        }
        
        public void StartFadeOut()
        {
            StartCoroutine(FadeOut());
        }
        
        public void StartFadeIn()
        {
            StartCoroutine(FadeIn());
        }
        
        public IEnumerator FadeOut()
        {
            var elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                _canvasGroup.alpha = Mathf.Clamp01(elapsed / duration);
                yield return null;
            }
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
        }
        
        public IEnumerator FadeIn()
        {
            var elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                _canvasGroup.alpha = 1f - Mathf.Clamp01(elapsed / duration);
                yield return null;
            }
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}
