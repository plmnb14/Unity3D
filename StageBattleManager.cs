using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBattleManager : MonoBehaviour
{
    public enum StatusType { HitPoint = 5, Mana, AttackDamage, Armor }

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
    private List<Creature> EnemySummonList = new List<Creature>();

    DamageFont DamageFontPrefab;
    private int damageFontCount = 30;
    private Queue<DamageFont> damageFontQueue = new Queue<DamageFont>();

    public void ReturnDamageFont(DamageFont obj)
    {
        damageFontQueue.Enqueue(obj);
    }

    public void GetDamageFont(float number, Vector3 position, DamageFont.DamageFontTypes type = DamageFont.DamageFontTypes.Default, float lifeTime = 1.0f)
    {
        if(damageFontQueue.Count > 0)
            damageFontQueue.Dequeue().SetupFont(number, position, type, lifeTime);
        else
        {
            damageFontCount++;
            CreateDamageFont();
            damageFontQueue.Dequeue().SetupFont(number, position, type, lifeTime);
        }
    }

    public void SetDamageFont()
    {
        for (int i = 0; i < damageFontCount; i++)
        {
            CreateDamageFont();
        }
    }

    public Creature FindTarget(StatusType type, LayerMask layerMask)
    {
        float value = 0.0f;
        Creature tmpCreature = null;

        var TeamList = 
            layerMask == LayerMask.NameToLayer("Player") ? 
            TeamSummonList : EnemySummonList;

        foreach(var creature in TeamList)
        {
            switch(type)
            {
                case StatusType.HitPoint:
                    {
                        if (creature.MyStatus.HitPoint > value)
                        {
                            value = creature.MyStatus.HitPoint;
                            tmpCreature = creature;
                        }
                        break;
                    }

                case StatusType.Mana:
                    {
                        if (creature.MyStatus.Mana > value)
                        {
                            value = creature.MyStatus.Mana;
                            tmpCreature = creature;
                        }
                        break;
                    }

                case StatusType.AttackDamage:
                    {
                        if (creature.MyStatus.AttackDamage > value)
                        {
                            value = creature.MyStatus.AttackDamage;
                            tmpCreature = creature;
                        }
                        break;
                    }

                case StatusType.Armor:
                    {
                        if (creature.MyStatus.Armor > value)
                        {
                            value = creature.MyStatus.Armor;
                            tmpCreature = creature;
                        }
                        break;
                    }
            }
        }

        return tmpCreature;
    }

    private void CreateDamageFont()
    {
        DamageFont dmgFont = Instantiate(DamageFontPrefab, this.transform);
        dmgFont.SpriteLoad("UI/DMG_Font_");
        dmgFont.gameObject.SetActive(false);
        damageFontQueue.Enqueue(dmgFont);
    }

    private void GenerateDamageFont()
    {
        DamageFontPrefab = Resources.Load<DamageFont>("_Prefabs/UI/DamageFont");
        SetDamageFont();
    }

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

        for (int i = 0; i < TeamSlotIndex.Count; i++)
        {
            Creature instance =  Instantiate(Resources.Load<Creature>("_Prefabs/Creature/" + TeamList[TeamIndex[i]].Name + "_Prefab"));

            CreatureData tmpData = TeamList[TeamIndex[i]].DeepCopy();
            instance.MyStatus = tmpData;
            instance.MyStatus.AttackDamage *= 1.5f;
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
            Creature instance = Instantiate(Resources.Load<Creature>("_Prefabs/Creature/" + TeamList[TeamIndex[i]].Name + "_Prefab"));

            CreatureData tmpData = TeamList[TeamIndex[i]].DeepCopy();
            instance.MyStatus = tmpData;
            instance.OriginStatus = tmpData.DeepCopy();
            instance.transform.position = new Vector3(coordTileList[TeamSlotIndex[i]].x * -1.0f, coordTileList[TeamSlotIndex[i]].y, coordTileList[TeamSlotIndex[i]].z);
            instance.transform.rotation = Quaternion.Euler(new Vector3(0.0f, -90.0f, 0.0f));
            instance.gameObject.layer = LayerMask.NameToLayer("Enemy");
            instance.TargetlayerMask = 1 << LayerMask.NameToLayer("Player");

            EnemySummonList.Add(instance);
        }
    }

    private void Awake()
    {
        GenerateCoordTile();
        GetTeamIndex();

        GenerateHeroCreature();
        GenerateEnemyCreature();

        GenerateDamageFont();
    }

    private void Start()
    {

    }
}
