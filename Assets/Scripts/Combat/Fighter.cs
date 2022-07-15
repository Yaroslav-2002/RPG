using System;
using RPG.Attributes;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;

namespace RPG.Combat
{
    public class Fighter:MonoBehaviour,IAction,ISavable
    {
        private Health _target;
        
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] private Weapon defaultWeapon;


        private float _timeSinceLastAttack = Mathf.Infinity;
        private Weapon _currentWeapon;
        
        
        private static readonly int Attack1 = Animator.StringToHash("attack");
        private static readonly int StopAttack = Animator.StringToHash("stopAttack");

        private void Awake()
        {
            if (_currentWeapon == null)
            {
                EquipWeapon(defaultWeapon);
                Debug.Log("Deafault gun");
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            _currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            try
            {
                weapon.Spawn(rightHandTransform, leftHandTransform, animator);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debug.Log(e,this);
            }
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
            if (_timeSinceLastAttack > _currentWeapon.timeBetweenAttacks)
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
            if (_currentWeapon.HasProjectile())
            {
                _currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, _target, gameObject);
            }
            else
            {
                _target.TakeDamage(gameObject, _currentWeapon.weaponDamage);
            }
        }
        
        private void Shoot()
        {
            Hit();
        }
        
        
        private bool IsInRange()
        {
            return Vector3.Distance(this.transform.position, _target.transform.position) < _currentWeapon.weaponRange;
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

        public object CaptureState()
        {
            Debug.Log("Saved gun");
            return _currentWeapon.name;
            
        }
        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
            Debug.Log("Restored gun");
        }
        public Health GetTarget()
        {
            return _target;
        }
    }
    }
    
    