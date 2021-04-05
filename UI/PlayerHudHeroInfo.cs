using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHudHeroInfo : PopUpUI
{
    private Image portrait;
    private HUDSkillInfo[] skillInfo;
    private StatSlider[] statSlider;

    public void SetUp(Creature hero)
    {
        portrait.sprite = Resources.Load<Sprite>("UI/MiniPortrait/" + hero.MyStatus.Name);

        for(int i = 0; i < 2; i++)
        {
            if(hero.skills[i+1] != null)
            {
                skillInfo[i].SetUp(hero.skills[i+1].skillData);
            }
        }

        statSlider[0].SetUp(hero.OriginStatus.HitPoint);
        statSlider[1].SetUp(hero.OriginStatus.Mana);

        hero.healthChange += statSlider[0].ChangeSliderValue;

        hero.activeSkill += OnSkillCooltime;
    }

    private void OnSkillCooltime(int skillIndex)
    {
        skillInfo[skillIndex].ActiveSkill();
    }

    private void Awake()
    {
        portrait = transform.GetChild(0).GetComponent<Image>();

        skillInfo = new HUDSkillInfo[2];
        skillInfo[0] = transform.GetChild(1).GetComponent<HUDSkillInfo>();
        skillInfo[1] = transform.GetChild(2).GetComponent<HUDSkillInfo>();

        statSlider = new StatSlider[2];
        statSlider[0] = transform.GetChild(3).GetComponent<StatSlider>();
        statSlider[1] = transform.GetChild(4).GetComponent<StatSlider>();
    }
}
