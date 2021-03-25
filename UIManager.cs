using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }

            return m_instance;
        }
    }

    private static UIManager m_instance;

    public Inventory_Item ItemInventory = null;
    public Inventory_Hero HeroInventory = null;

    public void ActiveInventory(bool boolen)
    {
        ItemInventory.gameObject.SetActive(boolen);
    }

    public void ActiveHeroInventory(bool boolen)
    {
        HeroInventory.gameObject.SetActive(boolen);
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        HeroInventory.gameObject.SetActive(false);
        ItemInventory.gameObject.SetActive(false);
    }

    void Update()
    {
        
    }
}
