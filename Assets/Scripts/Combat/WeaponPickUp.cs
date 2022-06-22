using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Combat
{
    public class WeaponPickUp : MonoBehaviour
    {
        [SerializeField] private Weapon weapon = null;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                Destroy(gameObject);
            }
        }
    }
}