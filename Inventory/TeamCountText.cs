using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamCountText : MonoBehaviour
{
    public Text textCount;
    private int count;

    public void UpdateCount(int num)
    {
        count = num;
        gameObject.GetComponent<Text>().text = num + " / 5";
    }

    void Start()
    {

    }

    void Update()
    {
        
    }
}
