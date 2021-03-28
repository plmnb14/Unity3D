using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldHPSlider : MonoBehaviour
{
    private Slider HpSlider;

    public void OnDie()
    {
        this.gameObject.SetActive(false);
    }

    public void ChangeHealth(float health)
    {
        HpSlider.value = health;

        if (health <= 0.0f)
        {
            OnDie();
        }
    }

    public void SetUpHealth(float health)
    {
        HpSlider.maxValue = health;
        HpSlider.value = health;
    }

    public void SetupData()
    {
        HpSlider = transform.GetChild(0).GetComponent<Slider>();
        HpSlider.gameObject.SetActive(true);
    }

    void Start()
    {

    }
}
