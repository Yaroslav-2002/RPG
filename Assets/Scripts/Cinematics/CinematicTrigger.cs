using System;
using RPG.Saving;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private bool _isTrigerred = false;
        private void OnTriggerEnter(Collider other)
        {
            if (!_isTrigerred && other.CompareTag("Player"))
            {
                GetComponent<PlayableDirector>().Play();
                _isTrigerred = true;
            }
        }
    }
}

