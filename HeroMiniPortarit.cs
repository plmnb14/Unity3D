using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroMiniPortarit : Portrait
{
    protected override void ChangePortrait()
    {
        HeroImage.sprite = Resources.Load<Sprite>("UI/MiniPortrait/" + MyStatus.Name);
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
