using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dungeon_BattleResultPopUp : PopUpUI
{
    private CanvasGroup canvasGroup;
    public void Progress()
    {
        StartCoroutine(FadeIn(canvasGroup, true, true, 2.0f, 2.0f,
            () => this.gameObject.SetActive(false),
            () => LoadingManager.LoadingScene("Scene_StageSelect")));
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.0f;
    }
}
