using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherType : Creature
{
    public override void OnDie()
    {
        base.OnDie();
    }

    protected override void ActiveSkill(int index)
    {
        base.ActiveSkill(index);
    }

    public override void OnDamage(Vector3 hitPoint, Vector3 hitNormal, float damage)
    {
        base.OnDamage(hitPoint, hitNormal, damage);
    }

    public override void Shoot()
    {
        base.Shoot();
    }

    float animationTime = 0.0f;
    protected override IEnumerator OnAttack()
    {
        base.OnAttack();

        if (canAttack)
        {
            animator.SetInteger("AttackNum", UnityEngine.Random.Range(0, 3));
            animator.SetTrigger("isAttack");
            canAttack = false;

            yield return new WaitForEndOfFrame();

            animationTime = animator.GetCurrentAnimatorStateInfo(0).length;
        }

        else
        {
            TargetLookAt();

            yield return new WaitForSeconds(animationTime);

            CurrentState = State.Idle;
        }
    }
}
