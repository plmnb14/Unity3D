using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dungeon_BattleStartPopUp : PopUpUI
{
    private const int childCount = 2;
    private const float waitingTime = 1.0f;
    private const float speed = 1.0f;
    private Image[] childImage = new Image[childCount];

    public void Progress()
    {
        StartCoroutine(Wait());
    }

    private WaitForSeconds waitSecond = new WaitForSeconds(waitingTime);
    private IEnumerator Wait()
    {
        yield return waitSecond;

        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        Color[] oldColor = new Color[childCount];
        for (int i = 0; i < childCount; i++)
        {
            oldColor[i] = childImage[i].color;
        }

        float ratio = 1.0f;
        while (ratio > 0.0f)
        {
            ratio -= 0.01f * speed;

            for (int i = 0; i < childCount; i++)
            {
                childImage[i].color = oldColor[i] * ratio;
            }

            yield return new WaitForEndOfFrame();
        }

        StageBattleManager.instance.MoveToNext();
        this.gameObject.SetActive(false);
    }

    private void Awake()
    {
        for (int i = 0; i < childCount; i++)
        {
            childImage[i] = this.transform.GetChild(i).GetComponent<Image>();
        }
    }
}
