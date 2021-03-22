using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Creature
{
    public override void OnDie()
    {

    }

    public override void OnDamage(Vector3 hitPoint, Vector3 hitNormal, float damage)
    {

    }

    protected override IEnumerator OnAttack()
    {
        return base.OnAttack();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
