using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroPortrait : MonoBehaviour
{
    private Image HeroImage = null;

    public void ChangePortarit(string HeroName)
    {
        HeroImage.sprite = Resources.Load<Sprite>("UI/Portrait/01_Viking");
    }

    void Start()
    {
        HeroImage = GetComponent<Image>();
    }

    void Update()
    {
        
    }
}
