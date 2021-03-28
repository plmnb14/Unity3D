using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public struct UnitData
{
    public string Name;
    public int UnitType;
    public int Level;
    public float HitPoint;
    public float Mana;
    public float Armor;
    public float AttackDamage;
    public float AttackRange;
    public float AttackDelay;
    public float AttackSpeed;
    public float MoveSpeed;
}

public class CSVReader : MonoBehaviour
{
    public static string DataPath = "UnitData.csv";

    public static Dictionary<string, UnitData> Read(string file)
    {
        Dictionary<string, UnitData> UnitDataDictionary = new Dictionary<string, UnitData>();

        StreamReader reader = new StreamReader(Application.dataPath + "/" + file);

        bool EOF = false;
        reader.ReadLine();
        while (!EOF)
        {
            string data_String = reader.ReadLine();
            if(data_String == null)
            {
                EOF = true;
                break;
            }
            var data_values = data_String.Split(',');
            UnitData tmpData = new UnitData();

            tmpData.Name = data_values[0];
            tmpData.UnitType = int.Parse(data_values[1]);
            tmpData.Level = int.Parse(data_values[2]);
            tmpData.HitPoint = float.Parse(data_values[3]);
            tmpData.Mana = float.Parse(data_values[4]);
            tmpData.Armor = float.Parse(data_values[5]);
            tmpData.AttackDamage = float.Parse(data_values[6]);
            tmpData.AttackDelay = float.Parse(data_values[7]);
            tmpData.AttackSpeed = float.Parse(data_values[8]);
            tmpData.MoveSpeed = float.Parse(data_values[9]);

            UnitDataDictionary.Add(tmpData.Name, tmpData);
        }

        return UnitDataDictionary;
    }
}
