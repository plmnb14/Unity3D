using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_07 : ArcherType
{
    void Start()
    {
        base.Initialize();
    }

    void Update()
    {
        CheckState();
        AttackTimer();
    }
}