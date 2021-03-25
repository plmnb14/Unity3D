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
    public Inventory_Team TeamInventory = null;

    private Stack<PopUpUI> UIStack = new Stack<PopUpUI>();
    private Stack<PopUpUI> PreUIStack = new Stack<PopUpUI>();

    public Inventory_Hero GetHeroInventory()
    {
        return HeroInventory;
    }

    public void CheckPreStack()
    {
        if(PreUIStack.Count > 0)
        {
            UIStack.Push(PreUIStack.Pop());
            UIStack.Peek().PopUPActive(true);
        }
    }

    public void PushOnUIStack(PopUpUI UI)
    {
        UI.PopUPActive(true);
        UIStack.Push(UI);
    }

    public void PopOnUIStack()
    {
        if (UIStack.Count > 0)
        {
            UIStack.Pop().PopUPActive(false);

            if(UIStack.Count == 1 && PreUIStack.Count == 0)
            {
                UIStack.Pop().PopUPActive(false);
            }
        }
    }

    public void ActivePreOnUIStack(bool boolen)
    {
        if (UIStack.Count > 0)
        {
            PreUIStack.Push(UIStack.Pop());
            PreUIStack.Peek().PopUPActive(false);
        }
    }

    public void ActiveInventory(bool boolen)
    {
        ItemInventory.gameObject.SetActive(boolen);
    }

    public void ActiveHeroInventory(bool boolen)
    {
        HeroInventory.OnActiveFirst(boolen);
    }

    public void ActiveMiniPortraitInventory(bool boolen)
    {
        HeroInventory.OnActiveMini(boolen);
    }

    public void ActiveTeamInventory(bool boolen)
    {
        TeamInventory.OnActive(boolen);
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
    }

    void Update()
    {
        
    }
}
