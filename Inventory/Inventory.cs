using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : PopUpUI
{
    public override void OnActive(bool boolen)
    {
        this.gameObject.SetActive(boolen);
    }
}
