using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Portrait : MonoBehaviour
{
    protected const string default_ImageName = "NULL_CREATURE";
    protected Image HeroImage = null;
    public int InvenIndex { get; set; } = 0;
    public string CreatureName { get; set; } = default_ImageName;

    protected void ChangeImage(string path)
    {
        HeroImage.sprite = Resources.Load<Sprite>(path);
    }

    protected void GetImageComponent()
    {
        HeroImage = GetComponent<Image>();
    }
}
