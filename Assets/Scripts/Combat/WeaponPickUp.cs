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
        [SerializeField] private float respawnTime = 5f;

        private Collider _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                StartCoroutine(HideFoSeconds(respawnTime));
            }
        }

        private IEnumerator HideFoSeconds(float seconds)
        {
            ShowPickUp(false);
            var waitForSeconds = new WaitForSeconds(seconds);
            yield return waitForSeconds;
            ShowPickUp(true);
        }

        private void ShowPickUp(bool shouldShow)
        {
            _collider.enabled = shouldShow;
            transform.GetChild(0).gameObject.SetActive(shouldShow);
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }

      
    }
}