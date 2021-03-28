using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button_ReadyToEnter : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        var Inven = UIManager.instance.InvenList[2];
        Inven.OnActive(true);
        Inven.GetComponent<Inventory_Team>().ActiveEnterDungeonButton(true);
    }
}
