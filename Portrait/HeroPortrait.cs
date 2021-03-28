using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeroPortrait : Portrait, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.instance.InvenList[0].OnActive(true);
    }

    private void Awake()
    {
        base.GetImageComponent();
    }

    void Start()
    {
        base.ChangeImage("UI/Portrait/" + CreatureName);
    }
}
