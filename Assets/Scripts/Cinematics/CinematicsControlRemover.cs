﻿using System;
using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.PlayerLoop;

namespace RPG.Cinematics
{
    public class CinematicsControlRemover : MonoBehaviour
    {
        private GameObject player;
        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;

        }

        void DisableControl(PlayableDirector playableDirector)
        {
            
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        void EnableControl(PlayableDirector playableDirector)
        {
            print("Enable");
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}