using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Health target = null;
    [SerializeField] private float speed = 1f;
    void Update()
    {
        if(target == null) return;
        
        transform.LookAt(GetAimlocation());
        transform.Translate(Vector3.forward * Time.deltaTime);
    }

    public void SetTarget(Health target)
    {
        this.target = target;
    }
    private Vector3 GetAimlocation()
    {
        CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
        if (targetCapsule == null)
        {
            return target.transform.position;
        }
        return target.transform.position + Vector3.up * targetCapsule.height / 2;
    }
}
