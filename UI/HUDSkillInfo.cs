using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDSkillInfo : MonoBehaviour
{
    private Image icon;
    private Image foreground;
    private Text cooltimeText;
    private float curCooltime;
    private float maxCooltime;

    public void SetUp(SkillBaseData data)
    {
        if(null != data)
        {
            icon.gameObject.SetActive(true);
            icon.sprite = Resources.Load<Sprite>("UI/SkillIcon/" + data.Name);
            foreground.sprite = Resources.Load<Sprite>("UI/SkillIcon/" + data.Name);
            maxCooltime = data.cooltime;
        }
        else
        {
            icon.sprite = null;
            foreground.sprite = null;
            maxCooltime = 0.0f;
        }
    }

    public void ActiveSkill()
    {
        foreground.gameObject.SetActive(true);
        cooltimeText.gameObject.SetActive(true);
        curCooltime = maxCooltime;

        StartCoroutine(OnCooltime());
    }

    WaitForEndOfFrame waitFrame = new WaitForEndOfFrame();
    private IEnumerator OnCooltime()
    {
        while(curCooltime > 0.0f)
        {
            curCooltime -= Time.deltaTime;
            if (curCooltime < 0.0f)
                curCooltime = 0.0f;

            foreground.fillAmount = curCooltime / maxCooltime;
            cooltimeText.text = ((int)curCooltime).ToString();

            yield return waitFrame;
        }

        cooltimeText.gameObject.SetActive(false);
        foreground.gameObject.SetActive(false);
    }

    private void Awake()
    {
        icon = transform.GetChild(0).GetComponent<Image>();
        foreground = transform.GetChild(1).GetComponent<Image>();
        cooltimeText = transform.GetChild(2).GetComponent<Text>();

        icon.gameObject.SetActive(false);
        cooltimeText.gameObject.SetActive(false);
        foreground.gameObject.SetActive(false);
    }
}
