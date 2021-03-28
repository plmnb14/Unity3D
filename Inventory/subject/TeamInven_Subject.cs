using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamInven_Subject : MonoBehaviour, Subject
{
    public delegate void event_handler();
    public event_handler eventHandler;
}
