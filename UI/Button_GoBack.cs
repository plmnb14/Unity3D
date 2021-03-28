using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button_GoBack : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GoBack();
    }

    void GoBack()
    {
        var UIManagement = UIManager.instance.UManagement;
        UIManagement.PopOnUIStack();
        UIManagement.CheckPreStack();
    }
}
