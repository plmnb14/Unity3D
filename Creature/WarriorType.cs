using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorType : Creature
{
    public override void OnDie()
    {
        base.OnDie();
    }

    public override void OnDamage(Vector3 hitPoint, Vector3 hitNormal, float damage)
    {
        base.OnDamage(hitPoint, hitNormal, damage);
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

            EnableWeaponCollider(0);
            CurrentState = State.Idle;
            animator.SetBool("isAttack", false);
        }
    }

    protected virtual void EnableWeaponCollider(int num)
    {
        bool enable = num > 0 ? true : false;

        Collider collider = MyWeapon.GetComponent<Collider>();
        collider.enabled = enable;
    }

    void Start()
    {
        base.Initialize();
    }

    void Update()
    {
        Debug.Log("현재 상태 : " + CurrentState);

        CheckState();
        AttackTimer();
    }
}
