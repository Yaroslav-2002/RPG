using RPG.Attributes;
using RPG.Core;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISavable
    {
        [SerializeField] private float maxSpeed = 5.62f;
        private Vector3 _velocity;
        private Vector3 _localVelocity;
        private Health _health;
        private float _speed;
        private NavMeshAgent _navMesh;
        private static readonly int ForwardSpeed = Animator.StringToHash("ForwardSpeed");

        public object CaptureState() => new SerializbleVector3(transform.position);
        
        private void Awake()
        {
            _navMesh = GetComponent<NavMeshAgent>();
            _health = GetComponent<Health>();
        }
        
        public void Cancel()
        {
            _navMesh.isStopped = true;
        }
        
        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }
        
        private void Update()
        {
            _navMesh.enabled = !_health.IsDead();
            UpdateAnimator();
        }
        
        public void MoveTo(Vector3 destination, float speedFraction)
        {
            _navMesh.destination = destination;
            _navMesh.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            _navMesh.isStopped = false;
            
        }
        
        private void UpdateAnimator()
        {
            _velocity = _navMesh.velocity;
            _localVelocity = transform.InverseTransformDirection(_velocity);
            _speed = _localVelocity.z;
            GetComponent<Animator>().SetFloat(ForwardSpeed, _speed);
        }
        public void RestoreState(object state)
        {
            SerializbleVector3 position = (SerializbleVector3)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector3();
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}
