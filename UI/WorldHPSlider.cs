using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldHPSlider : MonoBehaviour
{
    private Slider HpSlider;
    private GameObject buffCanvas;

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
        transform.position += Vector3.up * 2.2f;

        HpSlider = transform.GetChild(0).GetComponent<Slider>();
        HpSlider.gameObject.SetActive(true);

        buffCanvas = transform.GetChild(2).gameObject;
    }

    public void PutBuffOnGrid(BuffSkill skill)
    {
        skill.transform.SetParent(buffCanvas.transform);
        skill.transform.localPosition = Vector3.zero;
    }
}
