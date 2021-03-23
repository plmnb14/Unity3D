using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{

    void Start()
    {
        base.MyCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != targetLayer)
            return;

        MyCollider.enabled = false;

        Creature CollisionTarget = other.GetComponent<Creature>();

        if (null != CollisionTarget && !CollisionTarget.Dead)
        {
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            Vector3 hitNormal = transform.position - other.transform.position;

            CollisionTarget.OnDamage(hitPoint, hitNormal, WeaponDamage);
        }
    }
}
