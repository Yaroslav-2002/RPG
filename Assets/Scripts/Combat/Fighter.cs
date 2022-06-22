using System;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using UnityEngine.Serialization;

namespace RPG.Combat
{
    public class Fighter:MonoBehaviour,IAction
    {
        private Health _target;
        
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] private Weapon defaultWeapon = null;
        
        private float _timeSinceLastAttack = Mathf.Infinity;
        private Weapon curentWeapon = null;
        
        private static readonly int Attack1 = Animator.StringToHash("attack");
        private static readonly int StopAttack = Animator.StringToHash("stopAttack");

        private void Start()
        {
            EquipWeapon(defaultWeapon);
        }

        public void EquipWeapon(Weapon weapon)
        {
            curentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            if (_target == null) return;

            if (_target.IsDead()) return;

            if (!IsInRange())
            {
                GetComponent<Mover>().MoveTo(_target.transform.position, 1f);
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
            if (_timeSinceLastAttack > curentWeapon.timeBetweenAttacks)
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
            if (curentWeapon.HasProjectile())
            {
                curentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, _target);
            }
            _target.TakeDamage(curentWeapon.weaponDamage);
        }

        private void Shoot()
        {
            Hit();
        }
        
        private bool IsInRange()
        {
            return Vector3.Distance(this.transform.position, _target.transform.position) < curentWeapon.weaponRange;
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
            StopAttackFunc();
            _target = null;
            GetComponent<Mover>().Cancel();
        }
        private void StopAttackFunc(){
            GetComponent<Animator>().ResetTrigger(StopAttack);
            GetComponent<Animator>().SetTrigger(StopAttack);
        }
    }
    }
    
    