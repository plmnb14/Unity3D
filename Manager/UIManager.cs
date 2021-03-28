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

    public enum InvenType { Item, Hero, Team, InvenType_End };

    public List<Inventory> InvenList = new List<Inventory>();
    public UIStackManagement UManagement { get; set; } = null;

    public void OnActiveInen(int index)
    {
        InvenList[index].OnActive(true);
    }

    public void DeActiveInen(int index)
    {
        InvenList[index].OnActive(false);
    }

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        UManagement = new UIStackManagement();
    }
}


public class UIStackManagement
{
    private Stack<PopUpUI> UIStack = new Stack<PopUpUI>();
    private Stack<PopUpUI> PreUIStack = new Stack<PopUpUI>();

    public void CheckPreStack()
    {
        if (PreUIStack.Count > 0)
        {
            UIStack.Push(PreUIStack.Pop());
            UIStack.Peek().gameObject.SetActive(true);
        }
    }

    public void PushOnUIStack(PopUpUI UI)
    {
        UI.gameObject.SetActive(true);
        UIStack.Push(UI);
    }

    public void PopOnUIStack()
    {
        if (UIStack.Count > 0)
        {
            UIStack.Pop().gameObject.SetActive(false);

            if (UIStack.Count == 1 && PreUIStack.Count == 0)
            {
                UIStack.Pop().gameObject.SetActive(false);
            }
        }
    }

    public void ActivePreOnUIStack(bool boolen)
    {
        if (UIStack.Count > 0)
        {
            PreUIStack.Push(UIStack.Pop());
            PreUIStack.Peek().gameObject.SetActive(false);
        }
    }
}
