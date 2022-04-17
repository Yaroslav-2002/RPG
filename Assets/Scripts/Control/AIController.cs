using System;
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
        private Fighter _fighter;
        private GameObject _player;
        private Health _health;

        private Vector3 _guardPosition;
        private Mover _mover;
        private float _timeSinceLastSawPlayer = Mathf.Infinity;
        
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
                GuardBehaviour();
            }

            _timeSinceLastSawPlayer += Time.deltaTime;
        }

        private void GuardBehaviour()
        {
            _fighter.Cancel();
            _mover.StartMoveAction(_guardPosition);
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
    

