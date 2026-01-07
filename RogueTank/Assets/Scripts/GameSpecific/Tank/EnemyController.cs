using System.Collections;
using GameSpecific.Tank.Data;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace GameSpecific.Tank
{
    public class EnemyController : TankController
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private GameAction startTankAction, stopTankAction;
        [SerializeField] private Transform target;
        [SerializeField] private float stoppingDistance = 1f;
        [SerializeField] private float walkPointRange;
        [SerializeField] private LayerMask groundLayer, targetLayer;
        [SerializeField] private float searchAngleRange = 180;
        [SerializeField] private float sightRange;
        [SerializeField] private float attackRange;
        public Vector3 startingPosition;
    
        private float _pathUpdateDeadline;
        private Vector3 _walkPoint;
        private bool _walkPointSet;
        private bool _isRotating;
        private Vector3 _direction;
        private bool _alreadyAttacked;
        private bool _bombTriggered;
        private bool _canMove = true;
        public float attackTimer;
        private float _bombTimer;


        private void Awake()
        {
            agent = agent == null ? GetComponent<NavMeshAgent>() : agent;
            var player = GameObject.FindWithTag("Player");
            if (player != null) target = player.transform;
        }

        private void Start()
        {
            if (agent == null || tankData == null || tankData.stats == null) return;
            agent.speed = tankData.stats.moveSpeed;
            agent.stoppingDistance = stoppingDistance;

        }
    
        private void OnEnable()
        {
            startTankAction.RaiseEvent += TankStart;
            stopTankAction.RaiseEvent += TankStop;
        }

        private void TankStart(GameAction _)
        {
            // Debug.Log("StartTank");
            EnemyTankStats enemyTankStats = tankData.stats as EnemyTankStats;
        
            if (enemyTankStats != null && enemyTankStats.isStationary)
            {
                // Debug.Log("isStationary");
                agent.isStopped = false;
                _canMove = false;
                StartCoroutine(TankStationary());
            }
            else
            {
                // Debug.Log("isMoving");
                agent.isStopped = false;
                _canMove = true;
                StartCoroutine(TankMovement());
            }
        }
    
        private void TankStop(GameAction _)
        {
            Debug.Log("StopTank");
            agent.isStopped = true;
            _canMove = false;
            StopAllCoroutines();
        }
    
        private IEnumerator TankMovement()
        {
            while (_canMove)
            {
              attackTimer += Time.deltaTime;
              _bombTimer += Time.deltaTime;  
                Vector3 position = transform.position;
                bool playerInSightRange = Physics.CheckSphere(position, sightRange, targetLayer);
                bool playerInAttackRange = Physics.CheckSphere(position, attackRange, targetLayer);
                Patrolling();
                if (!CanSeePlayer() && playerInSightRange)
                {
                    //RandomRotateBarrel();
                    FindPlayer();
                }
                if (CanSeePlayer() && playerInAttackRange)
                {
                    RotateBarrel();
                    _isRotating = false;
                    if (attackTimer >= tankShooting.bulletData.timeBetweenShots)
                    {
                        AttackPlayer();
                        attackTimer = 0f;
                    }
                    
                }
                if (CanSeePlayer() && playerInSightRange)
                {
                    FollowPlayer();
                    RotateBarrel();
                    _isRotating = false;
                    //StopCoroutine(RotateTimer());
                    if (_bombTriggered)
                    {
                        if (_bombTimer >= tankShooting.bombData.timeBetweenShots)
                        {
                            tankShooting.BombPreformed();
                            _bombTimer = 0f;
                        }
                    }
                }
                yield return null;
            }
        }
        private IEnumerator TankStationary()
        {
            while (!_canMove)
            {
                attackTimer += Time.deltaTime;
                if (!CanSeePlayer())
                {
                    FindPlayer();
                }
                else
                {
                    RotateBarrel();
                }
            
                if (CanSeePlayer())
                {
                    Debug.Log("CanSeePlayer");
                    _isRotating = false;
                    if (attackTimer >= tankShooting.bulletData.timeBetweenShots)
                    {
                        Debug.Log("AttackPlayer1");
                        AttackPlayer();
                        attackTimer = 0f;
                    }
                }
            
                yield return null;
            }
        }
    
        private void Patrolling()
        {
            if (!_walkPointSet)
            {
                SearchWalkPoint();
            }
            else
            {
                agent.SetDestination(_walkPoint);
            }

            Vector3 distanceToWalkPoint = transform.position - _walkPoint;

            if (distanceToWalkPoint.magnitude < 1f)
            {
                _walkPointSet = false;
            }
        }
    
        private void SearchWalkPoint()
        {
            float randomAngle = Random.Range(-searchAngleRange / 2, searchAngleRange / 2);
            float randomDistance = Random.Range(0, walkPointRange);
    
            Vector3 direction = Quaternion.Euler(0, randomAngle, 0) * transform.forward;
            _walkPoint = transform.position + direction * randomDistance;
    
            if (Physics.Raycast(_walkPoint, -transform.up, 2f, groundLayer))
            {
                _walkPointSet = true;
            }
        }
     
        private void FollowPlayer()
        {
            agent.SetDestination(target.position);
        }

        private void FindPlayer()
        {
            const int maxReflections = 1;
            int reflections = 0;

            var transform1 = tankShooting.fireTransform.transform;
            Vector3 rayDirection = transform1.up;
            Vector3 nextStartPosition = transform1.position;
            while (reflections <= maxReflections)
            {
                if (Physics.Raycast(nextStartPosition, rayDirection, out var hit))
                {
                    Debug.DrawRay(nextStartPosition, rayDirection * hit.distance,
                        reflections == 0 ? Color.red : Color.green);
                    nextStartPosition = hit.point;

                    if (hit.transform == target)
                    {
                        // If the ray hits the player, perform the desired action
                        _isRotating = false;
                        //StopCoroutine(RotateTimer());
                        Debug.Log("AttackPlayer2");
                        AttackPlayer();
                        break;
                    }

                    // Calculate the reflection vector
                    rayDirection = Vector3.Reflect(rayDirection, hit.normal);
                    reflections++;
                }
                else
                {
                    // If the ray does not hit anything, stop the loop
                    break;
                }
            }
        
            if (reflections > maxReflections)
            {
                // If the ray reflected twice without hitting the player, rotate the barrel
                StartCoroutine(RotateTimer());
            }
        }
    
        // private void UpdatePath()
        // {
        //     if(Time.time >= _pathUpdateDeadline)
        //     {
        //         Debug.Log("Updating Path");
        //         _pathUpdateDeadline = Time.time + _pathUpdateDelay;
        //         agent.SetDestination(target.position);
        //     }
        // }
    
        private void AttackPlayer()
        {
            Debug.Log("Enemy is attacking the player.");
            if (_alreadyAttacked) return;
            _alreadyAttacked = true;
            tankShooting.FirePreformed();
            _alreadyAttacked = false;
        }
        private void ResetAttack()
        {
            
        }
    
        private IEnumerator RotateTimer()
        {
            yield return new WaitForSeconds(5f);
            RandomRotateBarrel();
        }

        private void RotateBarrel()
        {
            Vector3 targetPosition = target.position;
            var position = barrel.transform.position;
            targetPosition.y = position.y;
            _direction = targetPosition - position;

            Quaternion lookRotation = Quaternion.LookRotation(_direction);
            lookRotation *= Quaternion.Euler(0, 90, 0);
            barrel.transform.rotation = Quaternion.Slerp(barrel.transform.rotation, lookRotation, Time.deltaTime * tankData.stats.turnSpeed);
        }

        private void RandomRotateBarrel()
        {
            if (_isRotating) return;
            float randomAngle = Random.Range(0f, 360f);
            Quaternion newRotation = Quaternion.Euler(0f, randomAngle, 0f);
            StartCoroutine(SmoothRotate(newRotation));
        }

        private IEnumerator SmoothRotate(Quaternion targetRotation)
        {
            _isRotating = true;
            float elapsedTime = 0f;
            float duration = 5f; // Adjust duration as needed for smoother transitions
            Quaternion startRotation = barrel.transform.rotation;

            while (elapsedTime < duration)
            {
                barrel.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            barrel.transform.rotation = targetRotation;
            _isRotating = false;
        }
    
        private bool CanSeePlayer()
        {
            _direction = target.position - transform.position;
            if (!Physics.Raycast(transform.position, _direction, out var hit)) return false;
            // Debug.Log($"Raycast hit: {hit.transform.name}");
            return hit.transform == target;
            // Debug.Log("Enemy can see the player.");
            // Debug.Log("Enemy cannot see the player.");
        }

        protected override void Move()
        {
        }

        protected override void Turn()
        {
        }
        protected override void ResetTank()
        {
            //transform.position = startingPosition;
            agent.Warp(startingPosition);
            _canMove = true;
            gameObject.SetActive(true);
        }
    
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            var position = transform.position;
            Gizmos.DrawWireSphere(position, attackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(position, sightRange);
        }
    }
}