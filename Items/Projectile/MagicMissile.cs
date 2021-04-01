using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissile : Projectile
{
    protected override void OnDead()
    {
        ResetStatus();
        ObjectManager.ReturnObject(this, ObjectManager.ProjType.MagicMissile);
    }

    private void Awake()
    {
        Particles[(int)ParticleType.Tail] = Instantiate(Resources.Load<ParticleSystem>("Polygon Arsenal/Prefabs/Combat/Missiles/Plasma/PlasmaMissileBlue"));
        Particles[(int)ParticleType.Tail].transform.SetParent(transform.GetChild((int)ParticleType.Tail).gameObject.transform);
        Particles[(int)ParticleType.Tail].transform.localPosition = Vector3.zero;

        string path = "Polygon Arsenal/Prefabs/Combat/Explosions/Plasma/PlasmaExplosionBlue";
        ParticleManager.instance.AddParticle("MagicMissile_Dead", path);

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

        if (null != CollisionTarget && !CollisionTarget.Dead)
        {
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            Vector3 hitNormal = transform.position - other.transform.position;

            CollisionTarget.OnDamage(hitPoint, hitNormal, projectileDamage, out float finalDamage);

            StageBattleManager.instance.FindTarget(StageBattleManager.StatusType.HitPoint, Owner.gameObject.layer).GainHealth(finalDamage * 0.5f);

            var ExplosionParticle = ParticleManager.instance.GetParticle("MagicMissile_Dead");
            ExplosionParticle.transform.position = hitPoint;
            ExplosionParticle.GetComponent<ParticleObject>().OnPlay("MagicMissile_Dead");

            OnDead();
        }
    }
}
