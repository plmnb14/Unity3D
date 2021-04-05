using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHudHeroInfo : PopUpUI
{
    private Image portrait;
    private HUDSkillInfo[] skillInfo;
    private StatSlider[] statSlider;
    private Dictionary<string, BuffSkill> buffs;

    public void SetUp(Creature hero)
    {
        portrait.sprite = Resources.Load<Sprite>("UI/MiniPortrait/" + hero.MyStatus.Name);

        for (int i = 1; i < 3; i++)
        {
            if(hero.skills[i] != null)
            {
                skillInfo[i-1].SetUp(hero.skills[i].skillData);
            }
        }

        statSlider[0].SetUp(hero.OriginStatus.HitPoint);
        statSlider[1].SetUp(hero.OriginStatus.Mana);

        hero.healthChange += statSlider[0].ChangeSliderValue;

        hero.activeSkill += OnSkillCooltime;
    }

    public void BuffManage(BuffSkill buff, bool add)
    {

    }

    private void OnSkillCooltime(int skillIndex)
    {
        if(skillIndex-1 >= 0)
            skillInfo[skillIndex-1].ActiveSkill();
    }

    private void Awake()
    {
        buffs = new Dictionary<string, BuffSkill>();

        portrait = transform.GetChild(0).GetComponent<Image>();

        skillInfo = new HUDSkillInfo[2];
        skillInfo[0] = transform.GetChild(1).GetComponent<HUDSkillInfo>();
        skillInfo[1] = transform.GetChild(2).GetComponent<HUDSkillInfo>();
        
        statSlider = new StatSlider[2];
        statSlider[0] = transform.GetChild(3).GetComponent<StatSlider>();
        statSlider[1] = transform.GetChild(4).GetComponent<StatSlider>();
    }
}
