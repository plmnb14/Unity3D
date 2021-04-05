using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveCount : PopUpUI
{
    private int maxWaveCount;
    private int curWaveCount;
    private Text waveCountText;
    private CanvasGroup canvasGroup;

    public void SetUp(int maxCount)
    {
        maxWaveCount = maxCount;
        waveCountText.text = "Wave :  " +  curWaveCount + " / " + maxWaveCount;
    }

    public void Progess()
    {
        StartCoroutine(FadeIn(canvasGroup));
    }

    public void UpdateCount()
    {
        if(curWaveCount < maxWaveCount)
        {
            ++curWaveCount;
            waveCountText.text = "Wave :  " + curWaveCount + " / " + maxWaveCount;
        }
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.0f;
        waveCountText = transform.GetChild(1).gameObject.GetComponent<Text>();
    }
}
