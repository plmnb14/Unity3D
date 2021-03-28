using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMiniPortarit : Portrait
{
    private void Awake()
    {
        base.GetImageComponent();
    }

    void Start()
    {
        base.ChangeImage("UI/MiniPortrait/" + CreatureName);
    }
}
