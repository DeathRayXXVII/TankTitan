using GameSpecific.Tank.Data;
using UnityEngine;

namespace GameSpecific.Tank
{
    public abstract class TankController : MonoBehaviour
    {
        [SerializeField] protected TankData tankData;
        [SerializeField] protected TankShooting tankShooting;
        
        [SerializeField] protected GameObject barrel;
        
        protected abstract void Move();
        protected abstract void Turn();
        protected abstract void Death();
        protected abstract void ResetTank();
    }
}
