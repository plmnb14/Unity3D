using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior_08 : WarriorType
{

    protected override void ActiveWeaponCollider(int iValue)
    {
        MyWeapon.GetComponent<Collider>().enabled = iValue == 0 ? false : true;
    }

    private void Awake()
    {
        SkillManager.instance.GetSkillData("Skill_SmashGround", out skills[1], this);
        SkillManager.instance.GetSkillData("Buff_ArmorUp", out skills[2], this);
    }

    void Start()
    {
        base.Initialize();

        for(int i = 0; i < 2; i++)
            skills[i+1].targetMask = TargetlayerMask;
    }

    void Update()
    {
        base.CheckState();
        base.AttackTimer();
    }
}
