using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_Hero : Inventory
{
    const int Default_SlotCount = 4;

    public List<CreatureData> HeroDataList { get; } = new List<CreatureData>();
    List<HeroPortrait> HeroPortraitList = new List<HeroPortrait>();
    List<HeroMiniPortarit> HeroMiniPortraitList = new List<HeroMiniPortarit>();

    HeroPortrait PortraitPrefabs = null;
    HeroMiniPortarit MiniPortaritPrefabs = null;

    PopUpUI HeroScrollView = null;
    PopUpUI MiniScrollView = null;

    public GameObject HeroPortraitInven;
    public GameObject HeroMiniPortraitInven;

    private DataStruct DataManager;

    public override void OnActive(bool boolen)
    {
        if (!HeroScrollView.gameObject.activeInHierarchy)
        {
            OnActiveFirst(boolen);
        }

        else
        {
            OnActiveSecond(boolen);
        }
    }

    private void SetUpPortrait() // 쓸지말지 고민중
    {
        for (int i = 0; i < Default_SlotCount; i++)
        {
            HeroPortraitList.Add(Instantiate(PortraitPrefabs, HeroPortraitInven.transform));
            HeroMiniPortraitList.Add(Instantiate(MiniPortaritPrefabs, HeroMiniPortraitInven.transform));
        }
    }

    private void LoadUIPrefabs()
    {
        PortraitPrefabs = Resources.Load<HeroPortrait>("_Prefabs/UI/HeroPortrait");
        MiniPortaritPrefabs = Resources.Load<HeroMiniPortarit>("_Prefabs/UI/HeroMiniPortrait");
    }

    private void LoadChild()
    {
        HeroScrollView = transform.GetChild(2).gameObject.GetComponent<PopUpUI>();
        MiniScrollView = transform.GetChild(3).gameObject.GetComponent<PopUpUI>();
    }

    private void LoadHeroInvenData()
    {
        DataManager = new DataStruct();

        List<CreatureData> tmpData;
        DataManager.LoadCreatureData<CreatureData>(out tmpData, "Player_HeroData.json");

        CreateHerotoInven(ref tmpData);
        SetupInvenImage(ref tmpData);
    }

    private void CreateHerotoInven(ref List<CreatureData> dataList)
    {
        foreach(var data in dataList)
            HeroDataList.Add(data);
    }

    private void SetupInvenImage(ref List<CreatureData> dataList)
    {
        for(int i = 0; i < HeroDataList.Count; i++)
        {
            HeroPortraitList.Add(Instantiate(PortraitPrefabs, HeroPortraitInven.transform));
            HeroPortraitList[i].CreatureName = dataList[i].Name;
            HeroPortraitList[i].InvenIndex = i;

            HeroMiniPortraitList.Add(Instantiate(MiniPortaritPrefabs, HeroMiniPortraitInven.transform));
            HeroMiniPortraitList[i].CreatureName = dataList[i].Name;
            HeroMiniPortraitList[i].InvenIndex = i;
        }
    }

    private void OnActiveFirst(bool boolen)
    {
        if (boolen)
        {
            UIManager.instance.UManagement.PushOnUIStack(this);
            UIManager.instance.UManagement.PushOnUIStack(HeroScrollView);
        }
        else
        {
            UIManager.instance.UManagement.PopOnUIStack();
        }
    }

    private void OnActiveSecond(bool boolen)
    {
        if (boolen)
        {
            UIManager.instance.UManagement.ActivePreOnUIStack(!boolen);
            UIManager.instance.UManagement.PushOnUIStack(MiniScrollView);
        }

        else
        {
            UIManager.instance.UManagement.ActivePreOnUIStack(true);
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        LoadChild();
        LoadUIPrefabs();
    }

    void Start()
    {
        LoadHeroInvenData();

        MiniScrollView.OnActive(false);
        HeroScrollView.OnActive(false);
        gameObject.SetActive(false);
    }

    void Update()
    {

    }
}
