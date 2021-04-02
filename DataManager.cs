using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public enum DataType { Hero, Monster, Item};

    public Dictionary<string, CreatureData> MonsterDataDic { get; set; }
 

    public static DataManager instance
    {
        get
        {
            if(null == m_instance)
            {
                m_instance = FindObjectOfType<DataManager>();
            }

            return m_instance;
        }
    }
    private static DataManager m_instance;

    private void LoadData(string path, DataType type)
    {
        switch(type)
        {
            case DataType.Hero:
                {
                    Dictionary<string, CreatureData> tmpDic = new Dictionary<string, CreatureData>();
                    DataStruct.LoadData<string, CreatureData>(out tmpDic, path);
                    MonsterDataDic = new Dictionary<string, CreatureData>(tmpDic);

                    break;
                }

            case DataType.Monster:
                {
                    Dictionary<string, CreatureData> tmpDic = new Dictionary<string, CreatureData>();
                    DataStruct.LoadData<string, CreatureData>(out tmpDic, path);
                    MonsterDataDic = new Dictionary<string, CreatureData>(tmpDic);

                    break;
                }
        }
    }

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        LoadData("MonsterData", DataType.Monster);

        //LoadData("ItemData");
    }
}
