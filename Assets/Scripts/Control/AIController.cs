using System;
using RPG.Attributes;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTime = 3f;
        [SerializeField] private float wayPointDwellTime = 3f;
        [SerializeField] private PatrolPath patrolPath;
        [Range(0,1)]
        [SerializeField] private float patrolSpeedFraction = 0.2f;
        private Fighter _fighter;
        private GameObject _player;
        private Health _health;
        private Mover _mover;
        private Vector3 _guardPosition;
        
        private int _currentWayPointIndex = 0;
        private float _timeSinceLastSawPlayer = Mathf.Infinity;
        private float _timeSinceArrivedAtWayPoint =  Mathf.Infinity;
       
        private void Start()
        {
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _player = GameObject.FindWithTag("Player");
            _mover = GetComponent<Mover>();

            _guardPosition = transform.position;
        }

        private void Update()
        {
            if (_health.IsDead()) return;
            
            var player = GameObject.FindWithTag("Player");
            if (InAttackRange() && _fighter.CanAttack(player))
            {
                _timeSinceLastSawPlayer = 0;
                AttackBehaviour();
            }
            else if(Math.Abs(_timeSinceLastSawPlayer - suspicionTime) < 0.1f)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            _timeSinceLastSawPlayer += Time.deltaTime;
            _timeSinceArrivedAtWayPoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPos = _guardPosition;
            if (patrolPath != null)
            {
                if (AtWayPoint())
                {
                    _timeSinceArrivedAtWayPoint = 0;
                    CycleWaypoint();
                }
                nextPos = GetCurrentWayPoint();
            }

            if (_timeSinceArrivedAtWayPoint > wayPointDwellTime)
            {
                _mover.StartMoveAction(nextPos, patrolSpeedFraction);
            }
            
        }

        private Vector3 GetCurrentWayPoint()
        {
            return patrolPath.GetWaypoint(_currentWayPointIndex);
        }

        private void CycleWaypoint()
        {
            _currentWayPointIndex = patrolPath.GetNextIndex(_currentWayPointIndex);
        }

        private bool AtWayPoint()
        {
            float distanceToWayPoint = Vector3.Distance(transform.position, GetCurrentWayPoint());
            return distanceToWayPoint < 1f;
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            _fighter.Attack(_player);
        }

        private bool InAttackRange()
        {
            return Vector3.Distance(_player.transform.position, this.transform.position) < chaseDistance;
        }
        //called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(this.transform.position, chaseDistance);
        }
    }
}
    

