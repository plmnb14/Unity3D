using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBattleManager : MonoBehaviour
{
    public static StageBattleManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<StageBattleManager>();
            }

            return m_instance;
        }
    }
    private static StageBattleManager m_instance;

    private List<Vector3> coordTileList = new List<Vector3>();
    private List<int> TeamSlotIndex = new List<int>(); // "ΩΩ∑‘"¿« ¿Œµ¶Ω∫
    private List<int> TeamIndex = new List<int>(); // "ΩΩ∑‘"¿« ¿Œµ¶Ω∫
    //private List<GameObject> PrefabList = new List<GameObject>();
    private List<Creature> TeamSummonList = new List<Creature>();

    private void GenerateCoordTile()
    {
        coordTileList.Add(new Vector3(0.0f, 0.0f, 0.0f));
        coordTileList.Add(new Vector3(0.0f, 0.0f, -1.5f));
        coordTileList.Add(new Vector3(0.0f, 0.0f, -3.0f));

        coordTileList.Add(new Vector3(-1.5f, 0.0f, -1.0f));
        coordTileList.Add(new Vector3(-1.5f, 0.0f, -2.5f));

        coordTileList.Add(new Vector3(-3.0f, 0.0f, 0.0f));
        coordTileList.Add(new Vector3(-3.0f, 0.0f, -1.5f));
        coordTileList.Add(new Vector3(-3.0f, 0.0f, -3.0f));
    }

    private void GetTeamIndex()
    {
        var TeamInven = UIManager.instance.InvenList[2].GetComponent<Inventory_Team>();
        TeamInven.ReadyTeamIndex();
        TeamIndex = TeamInven.TeamIndex;
        TeamSlotIndex = TeamInven.TeamSlotIndex;
    }

    private void GenerateHeroCreature()
    {
        var TeamList = UIManager.instance.InvenList[0].GetComponent<Inventory_Hero>().HeroDataList;

        for (int i = 0; i < 1; i++)
        {
            Creature LoadCreature =  Resources.Load<Creature>("_Prefabs/Creature/" + TeamList[TeamIndex[i]].Name + "_Prefab");
            Creature instance = Instantiate(LoadCreature);

            CreatureData tmpData = TeamList[TeamIndex[i]].DeepCopy();
            instance.MyStatus = tmpData;
            instance.OriginStatus = tmpData.DeepCopy();
            instance.transform.position = coordTileList[TeamSlotIndex[i]];
            instance.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 90.0f, 0.0f));
            instance.gameObject.layer = LayerMask.NameToLayer("Player");
            instance.TargetlayerMask = 1 << LayerMask.NameToLayer("Enemy");

            TeamSummonList.Add(instance);
        }
    }

    private void GenerateEnemyCreature()
    {
        var TeamList = UIManager.instance.InvenList[0].GetComponent<Inventory_Hero>().HeroDataList;

        for (int i = 0; i < TeamSlotIndex.Count; i++)
        {
            Creature LoadCreature = Resources.Load<Creature>("_Prefabs/Creature/" + TeamList[TeamIndex[i]].Name + "_Prefab");

            Creature instance = Instantiate(LoadCreature);

            CreatureData tmpData = TeamList[TeamIndex[i]].DeepCopy();
            instance.MyStatus = tmpData;
            instance.OriginStatus = tmpData.DeepCopy();
            instance.transform.position = coordTileList[TeamSlotIndex[i]] + new Vector3(5.0f, 0.0f, 0.0f);
            instance.transform.rotation = Quaternion.Euler(new Vector3(0.0f, -90.0f, 0.0f));
            instance.gameObject.layer = LayerMask.NameToLayer("Enemy");
            instance.TargetlayerMask = 1 << LayerMask.NameToLayer("Player");

            TeamSummonList.Add(instance);
        }
    }

    private void Awake()
    {
        GenerateCoordTile();
        GetTeamIndex();

        GenerateHeroCreature();
        GenerateEnemyCreature();

    }
    private void Start()
    {

    }

    void Update()
    {
        
    }
}
