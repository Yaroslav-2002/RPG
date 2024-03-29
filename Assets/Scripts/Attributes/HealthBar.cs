﻿using System;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health healthComponent;
        [SerializeField] private RectTransform foreground;
        [SerializeField] private Canvas rootCanvas;
        private void Update()
        {
            if (Mathf.Approximately(healthComponent.GetFraction(), 0) 
                || Mathf.Approximately(healthComponent.GetFraction(),1))
            {
                rootCanvas.enabled = false;
                return;
            }
            rootCanvas.enabled = true;
            foreground.localScale = new Vector3(healthComponent.GetFraction(), 1 , 1);
        }
    }
}