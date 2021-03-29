using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_SmashGround : SkillData
{
    ParticleSystem SmashGround;
    
    public override void ActiveSkill()
    {
        SmashGround.transform.position =
            Owner.transform.position +
            Owner.transform.forward * 1.5f +
            new Vector3(0.0f, 0.05f, 0.0f);
        SmashGround.Play();

        CheckTarget();
    }

    public void CheckTarget()
    {
        Collider[] colliders =
            Physics.OverlapSphere(transform.position, 2.0f, targetMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            Creature tmpTarget = colliders[i].gameObject.GetComponent<Creature>();

            if (null != tmpTarget && !tmpTarget.Dead)
            {
                Vector3 hitPoint = colliders[i].ClosestPoint(transform.position);
                Vector3 hitNormal = transform.position - colliders[i].transform.position;

                tmpTarget.OnDamage(hitPoint, hitNormal, Owner.MyStatus.AttackDamage * skillData.percentage);
            }
        }
    }

    private void Awake()
    {
        SmashGround = Instantiate(Resources.Load<ParticleSystem>("_Prefabs/Effect/FX_GroundCrack_Blast_01"));
    }
}
