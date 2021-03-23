using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldHPSlider : MonoBehaviour
{
    public Slider HpSlider;
    public Creature HpTarget;
    private bool Dead;

    public void OnDie()
    {
        this.gameObject.SetActive(false);
    }

    void Start()
    {
        HpSlider.gameObject.SetActive(true);
        HpSlider.value = HpTarget.HitPoint;
        HpSlider.maxValue = HpTarget.HitPoint;

        Dead = false;
    }

    void Update()
    {
        if(!Dead)
        {
            HpSlider.value = HpTarget.HitPoint;

            if (HpTarget.HitPoint <= 0.0f)
            {
                OnDie();
            }
        }
    }
}
