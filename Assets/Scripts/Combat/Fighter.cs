using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter:MonoBehaviour,IAction
    {
        private Health _target;
        
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private float weaponDamage = 15f;
        private float _timeSinceLastAttack = Mathf.Infinity;
        
        private static readonly int Attack1 = Animator.StringToHash("attack");
        private static readonly int StopAttack = Animator.StringToHash("stopAttack");

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            if (_target == null) return;

            if (_target.IsDead()) return;

            if (!IsInRange())
            {
                GetComponent<Mover>().MoveTo(_target.transform.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(_target.transform);
            if (_timeSinceLastAttack > timeBetweenAttacks)
            {
                //this will trigger the Hit() event
                GetComponent<Animator>().ResetTrigger(Attack1);
                GetComponent<Animator>().SetTrigger(Attack1);
                _timeSinceLastAttack = 0;
                
            }
            
        }
        // Animation event
        private void Hit()
        {
            if(_target == null) return;
            _target.TakeDamage(weaponDamage);
        }
        
        private bool IsInRange()
        {
            return Vector3.Distance(this.transform.position, _target.transform.position) < weaponRange;
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            _target = combatTarget.GetComponent<Health>();
        }
        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }
        public void Cancel()
        {
            GetComponent<Animator>().ResetTrigger(StopAttack);
            GetComponent<Animator>().SetTrigger(StopAttack);
            _target = null;
        }
    }
    }
    
    