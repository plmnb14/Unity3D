using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    private Item getItem = null;
    private int itemCount = 0;
    private Image itemImage = null;

    Image Icon = null;

    public void ChangeSlot()
    {
        Icon.sprite = Resources.Load<Sprite>("UI/ItemIcon/Kills");
    }

    public void UpdateSlot()
    {

    }

    public void SlotInfoPopUp()
    {

    }

    void Start()
    {
        Icon = gameObject.transform.GetChild(1).gameObject.GetComponent<Image>();
    }

    void Update()
    {
        
    }
}
