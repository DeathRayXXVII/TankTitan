using System.Collections;
using GameSpecific.Tank.Data;
using UnityEngine;

namespace GameSpecific.Tank
{
    public class BombBehavior : MonoBehaviour
    {
        [SerializeField] private BulletData bombData;
        [SerializeField] private LayerMask destructibleLayer;
        [SerializeField] private GameAction playerHitAction, enemyHitAction;
        private float _counterNum;
        private WaitForSeconds _wfsObj;
    
        public void StartExplosion()
        {
            Invoke(nameof(Explode), bombData.delay);
            StartCoroutine(Explosion());
        }
        
        private void Explode()
        {
            var colliders = Physics.OverlapSphere(transform.position, bombData.explosionRadius);
            foreach (var hit in colliders)
            {
                switch (hit.gameObject.layer)
                {
                    case 6:
                        playerHitAction.RaiseAction();
                        hit.gameObject.SetActive(false);
                        gameObject.SetActive(false);
                        break;
                    case 7:
                        enemyHitAction.RaiseAction();
                        hit.gameObject.SetActive(false);
                        gameObject.SetActive(false);
                        break;
                }
                if ((destructibleLayer.value & (1 << hit.gameObject.layer)) == 0) continue;
                hit.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
        }

        private IEnumerator Explosion()
        {
            yield return _wfsObj;
            while (_counterNum > 0)
            {
                
                yield return _wfsObj;
                _counterNum -= Time.deltaTime;
            }
            gameObject.SetActive(false);
            _counterNum = 3.0f;
        }
    
        public void ResetBomb()
        {
            gameObject.SetActive(false);
        }
    }
}
