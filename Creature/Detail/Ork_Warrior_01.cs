using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ork_Warrior_01 : WarriorType
{
    protected override void ActiveWeaponCollider(int iValue)
    {
        MyWeapon.GetComponent<Collider>().enabled = iValue == 0 ? false : true;
    }

    void Start()
    {
        base.Initialize();

        //animator.SetFloat("AttackSpeed", 1.0f);
        MyWeapon.WeaponDamage = MyStatus.AttackDamage;
    }

    void Update()
    {
        base.CheckState();
        base.AttackTimer();
    }
}
