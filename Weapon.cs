using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipment
{
    public float WeaponDamage { get; set; } = 100.0f;
    public float OnwerDamage { get; set; } = 0.0f;

    protected Collider MyCollider;

    public void SetAttackEnable(bool setBool)
    {
        MyCollider.enabled = setBool;
    }

    public virtual void OnAttack(Vector3 OwnerPosition, Vector3 Direction)
    {

    }

    void Start()
    {

    }

    void Update()
    {
        
    }
}
