using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestType : Creature
{
    public override void OnDie()
    {
        base.OnDie();
    }

    public override void OnDamage(Vector3 hitPoint, Vector3 hitNormal, float damage)
    {
        base.OnDamage(hitPoint, hitNormal, damage);
    }

    public virtual void Shoot()
    {
        if (null != target)
        {

        }
    }
}
