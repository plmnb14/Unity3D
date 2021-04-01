using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior_08 : WarriorType
{

    protected override void ActiveWeaponCollider(int iValue)
    {
        MyWeapon.GetComponent<Collider>().enabled = iValue == 0 ? false : true;
    }

    void Start()
    {
        base.Initialize();

        SkillManager.instance.GetSkillData("Skill_SmashGround", out skills[1], this);
        skills[1].targetMask = TargetlayerMask;

        SkillManager.instance.GetSkillData("Buff_ArmorUp", out skills[2], this);
        skills[2].targetMask = TargetlayerMask;
    }

    void Update()
    {
        base.CheckState();
        base.AttackTimer();
    }
}
