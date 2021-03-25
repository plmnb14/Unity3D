using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoBackButton : MonoBehaviour
{
    Button MyButton = null;

    void Start()
    {
        MyButton = transform.GetComponent<Button>();
        MyButton.onClick.AddListener(GoBack);
    }

    void GoBack()
    {
        UIManager.instance.PopOnUIStack();
        UIManager.instance.CheckPreStack();
    }
}
