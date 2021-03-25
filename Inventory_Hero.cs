using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_Hero : MonoBehaviour
{

    List<Creature> HeroInventory = new List<Creature>();
    List<HeroPortrait> HeroPortrait = new List<HeroPortrait>();

    const int Default_SlotCount = 4;

    HeroPortrait PortraitPrefabs = null;
    public GameObject HeroInven;

    private void SetUpPortrait()
    {
        for (int i = 0; i < Default_SlotCount; i++)
        {
            HeroPortrait.Add(Instantiate(PortraitPrefabs, HeroInven.transform));
        }

        HeroPortrait[0].ChangePortarit("01_Viking");
        HeroPortrait[1].ChangePortarit("02_Viking");
        HeroPortrait[2].ChangePortarit("03_Viking");
        HeroPortrait[3].ChangePortarit("03_Viking");
    }

    private void LoadUIPrefabs()
    {
        PortraitPrefabs = Resources.Load<HeroPortrait>("_Prefabs/UI/Hero Portrait");
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        LoadUIPrefabs();
        SetUpPortrait();
    }

    void Update()
    {
        //HeroPortrait[0].ChangePortarit("01_Viking");
        //HeroPortrait[1].ChangePortarit("02_Viking");
        //HeroPortrait[2].ChangePortarit("03_Viking");
        //HeroPortrait[3].ChangePortarit("03_Viking");
    }
}
