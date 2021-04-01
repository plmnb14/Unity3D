using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priest_04 : PriestType
{
    public override void Shoot()
    {
        if (null != target)
        {
            Projectile newMissile = ObjectManager.instance.GetObject(ObjectManager.ProjType.MagicMissile);
            Vector3 Dir = (target.transform.position + Vector3.up) - AimPoint.transform.position;
            newMissile.Initialize(AimPoint.transform.position, Dir, target.gameObject.layer, MyStatus.AttackDamage, 2.0f);
            newMissile.Owner = this;
        }
    }

    void Start()
    {
        base.Initialize();
    }

    void Update()
    {
        CheckState();
        AttackTimer();
    }
}
