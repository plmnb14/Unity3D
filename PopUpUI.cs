using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpUI : MonoBehaviour
{
    public virtual void PopUPActive(bool boolen)
    {
        gameObject.SetActive(boolen);
    }
}
