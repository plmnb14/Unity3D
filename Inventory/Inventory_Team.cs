using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void TeamCountChanged(int count);

public class Inventory_Team : Inventory
{
    public event TeamCountChanged countChanged;

    public List<TeamPortrait> TeamPortraitList { get; set; } = new List<TeamPortrait>();
    public List<TeamMiniPortrait> TeamMiniPortrait = new List<TeamMiniPortrait>();

    TeamPortrait TeamPortraitPrefabs = null;

    public GameObject TeamPortrait;

    public TeamMiniPortrait ClickedMiniPortrait { get; set; } = null;
    public bool ClickedTeamPortrait { get; set; } = false;
    public bool ClickedMini { get; set; } = false;
    public int ClickedIndex { get; set; } = 0;
    public int TeamCountMax { get; set; } = 5;
    public int TeamCount
    {   get
        {
            return TeamCountCur;
        }

        set
        {
            TeamCountCur = value;
            countChanged(TeamCountCur);
        } 
    }
    private int TeamCountCur = 0;

    public List<int> TeamIndex { get; } = new List<int>();
    public List<int> TeamSlotIndex { get; } = new List<int>();

    public TeamCountText ChildText;

    public void ActiveEnterDungeonButton(bool boolen)
    {
        transform.GetChild(4).gameObject.SetActive(boolen);
    }

    public void ReadyTeamIndex()
    {
        for(int i = 0; i < 8; i++)
        {
            if(TeamMiniPortrait[i].hasCreature)
            {
                TeamIndex.Add(TeamMiniPortrait[i].InvenIndex);
                TeamSlotIndex.Add(TeamMiniPortrait[i].slotIndex);
            }
        }
    }

    public void ResetTeamIndex()
    {
        TeamIndex.Clear();
        TeamSlotIndex.Clear();
    }

    public override void OnActive(bool boolen)
    {
        if (boolen)
        {
            UIManager.instance.UManagement.PushOnUIStack(this);

            if (TeamPortraitList.Count != UIManager.instance.InvenList[0].gameObject.GetComponent<Inventory_Hero>().HeroDataList.Count)
            {
                CopyInvenData();
            }
        }

        else if(!boolen)
        {
            UIManager.instance.UManagement.PopOnUIStack();
            ActiveEnterDungeonButton(boolen);
        }
    }

    private void CopyInvenData()
    {
        int count = UIManager.instance.InvenList[0].gameObject.GetComponent<Inventory_Hero>().HeroDataList.Count;

        for (int i = 0; i < count; i++)
        {
            TeamPortraitList.Add(Instantiate(TeamPortraitPrefabs, TeamPortrait.transform));
            TeamPortraitList[i].CreatureName = UIManager.instance.InvenList[0].gameObject.GetComponent<Inventory_Hero>().HeroDataList[i].Name;
            TeamPortraitList[i].InvenIndex = i;
        }
    }

    private void Indexing()
    {
        for(int i = 0; i < TeamMiniPortrait.Count; i++)
        {
            TeamMiniPortrait[i].slotIndex = i;
        }
    }

    private void LoadUIPrefabs()
    {
        TeamPortraitPrefabs = Resources.Load<TeamPortrait>("_Prefabs/UI/TeamPortrait");
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        LoadUIPrefabs();
        CopyInvenData();
    }

    private void Start()
    {
        countChanged += ChildText.UpdateCount;
        countChanged(TeamCountCur);

        Indexing();

        gameObject.SetActive(false);
    }
}
