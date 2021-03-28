using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior_08 : WarriorType
{
    public override void OnDie()
    {
        base.OnDie();
    }

    public override void OnDamage(Vector3 hitPoint, Vector3 hitNormal, float damage)
    {
        base.OnDamage(hitPoint, hitNormal, damage);
    }

    void Start()
    {
        base.Initialize();
    }

    void Update()
    {
        base.CheckState();
        base.AttackTimer();
    }
}
