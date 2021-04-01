using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile
{
    protected override void OnDead()
    {
        ResetStatus();
        ObjectManager.ReturnObject(this, ObjectManager.ProjType.Arrow);
    }

    private void Awake()
    {
        Particles[(int)ParticleType.Tail] = Instantiate(Resources.Load<ParticleSystem>("Polygon Arsenal/Prefabs/Combat/Missiles/Sniper/SniperBulletGreen"));
        Particles[(int)ParticleType.Tail].transform.SetParent(transform.GetChild((int)ParticleType.Tail).gameObject.transform);
        Particles[(int)ParticleType.Tail].transform.localPosition = Vector3.zero;
    }

    private void Update()
    {
        Flying();
        StartCoroutine(LifeCycle());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != targetLayer)
            return;

        Creature CollisionTarget = other.GetComponent<Creature>();

        if(null != CollisionTarget && !CollisionTarget.Dead)
        {
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            Vector3 hitNormal = transform.position - other.transform.position;

            CollisionTarget.OnDamage(hitPoint, hitNormal, projectileDamage);

            OnDead();
        }
    }
}
