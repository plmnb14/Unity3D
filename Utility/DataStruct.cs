using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataStruct : MonoBehaviour
{
    public void SaveCreatureData<T>(List<T> list, string jsonDataName)
    {
        string jsonData = JsonUtility.ToJson(new Serialization<T>(list), true);
        string path = Application.dataPath + "/Data/Json/" + jsonDataName;
        File.WriteAllText(path, jsonData);
    }

    public void SaveCreatureData<T>(T singleData, string jsonDataName)
    {
        string jsonData = JsonUtility.ToJson(singleData, true);
        string path = Application.dataPath + "/Data/Json/" + jsonDataName;
        File.WriteAllText(path, jsonData);
    }

    public void LoadCreatureData<T>(out List<T> list, string dataName)
    {
        string path = Application.dataPath + "/Data/Json/" + dataName;
        string jsonData = File.ReadAllText(path);
        list = JsonUtility.FromJson<Serialization<T>>(jsonData).ToList();
    }

    public void LoadCreatureData<T>(string dataName)
    {
        string path = Application.dataPath + "/Data/Json/" + dataName;
        string jsonData = File.ReadAllText(path);
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
public class CreatureData
{
    public string Name;
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