using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TeamPortrait : Portrait, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
    }

    protected override void ChangePortrait()
    {
        HeroImage.sprite = Resources.Load<Sprite>("UI/Portrait/" + MyStatus.Name);
    }

    private void Awake()
    {
        HeroImage = GetComponent<Image>();
    }

    void Start()
    {
        ChangePortrait();
    }
}
