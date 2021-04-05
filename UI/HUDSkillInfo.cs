using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDSkillInfo : MonoBehaviour
{
    private SkillData skillData;
    private Image icon;
    private Image background;
    private Text cooltimeText;
    private float curCooltime;
    private float maxCooltime;

    public void SetUp(SkillBaseData data)
    {
        icon.sprite = Resources.Load<Sprite>("UI/SkillIcon/" + data.Name);
        background.sprite = Resources.Load<Sprite>("UI/SkillIcon/" + data.Name);
        maxCooltime = data.cooltime;
    }

    public void ActiveSkill()
    {
        cooltimeText.gameObject.SetActive(true);
        curCooltime = maxCooltime;

        StartCoroutine(OnCooltime());
    }

    WaitForEndOfFrame waitFrame = new WaitForEndOfFrame();
    private IEnumerator OnCooltime()
    {
        while(curCooltime < 0.0f)
        {
            curCooltime -= Time.deltaTime;
            if (curCooltime < 0.0f)
                curCooltime = 0.0f;

            icon.fillAmount = curCooltime / maxCooltime;
            cooltimeText.text = ((int)curCooltime).ToString();

            yield return waitFrame;
        }

        cooltimeText.gameObject.SetActive(false);
    }

    private void Awake()
    {
        icon = transform.GetChild(0).GetComponent<Image>();
        background = transform.GetChild(1).GetComponent<Image>();
        cooltimeText = transform.GetChild(2).GetComponent<Text>();
        cooltimeText.gameObject.SetActive(false);
    }
}
