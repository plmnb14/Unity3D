using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TeamMiniPortrait : Portrait, IPointerClickHandler
{
    public int slotIndex { get; set; } = -1;

    public bool hasCreature { get; set; } = false;
    private bool Clicked = false;

    private Image ClickedShadow = null;
    private Inventory_Team TeamInven = null;

    public void OnPointerClick(PointerEventData eventData)
    {
        // 이미 보유중인 영웅이 있다면 기본 상태로.
        if(hasCreature)
        {
            if (!UIManager.instance.InvenList[2].GetComponent<Inventory_Team>().ClickedTeamPortrait)
            {
                if (null != TeamInven.ClickedMiniPortrait)
                {
                    TeamInven.ClickedMini = false;
                    TeamInven.ClickedMiniPortrait.ClickedShadow.gameObject.SetActive(false);
                    TeamInven.ClickedMiniPortrait = null;
                }

                TeamInven.TeamPortraitList[InvenIndex].Clicked = false;
                TeamInven.TeamPortraitList[InvenIndex].OnSelect();

                InvenIndex = -1;
                CreatureName = default_ImageName;
                hasCreature = false;

                base.ChangeImage("UI/MiniPortrait/" + CreatureName);

                TeamInven.TeamCount -= 1;
            }
        }

        else
        {
            // 이미 클릭한게 있다면
            if (UIManager.instance.InvenList[2].GetComponent<Inventory_Team>().ClickedTeamPortrait)
            {
                InvenIndex = TeamInven.ClickedIndex;
                CreatureName = UIManager.instance.InvenList[0].GetComponent<Inventory_Hero>().HeroDataList[InvenIndex].Name;
                TeamInven.ClickedTeamPortrait = false;
                TeamInven.ClickedIndex = -1;
                TeamInven.TeamPortraitList[InvenIndex].linkedSlotIndex = slotIndex;

                base.ChangeImage("UI/MiniPortrait/" + CreatureName);
                hasCreature = true;

                TeamInven.TeamCount += 1;
            }

            // 미니 초상화 상호작용
            else
            {
                if (!TeamInven.ClickedMini)
                {
                    Clicked = true;
                    ClickedShadow.gameObject.SetActive(true);

                    TeamInven.ClickedMini = true;
                    TeamInven.ClickedMiniPortrait = this;
                }

                else
                {
                    if(Clicked)
                    {
                        ResetDefault();
                    }
                }
            }
        }
    }

    public void ResetOverlapDefault()
    {
        if (null != TeamInven.ClickedMiniPortrait)
        {
            TeamInven.ClickedMini = false;
            TeamInven.ClickedMiniPortrait.ClickedShadow.gameObject.SetActive(false);
        }

        Clicked = false;
        ClickedShadow.gameObject.SetActive(false);

        CreatureName = default_ImageName;
        hasCreature = false;

        base.ChangeImage("UI/MiniPortrait/" + CreatureName);
    }

    public void ResetDefault()
    {
        Clicked = false;
        ClickedShadow.gameObject.SetActive(false);

        TeamInven.ClickedMini = false;
        TeamInven.ClickedMiniPortrait = null;

        CreatureName = default_ImageName;
        hasCreature = false;

        base.ChangeImage("UI/MiniPortrait/" + CreatureName);
    }

    public void ChangePortrait()
    {
        Debug.Log(CreatureName);
        base.ChangeImage("UI/MiniPortrait/" + CreatureName);

        ClickedShadow.gameObject.SetActive(false);
        Clicked = false;
    }

    private void LoadChild()
    {
        ClickedShadow = transform.GetChild(0).gameObject.GetComponent<Image>();
    }

    private void Awake()
    {
        base.GetImageComponent();
    }

    void Start()
    {
        TeamInven = UIManager.instance.InvenList[2].GetComponent<Inventory_Team>();

        LoadChild();
        base.ChangeImage("UI/MiniPortrait/" + CreatureName);
    }
}
