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

    public void SaveSkillData()
    {
        SkillBaseData tmpData1 = new SkillBaseData();
        tmpData1.cooltime = 10.0f;
        tmpData1.manaCost = 10.0f;
        tmpData1.stateName = "Hello";
        tmpData1.skillName = "GroundSmash";

        SkillBaseData tmpData2 = new SkillBaseData();
        tmpData2.cooltime = 10.0f;
        tmpData2.manaCost = 10.0f;
        tmpData2.stateName = "Hello";
        tmpData2.skillName = "IU";

        SkillDictionary.Add(tmpData1.skillName, tmpData1);
        SkillDictionary.Add(tmpData2.skillName, tmpData2);

        DataStruct.SaveData<string, SkillBaseData>(SkillDictionary, "Warrior_08_SkillData.json");
    }

    public void LoadSkillData()
    {
        DataStruct.LoadData<string, SkillBaseData>(out SkillDictionary, "Warrior_08_SkillData.json");
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