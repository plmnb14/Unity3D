using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button_ItemInven : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Open();
    }

    private void Open()
    {
        UIManager.instance.OnActiveInen(1);
    }
}
