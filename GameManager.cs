using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GameManager>();
            }

            return m_instance;
        }
    }
    private static GameManager m_instance;

    private Dictionary<string, UnitData> UnitDataDic = new Dictionary<string, UnitData>();

    private void Awake()
    {
        if(instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        UnitDataDic = CSVReader.Read("UnitData.csv");

        // 소환코드
        //GameObject Instance = Resources.Load<GameObject>("_Prefabs/Weapon/SM_WPN_2HandAxe_01");
        //Instantiate(Instance, Instance.transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
