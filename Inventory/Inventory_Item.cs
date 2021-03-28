using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_Item : Inventory
{
    private const int Equipment_SlotIndex = 0;
    private const int Material_SlotIndex = 1;
    private const int Etc_SlotIndex = 2;
    private const int InventoryIndexMax = 3;

    private const int Default_SlotCount = 32;

    private int SlotCountMax = 64;
    private int SlotCountCur = 32;

    List<List<Item>> MainInventory = new List<List<Item>>();
    List<List<InventorySlot>> MainInventorySlot = new List<List<InventorySlot>>();

    InventorySlot SlotPrefab = null;

    public GameObject Equipment_Inven;

    public override void OnActive(bool boolen)
    {
        base.OnActive(boolen);
    }

    public void AddItem(Item item)
    {

    }

    private void SetupInventory()
    {
        for (int i = 0; i < InventoryIndexMax; i++)
        {
            MainInventory.Add(new List<Item>());
            MainInventorySlot.Add(new List<InventorySlot>());
        }
    }

    private void SetupSlot()
    {
        for (int i = 0; i < InventoryIndexMax; i++)
        {
            for (int j = 0; j < Default_SlotCount; j++)
            {
                MainInventorySlot[i].Add(Instantiate(SlotPrefab, Equipment_Inven.transform));
            }
        }
    }

    private void LoadUIPrefabs()
    {
        SlotPrefab = Resources.Load<InventorySlot>("_Prefabs/UI/InventorySlot");
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        LoadUIPrefabs();
        SetupInventory();
        SetupSlot();

        gameObject.SetActive(false);
    }

    void Update()
    {

    }
}
