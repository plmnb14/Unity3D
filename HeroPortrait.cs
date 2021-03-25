using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HeroPortrait : Portrait, IPointerClickHandler
{

    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.instance.ActiveMiniPortraitInventory(true);
    }

    protected override void ChangePortrait()
    {
        HeroImage.sprite = Resources.Load<Sprite>("UI/Portrait/" + MyStatus.Name);
    }

    private void Awake()
    {
        GetImageComponent();
    }

    void Start()
    {
        ChangePortrait();
    }

    void Update()
    {
        
    }
}
