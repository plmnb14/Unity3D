using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveCount : MonoBehaviour
{
    private int maxWaveCount;
    private int curWaveCount;
    private Text waveCountText;

    public void SetUp(int maxCount)
    {
        maxWaveCount = maxCount;
        waveCountText.text = "Wave :  " +  curWaveCount + " / " + maxWaveCount;
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
        waveCountText = transform.GetChild(1).gameObject.GetComponent<Text>();
    }
}
