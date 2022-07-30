﻿using System;
using RPG.Attributes;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;

namespace RPG.Combat
{
    public class Fighter:MonoBehaviour,IAction,ISavable
    {
        [SerializeField] private Transform rightHandTransform;
        [SerializeField] private Transform leftHandTransform;
        [SerializeField] private Weapon defaultWeapon;
        private Health _target;
        private Weapon _currentWeapon;
        private float _timeSinceLastAttack = Mathf.Infinity;
        private static readonly int Attack1 = Animator.StringToHash("attack");
        private static readonly int StopAttack = Animator.StringToHash("stopAttack");
        
        public Health GetTarget() =>_target;
        public object CaptureState() => _currentWeapon.name;

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
            
            var damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (_currentWeapon.HasProjectile())
            {
                _currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, _target, gameObject, damage);
            }
            else
            {
                _target.TakeDamage(gameObject, damage);
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
        
        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
            Debug.Log("Restored gun");
        }
    }
}
    
    