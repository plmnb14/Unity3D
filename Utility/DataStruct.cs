using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataStruct : MonoBehaviour
{
    static public void SaveData<T>(T singleData, string jsonDataName)
    {
        string jsonData = JsonUtility.ToJson(singleData, true);
        string path = Application.dataPath + "/Data/Json/" + jsonDataName + ".json";
        File.WriteAllText(path, jsonData);
    }

    static public void SaveData<T>(List<T> list, string jsonDataName)
    {
        string jsonData = JsonUtility.ToJson(new Serialization<T>(list), true);
        string path = Application.dataPath + "/Data/Json/" + jsonDataName + ".json";
        File.WriteAllText(path, jsonData);
    }

    static public void SaveData<TKey, TValue>(Dictionary<TKey, TValue> dic, string jsonDataName)
    {
        string jsonData = JsonUtility.ToJson(new Serialization<TKey, TValue>(dic), true);
        string path = Application.dataPath + "/Data/Json/" + jsonDataName + ".json";
        File.WriteAllText(path, jsonData);
    }

    static public void LoadData<T>(out T data, string dataName)
    {
        string path = Application.dataPath + "/Data/Json/" + dataName + ".json";
        string jsonData = File.ReadAllText(path);
        data = JsonUtility.FromJson<T>(jsonData);
    }

    static public void LoadData<T>(out List<T> list, string dataName)
    {
        string path = Application.dataPath + "/Data/Json/" + dataName + ".json";
        string jsonData = File.ReadAllText(path);
        list = JsonUtility.FromJson<Serialization<T>>(jsonData).ToList();
    }

    static public void LoadData<TKey, TValue>(out Dictionary<TKey, TValue> dic, string dataName)
    {
        string path = Application.dataPath + "/Data/Json/" + dataName + ".json";
        string jsonData = File.ReadAllText(path);
        dic = JsonUtility.FromJson<Serialization<TKey, TValue>>(jsonData).ToDictionary();
    }
}

[System.Serializable]
public class Serialization<T>
{
    [SerializeField]
    List<T> target;
    
    public List<T> ToList() { return target; }
    
    public Serialization(List<T> target)
    {
        this.target = target;
    }
}

[System.Serializable]
public class Serialization<TKey, TValue> : ISerializationCallbackReceiver
{
    [SerializeField]
    List<TKey> keys;
    [SerializeField]
    List<TValue> values;

    Dictionary<TKey, TValue> target;
    public Dictionary<TKey, TValue> ToDictionary() { return target; }

    public Serialization(Dictionary<TKey, TValue> target)
    {
        this.target = target;
    }

    public void OnBeforeSerialize()
    {
        keys = new List<TKey>(target.Keys);
        values = new List<TValue>(target.Values);
    }

    public void OnAfterDeserialize()
    {
        var count = Mathf.Min(keys.Count, values.Count);
        target = new Dictionary<TKey, TValue>(count);
        for(var i = 0; i < count; ++i)
        {
            target.Add(keys[i], values[i]);
        }
    }
}

[System.Serializable]
public class BaseData
{
    public string Name;

    public virtual BaseData DeepCopy()
    {
        BaseData copy = new BaseData();
        copy.Name = this.Name;

        return copy;
    }
}

[System.Serializable]
public class CreatureData : BaseData
{
    public override BaseData DeepCopy()
    {
        base.DeepCopy();

        CreatureData copy = new CreatureData();
        copy.isCaptain = this.isCaptain;
        copy.UnitType = this.UnitType;
        copy.Level = this.Level;
        copy.ReinforceLevel = this.ReinforceLevel;
        copy.HitPoint = this.HitPoint;
        copy.Mana = this.Mana;
        copy.Armor = this.Armor;
        copy.AttackDamage = this.AttackDamage;
        copy.AttackRange = this.AttackRange;
        copy.AttackDelay = this.AttackDelay;
        copy.AttackSpeed = this.AttackSpeed;
        copy.MoveSpeed = this.MoveSpeed;

        return copy;
    }

    public bool isCaptain = false;
    public int UnitType;
    public int Level;
    public int ReinforceLevel;
    public float HitPoint;
    public float Mana;
    public float Armor;
    public float AttackDamage;
    public float AttackRange;
    public float AttackDelay;
    public float AttackSpeed;
    public float MoveSpeed;
}

[System.Serializable]
public class SkillBaseData : BaseData
{
    public override BaseData DeepCopy()
    {
        base.DeepCopy();

        SkillBaseData copy = new SkillBaseData();
        copy.skillType = this.skillType;
        copy.targetType = this.targetType;
        copy.stateName = this.stateName;
        copy.cooltime = this.cooltime;
        copy.manaCost = this.manaCost;
        copy.duration = this.duration;
        copy.percentage = this.percentage;

        return copy;
    }

    public string skillType;
    public string targetType;
    public string stateName;
    public float cooltime;
    public float manaCost;
    public float duration;
    public float percentage;
}

[System.Serializable]
public class StageMonsterData : BaseData
{
    public override BaseData DeepCopy()
    {
        base.DeepCopy();
        StageMonsterData copy = new StageMonsterData();
        copy.position = this.position;
        copy.yRotate = this.yRotate;

        return copy;
    }

    public Vector3 position;
    public float yRotate;
}

[System.Serializable]
public class StageInfo : BaseData
{
    public override BaseData DeepCopy()
    {
        base.DeepCopy();

        StageInfo copy = new StageInfo();
        copy.summonCount = this.summonCount;
        copy.waveCount = this.waveCount;
        copy.summonPerWave = this.summonPerWave;
        copy.gold = this.gold;
        copy.exp = this.exp;

        return copy;
    }

    public int summonCount;
    public int waveCount;
    public List<int> summonPerWave;
    public int gold;
    public int exp;
}