using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TeamPortrait : Portrait, IPointerClickHandler
{
    public bool Clicked { get; set; } = false;
    private Image ClickedShadow = null;
    private Inventory_Team TeamInven = null;

    public int linkedSlotIndex { get; set; } = -1;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!Clicked)
        {
            if (TeamInven.ClickedMini)
            {
                TeamInven.ClickedMiniPortrait.InvenIndex = InvenIndex;
                TeamInven.ClickedMiniPortrait.CreatureName = CreatureName;
                TeamInven.ClickedMiniPortrait.ChangePortrait();
                linkedSlotIndex = TeamInven.ClickedMiniPortrait.slotIndex;

                TeamInven.ClickedMiniPortrait.hasCreature = true;
                TeamInven.ClickedMiniPortrait = null;
                TeamInven.ClickedMini = false;

                Clicked = true;
                ClickedShadow.gameObject.SetActive(true);

                TeamInven.TeamCount += 1;
            }

            else if (!TeamInven.ClickedTeamPortrait)
            {
                Clicked = true;
                OnSelect();
            }
        }

        else
        {
            if(TeamInven.ClickedMini)
            {
                TeamInven.TeamMiniPortrait[linkedSlotIndex].ResetOverlapDefault();

                TeamInven.ClickedMiniPortrait.InvenIndex = InvenIndex;
                TeamInven.ClickedMiniPortrait.CreatureName = CreatureName;
                TeamInven.ClickedMiniPortrait.ChangePortrait();
                linkedSlotIndex = TeamInven.ClickedMiniPortrait.slotIndex;

                TeamInven.ClickedMiniPortrait = null;
                TeamInven.ClickedMini = false;

                Clicked = true;
                ClickedShadow.gameObject.SetActive(true);

                // 기존에 링크된 애는 포기화
                // 새로운 애한테 할당
            }

            else
            {
                Clicked = false;
                OnSelect();

                if(linkedSlotIndex != -1)
                {
                    TeamInven.TeamCount -= 1;
                    TeamInven.TeamMiniPortrait[linkedSlotIndex].ResetDefault();
                }
            }
        }
    }

    public void OnSelect()
    {
        ClickedShadow.gameObject.SetActive(Clicked);
        TeamInven.ClickedTeamPortrait = Clicked;
        TeamInven.ClickedIndex = Clicked ? InvenIndex : -1;
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
        ChangeImage("UI/Portrait/" + CreatureName);
        ClickedShadow.gameObject.SetActive(false);
    }
}
