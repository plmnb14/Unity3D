using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Portrait : MonoBehaviour
{
    protected Image HeroImage = null;
    public CreatureData MyStatus { get; set; } = null;

    protected virtual void ChangePortrait() { }

    protected void GetImageComponent()
    {
        HeroImage = GetComponent<Image>();
    }
}
