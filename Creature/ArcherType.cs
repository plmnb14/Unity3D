using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherType : Creature
{
    public GameObject AimPoint;

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
            Arrow newArrow = ObjectManager.GetObject();
            Vector3 Dir = target.transform.position - transform.position;
            newArrow.Initialize(AimPoint.transform.position, Dir, target.gameObject.layer, AttackDamage);
        }
    }

    protected override IEnumerator OnAttack()
    {
        base.OnAttack();

        if (canAttack)
        {
            canAttack = false;
            animator.SetInteger("AttackNum", UnityEngine.Random.Range(0, 3));
            animator.SetBool("isAttack", true);
        }

        else
        {
            TargetLookAt();

            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

            CurrentState = State.Idle;
            animator.SetBool("isAttack", false);
        }
    }

    void Start()
    {
        base.Initialize();

        AttackDelay = 0.1f;
        AttackSpeed = 2.0f;
        AttackDamage = 400.0f;
        AttackRange = 8.0f;
    }

    void Update()
    {
        CheckState();
        AttackTimer();
    }
}
