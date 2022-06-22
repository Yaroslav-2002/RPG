using System;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        public float weaponRange = 2f;
        public float weaponDamage = 15f;
        public float timeBetweenAttacks = 1f;
        [SerializeField] private AnimatorOverrideController weaponOverride = null;
        [SerializeField] private GameObject equipedPefab = null;
        [SerializeField] private bool isRightHanded = true;
        [SerializeField] private Projectile projectile = null;

        public void Spawn(Transform rightHand,Transform leftHand, Animator animator)
        {
            if (equipedPefab != null)
            {
                Transform handTransfrom = GetTransform(leftHand, rightHand);
                Instantiate(equipedPefab, handTransfrom);
            }

            if (weaponOverride != null)
            {
                animator.runtimeAnimatorController = weaponOverride;
            }
        }

        private Transform GetTransform(Transform leftHand, Transform rightHand)
        {
            Transform handTransfrom;
            if (isRightHanded) 
                handTransfrom = rightHand;
            else
                handTransfrom = leftHand;
            return handTransfrom;
        }
        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Projectile projectileInstance =
                Instantiate(projectile, GetTransform(leftHand, rightHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target);
        }
    }
}