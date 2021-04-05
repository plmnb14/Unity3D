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

    private PlayerHUD playerHud;
    private const int wayPointCount = 5;
    private List<Vector3>[] coordTileList = new List<Vector3>[wayPointCount];
    private List<int> TeamSlotIndex = new List<int>();
    private List<int> TeamIndex = new List<int>();
    private List<Creature> TeamSummonList = new List<Creature>();
    private List<Creature> MonsterSummonList = new List<Creature>();
    private Queue<StageMonsterData> StageMonsterDataQueue = new Queue<StageMonsterData>();
    private Queue<Creature> MonsterReadyQueue = new Queue<Creature>();
    private Queue<DamageFont> damageFontQueue = new Queue<DamageFont>();

    private bool OnFirst = true;
    private const int damageFontCount = 30;
    private int waveCount = 0;
    public int arrivedCount { get; set; } = 0;
    private StageInfo stageData;
    private bool OnBattle = false;
    private int playerAliveCount { get; set; } = 0;
    private int MonsterAliveCount { get; set; } = 0;

    DamageFont DamageFontPrefab;

    private Dungeon_BattleResultPopUp resultUI;
    private WaveCount waveCountUI;

    public bool OnBattleStage;

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
            //damageFontCount++;
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
            TeamSummonList : MonsterSummonList;

        foreach(var creature in TeamList)
        {
            Debug.Log(TeamList.Count);

            if (null == creature)
                continue;

            switch(type)
            {
                case StatusType.HitPoint:
                    {
                        if (creature.OriginStatus.HitPoint - creature.MyStatus.HitPoint >= value)
                        {
                            value = creature.MyStatus.HitPoint;
                            tmpCreature = creature;
                        }
                        break;
                    }

                case StatusType.Mana:
                    {
                        if (creature.MyStatus.Mana < value)
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
        float distance = 25.0f;
        for(int i = 0; i < wayPointCount; i++)
        {
            List<Vector3> newList = new List<Vector3>();
            newList.Add(new Vector3(0.0f + distance * i, 0.0f, 0.0f));
            newList.Add(new Vector3(0.0f + distance * i, 0.0f, -1.5f));
            newList.Add(new Vector3(0.0f + distance * i, 0.0f, -3.0f));

            newList.Add(new Vector3(-1.5f + distance * i, 0.0f, -1.0f));
            newList.Add(new Vector3(-1.5f + distance * i, 0.0f, -2.5f));

            newList.Add(new Vector3(-3.0f + distance * i, 0.0f, 0.0f));
            newList.Add(new Vector3(-3.0f + distance * i, 0.0f, -1.5f));
            newList.Add(new Vector3(-3.0f + distance * i, 0.0f, -3.0f));

            coordTileList[i] = newList;
        }
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
            playerAliveCount++;
            Creature instance = Instantiate(Resources.Load<Creature>("_Prefabs/Creature/" + TeamList[TeamIndex[i]].Name + "_Prefab"));

            instance.OriginStatus = (CreatureData)(TeamList[TeamIndex[i]].DeepCopy());
            instance.OriginStatus.Name = TeamList[TeamIndex[i]].DeepCopy().Name;
            instance.MyStatus = (CreatureData)instance.OriginStatus.DeepCopy();
            instance.transform.position = coordTileList[0][TeamSlotIndex[i]];
            instance.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 90.0f, 0.0f));
            instance.gameObject.layer = LayerMask.NameToLayer("Player");
            instance.TargetlayerMask = 1 << LayerMask.NameToLayer("Enemy");
            instance.onDeath += OnPlayerDeadCount;

            TeamSummonList.Add(instance);
        }
    }

    public void StageData(string name)
    {
        DataStruct.LoadData<StageInfo>(out stageData, name);

        LoadLevelData(name);
    }

    public void LoadLevelData(string name)
    {
        List<StageMonsterData> monsterData = new List<StageMonsterData>();
        DataStruct.LoadData<StageMonsterData>(out monsterData, name + "_Level");

        GenerateEnemyCreature(monsterData);
        ReadyNextWave();
    }

    private void GenerateEnemyCreature(List<StageMonsterData> data)
    {
        for (int i = 0; i < stageData.summonCount; i++)
        {
            Creature instance = Instantiate(Resources.Load<Creature>("_Prefabs/Creature/" + data[i].Name + "_Prefab"));

            instance.OriginStatus = (CreatureData)DataManager.instance.MonsterDataDic[data[i].Name].DeepCopy();
            instance.MyStatus = (CreatureData)instance.OriginStatus.DeepCopy();
            instance.transform.position = data[i].position;
            instance.transform.rotation = Quaternion.Euler(new Vector3(0.0f, data[i].yRotate, 0.0f));
            instance.gameObject.layer = LayerMask.NameToLayer("Enemy");
            instance.TargetlayerMask = 1 << LayerMask.NameToLayer("Player");
            instance.gameObject.SetActive(false);

            MonsterReadyQueue.Enqueue(instance);
        }
    }

    private void OnMonsterDeadCount()
    {
        --MonsterAliveCount;

        if(MonsterAliveCount == 0)
        {
            OnBattle = false;

            if (waveCount < stageData.waveCount)
            {
                StartCoroutine(WaitForBattleEnd());
            }

            else
            {
                MonsterSummonList.Clear();
                StartCoroutine(WaitForEndStage());
            }
        }
    }

    private IEnumerator WaitForEndStage()
    {
        yield return new WaitForSeconds(2.0f);

        var TeamInven = UIManager.instance.InvenList[2].GetComponent<Inventory_Team>();
        TeamInven.ResetTeamIndex();

        resultUI.gameObject.SetActive(true);
        resultUI.Progress();
    }

    private IEnumerator WaitForBattleEnd()
    {
        foreach (var hero in TeamSummonList)
        {
            hero.CurrentState = Creature.State.Pause;
        }

        yield return new WaitForSeconds(2.0f);

        ReadyNextWave();
        MoveToNext();
    }

    private void OnPlayerDeadCount()
    {
        playerAliveCount--;

        if(playerAliveCount == 0)
        {
            // 게임 실패!
        }
    }

    private void ReadyNextWave()
    {
        MonsterSummonList.Clear();
        for (int i = 0; i < stageData.summonPerWave[waveCount]; i++)
        {
            MonsterAliveCount++;
            var monster = MonsterReadyQueue.Dequeue();
            monster.gameObject.SetActive(true);
            monster.onDeath += OnMonsterDeadCount;
            MonsterSummonList.Add(monster);
        }
    }

    public void WaveStart()
    {
        --arrivedCount;
        if (arrivedCount <= 0)
        {
            StartCoroutine(WakeUp());
        }
    }

    public IEnumerator WakeUp()
    {
        OnBattle = true;

        yield return new WaitForSeconds(2.0f);

        foreach (var hero in TeamSummonList)
        {
            hero.CurrentState = Creature.State.Idle;
            hero.UpdateNavigation();
        }

        foreach (var monster in MonsterSummonList)
        {
            monster.CurrentState = Creature.State.Idle;
        }
    }

    public void MoveToNext()
    {
        if(OnFirst)
        {
            OnFirst = false;
            waveCountUI.gameObject.SetActive(true);
            waveCountUI.Progess();

            playerHud.gameObject.SetActive(true);
            playerHud.Progress();
        }

        ++waveCount;
        waveCountUI.UpdateCount();
        for (int i = 0; i < TeamSummonList.Count; i++)
        {
            if (TeamSummonList[i].Dead)
                continue;

            TeamSummonList[i].CurrentState = Creature.State.MoveStage;
            TeamSummonList[i].targetWayPoint = coordTileList[waveCount][TeamSlotIndex[i]];
            TeamSummonList[i].UpdateNavigation();
            StartCoroutine(TeamSummonList[i].MoveStage());
            ++arrivedCount;
        }
    }

    private IEnumerator UpdateCamera()
    {
        while(OnBattleStage)
        {
            Vector3 sumVector = new Vector3();
            Vector3 tmp;

            if (!OnBattle)
            {
                int count = 0;
                foreach (var creature in TeamSummonList)
                {
                    if (creature.Dead)
                        continue;

                    ++count;
                    sumVector += creature.transform.position;
                }

                sumVector /= count;
                sumVector += Vector3.up;
                tmp = new Vector3(0.0f, 3.5f, -4.5f);
            }

            else
            {
                int count = 0;
                foreach (var creature in TeamSummonList)
                {
                    if (creature.Dead)
                        continue;

                    ++count;
                    sumVector += creature.transform.position;
                }

                foreach (var creature in MonsterSummonList)
                {
                    if (creature.Dead)
                        continue;

                    ++count;
                    sumVector += creature.transform.position;
                }

                sumVector /= count;
                sumVector += Vector3.left;
                tmp = new Vector3(0.0f, 5.2f, -6.5f);
            }

            Camera.main.transform.position = Vector3.Slerp(Camera.main.transform.position, sumVector + tmp, Time.deltaTime * 10.0f);
            Camera.main.transform.forward = Vector3.Slerp(Camera.main.transform.forward, Vector3.Normalize(sumVector - Camera.main.transform.position), Time.deltaTime * 20.0f);

            yield return null;
        }
    }

    private void ReadyUI()
    {
        playerHud = Instantiate(Resources.Load<PlayerHUD>("_Prefabs/UI/Player HUD"));
        playerHud.SetUp(ref TeamSummonList);
        playerHud.gameObject.SetActive(false);

        resultUI = Instantiate(Resources.Load<Dungeon_BattleResultPopUp>("_Prefabs/UI/BattleResult PopUp"));
        resultUI.gameObject.SetActive(false);

        waveCountUI = Instantiate(Resources.Load<WaveCount>("_Prefabs/UI/Wave UI"));
        waveCountUI.SetUp(stageData.waveCount);
        waveCountUI.gameObject.SetActive(false);

        var obj = Instantiate(Resources.Load<Dungeon_BattleStartPopUp>("_Prefabs/UI/StageProgress PopUp HUD"));
        obj.Progress();
    }

    private void Awake()
    {
        //if (this != instance)
        //{
        //    Destroy(gameObject);
        //}

        //DontDestroyOnLoad(gameObject);

        GenerateCoordTile();
        GetTeamIndex();

        StageData(LoadingManager.nextScene);
        GenerateHeroCreature();

        GenerateDamageFont();

        ReadyUI();

        OnBattleStage = true;
    }

    private void Start()
    {
        StartCoroutine(UpdateCamera());
    }
}
