using System;
using System.Collections;
using System.Collections.Generic;
using RPG.UI.DamageText;
using UnityEngine;

public class DamageTextSpawner : MonoBehaviour
{
    [SerializeField] private DamageText damageTextPrefab;
    public void Spawn(float damageAmount)
    {
        DamageText instance = Instantiate<DamageText>(damageTextPrefab, transform);
        instance.SetValue(damageAmount);
    }
}
