using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_Hero : PopUpUI
{
    const int Default_SlotCount = 4;

    public List<Creature> HeroInventoryList { get; } = new List<Creature>();
    List<HeroPortrait> HeroPortraitList = new List<HeroPortrait>();
    List<HeroMiniPortarit> HeroMiniPortraitList = new List<HeroMiniPortarit>();

    HeroPortrait PortraitPrefabs = null;
    HeroMiniPortarit MiniPortaritPrefabs = null;

    PopUpUI HeroScrollView = null;
    PopUpUI MiniScrollView = null;

    public GameObject HeroPortraitInven;
    public GameObject HeroMiniPortraitInven;

    private DataStruct DataManager;

    public void CopyInvenData(List<TeamPortrait> list)
    {
        for(int i = 0; i < HeroPortraitList.Count; i++)
        {
            list[i].MyStatus = HeroPortraitList[i].MyStatus;
        }
    }

    private void SetUpPortrait()
    {
        for (int i = 0; i < Default_SlotCount; i++)
        {
            HeroPortraitList.Add(Instantiate(PortraitPrefabs, HeroPortraitInven.transform));
            HeroMiniPortraitList.Add(Instantiate(MiniPortaritPrefabs, HeroMiniPortraitInven.transform));
        }
    }

    private void LoadUIPrefabs()
    {
        PortraitPrefabs = Resources.Load<HeroPortrait>("_Prefabs/UI/Hero Portrait");
        MiniPortaritPrefabs = Resources.Load<HeroMiniPortarit>("_Prefabs/UI/HeroMiniPortrait");
    }

    private void LoadChild()
    {
        HeroScrollView = transform.GetChild(2).gameObject.GetComponent<PopUpUI>();
        MiniScrollView = transform.GetChild(3).gameObject.GetComponent<PopUpUI>();
    }

    public void SaveHeroInvenData()
    {

    }

    private void LoadHeroInvenData()
    {
        DataManager = new DataStruct();

        List<CreatureData> tmpData = new List<CreatureData>();
        DataManager.LoadCreatureData<CreatureData>(out tmpData, "Player_HeroData.json");

        CreateHerotoInven(ref tmpData);
        SetupInvenImage(ref tmpData);
    }

    private void CreateHerotoInven(ref List<CreatureData> dataList)
    {
        Debug.Log(dataList[0].Name);
        for (int i = 0; i < dataList.Count; i++)
        {
            Creature instance = Instantiate(Resources.Load<Creature>("_Prefabs/Creature/" + dataList[i].Name + "_Prefab"));
            instance.SetStatus(dataList[i]);
            instance.gameObject.transform.position = Vector3.zero;
            instance.gameObject.SetActive(false);

            HeroInventoryList.Add(instance);
        }
    }

    private void SetupInvenImage(ref List<CreatureData> dataList)
    {
        for(int i = 0; i < HeroInventoryList.Count; i++)
        {
            HeroPortraitList.Add(Instantiate(PortraitPrefabs, HeroPortraitInven.transform));
            HeroPortraitList[i].MyStatus = dataList[i];

            HeroMiniPortraitList.Add(Instantiate(MiniPortaritPrefabs, HeroMiniPortraitInven.transform));
            HeroMiniPortraitList[i].MyStatus = dataList[i];
        }
    }

    public void OnActiveFirst(bool boolen)
    {
        if (boolen)
        {
            UIManager.instance.PushOnUIStack(this);
            UIManager.instance.PushOnUIStack(HeroScrollView);
        }
        else
        {
            UIManager.instance.PopOnUIStack();
        }
    }

    public void OnActiveMini(bool boolen)
    {
        if (boolen)
        {
            UIManager.instance.ActivePreOnUIStack(!boolen);
            UIManager.instance.PushOnUIStack(MiniScrollView);
        }

        else
        {
            UIManager.instance.ActivePreOnUIStack(true);
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        //LoadHeroInvenData();
        LoadChild();
        LoadUIPrefabs();
    }

    void Start()
    {
        LoadHeroInvenData();

        MiniScrollView.PopUPActive(false);
        HeroScrollView.PopUPActive(false);
        gameObject.SetActive(false);
    }

    void Update()
    {

    }
}
