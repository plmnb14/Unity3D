using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_ArmorUp : BuffSkill
{
    ParticleSystem armorUp;
    ParticleSystem buffAura;

    public override void ActiveSkill()
    {
        Owner.AddBuff(this);

        icon.gameObject.SetActive(true);
        iconBack.gameObject.SetActive(true);

        this.gameObject.SetActive(true);
        armorUp.gameObject.SetActive(true);
        buffAura.gameObject.SetActive(true);

        Owner.MyStatus.Armor += (Owner.OriginStatus.Armor * skillData.percentage);

        armorUp.transform.parent = Owner.transform;
        armorUp.transform.localPosition = Vector3.zero + new Vector3(0.0f, 0.7f, 0.0f);
        armorUp.Play();

        buffAura.transform.parent = Owner.transform;
        buffAura.transform.localPosition = Vector3.zero;
        buffAura.transform.localScale = Vector3.one * 1.5f;
        buffAura.Play();

        Owner.onDeath += DeActivation;
        Execute();
    }

    protected override void DeActivation()
    {
        Owner.onDeath -= DeActivation;

        Owner.MyStatus.Armor -= (Owner.OriginStatus.Armor * skillData.percentage);

        armorUp.gameObject.SetActive(false);
        buffAura.gameObject.SetActive(false);

        icon.gameObject.SetActive(false);
        iconBack.gameObject.SetActive(false);

        Owner.RemoveBuff(this);
    }

    private void Awake()
    {
        Loadchild();
        buffAura = Instantiate(Resources.Load<ParticleSystem>("Polygon Arsenal/Prefabs/Combat/Buff/BuffYellow"));
        armorUp = Instantiate(Resources.Load<ParticleSystem>("Polygon Arsenal/Prefabs/Combat/Shield/ShieldYellow"));
    }
}
