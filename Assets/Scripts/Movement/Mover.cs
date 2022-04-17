using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        private Vector3 _velocity;
        private Vector3 _localVelocity;
        private Health _health;
        private float _speed;
        private NavMeshAgent _navMesh;
        private static readonly int ForwardSpeed = Animator.StringToHash("ForwardSpeed");

        private void Awake()
        {
            _navMesh = GetComponent<NavMeshAgent>();
            _health = GetComponent<Health>();
        }

        public void Cancel()
        {
            _navMesh.isStopped = true;
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }

        private void Update()
        {
            _navMesh.enabled = !_health.IsDead();
            UpdateAnimator();
        }
        public void MoveTo(Vector3 destination)
        {
            _navMesh.destination = destination;
            _navMesh.isStopped = false;
            
        }
        private void UpdateAnimator()
        {
            _velocity = _navMesh.velocity;
            _localVelocity = transform.InverseTransformDirection(_velocity);
            _speed = _localVelocity.z;
            GetComponent<Animator>().SetFloat(ForwardSpeed, _speed);
        }
    }
}
