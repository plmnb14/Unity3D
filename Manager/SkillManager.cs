using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    Dictionary<string, SkillBaseData> SkillDictionary;

    public static SkillManager instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = FindObjectOfType<SkillManager>();
            }

            return m_instance;
        }
    }
    private static SkillManager m_instance;

    public void GetSkillData(string skillName, out SkillData data, Creature Owner)
    {
        data = null;

        if (SkillDictionary.Count < 0)
            return;

        SkillBaseData baseData;
        if(SkillDictionary.TryGetValue(skillName, out baseData))
        {
            data = Instantiate(Resources.Load<SkillData>("_Prefabs/Skill/" + skillName), Owner.gameObject.transform);
            data.skillData = baseData;
        }
    }

    public void LoadSkillData()
    {
        DataStruct.LoadData<string, SkillBaseData>(out SkillDictionary, "Warrior_08_SkillData");
    }

    private void Awake()
    {
        if(instance != this)
        {
            Destroy(gameObject);
        }

        SkillDictionary = new Dictionary<string, SkillBaseData>();
        //SaveSkillData();
        LoadSkillData();
    }
}