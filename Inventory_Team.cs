using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Team : PopUpUI
{
    List<TeamPortrait> TeamPortraitList = new List<TeamPortrait>();

    TeamPortrait TeamPortraitPrefabs = null;

    public GameObject TeamPortrait;

    public void OnActive(bool boolen)
    {
        UIManager.instance.PushOnUIStack(this);
    }

    private void CopyInvenData()
    {
        int count = UIManager.instance.HeroInventory.HeroInventoryList.Count;

        for(int i = 0; i < count; i++)
        {
            TeamPortraitList.Add(Instantiate(TeamPortraitPrefabs, TeamPortrait.transform));
        }
    }

    private void LoadUIPrefabs()
    {
        TeamPortraitPrefabs = Resources.Load<TeamPortrait>("_Prefabs/UI/Hero Portrait");
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        LoadUIPrefabs();
        CopyInvenData();
    }

    private void Start()
    {
        CopyInvenData();
    }
}
