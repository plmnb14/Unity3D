using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dungeon_BattleStartPopUp : PopUpUI
{
    private CanvasGroup canvasGroup;
    private bool fadeOut;

    public void Progress()
    {
        StartCoroutine(FadeInOut());
    }

    public IEnumerator FadeInOut()
    {
        StartCoroutine(FadeIn(canvasGroup, () => fadeOut = true));

        if(!fadeOut)
            yield return new WaitForEndOfFrame();

        StartCoroutine(FadeOut(canvasGroup, true, true, 2.0f, 0.5f,
            () => this.gameObject.SetActive(false),
            () => StageBattleManager.instance.MoveToNext()));
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.0f;
    }
}
