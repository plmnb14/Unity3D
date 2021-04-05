using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUD : PopUpUI
{
    PlayerHudHeroInfo[] HeroInfo;
    CanvasGroup group;

    public void SetUp(ref List<Creature> list)
    {
        Debug.Log(list.Count);
        for(int i = 0; i < list.Count; i++)
        {
            HeroInfo[i].SetUp(list[i]);
            HeroInfo[i].gameObject.SetActive(true);
        }
    }

    public void Progress()
    {
        StartCoroutine(FadeIn(group));
    }

    private void Awake()
    {
        HeroInfo = new PlayerHudHeroInfo[4];
        for(int i = 0; i < 4; i++)
        {
            if(null != transform.GetChild(0).GetChild(i))
            {
                HeroInfo[i] = transform.GetChild(0).GetChild(i).GetComponent<PlayerHudHeroInfo>();
                HeroInfo[i].gameObject.SetActive(false);
            }
        }

        group = GetComponent<CanvasGroup>();
        group.alpha = 0.0f;
    }
}
