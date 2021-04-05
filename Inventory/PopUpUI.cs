using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpUI : MonoBehaviour
{
    float fadeSpeed = 1.0f;

    public virtual void OnActive(bool boolen)
    {
        gameObject.SetActive(boolen);
    }

    public virtual IEnumerator FadeOut(CanvasGroup canvasGroup, System.Action nextFirstEvent = null, System.Action nextSecondEvent = null)
    {
        float ratio = 1.0f;
        WaitForEndOfFrame waitFrame = new WaitForEndOfFrame();
        while (ratio > 0.0f)
        {
            ratio -= fadeSpeed * Time.deltaTime;

            canvasGroup.alpha = ratio;

            if (canvasGroup.alpha <= 0.0f)
                canvasGroup.alpha = 0.0f;

            yield return waitFrame;
        }

        if (null != nextFirstEvent)
        {
            nextFirstEvent();
        }
    
        if(null != nextSecondEvent)
        {
            nextSecondEvent();
        }
    }

    public virtual IEnumerator FadeOut(CanvasGroup canvasGroup, bool wait = false, float waitTime = 0.0f, System.Action nextFirstEvent = null, System.Action nextSecondEvent = null)
    {
        if(wait)
        {
            yield return new WaitForSeconds(waitTime);
        }

        float ratio = 1.0f;
        WaitForEndOfFrame waitFrame = new WaitForEndOfFrame();
        while (ratio > 0.0f)
        {
            ratio -= fadeSpeed * Time.deltaTime;

            canvasGroup.alpha = ratio;

            if (canvasGroup.alpha <= 0.0f)
                canvasGroup.alpha = 0.0f;

            yield return waitFrame;
        }


        if (wait)
        {
            yield return new WaitForSeconds(waitTime);
        }

        if (null != nextFirstEvent)
        {
            nextFirstEvent();
        }

        if (null != nextSecondEvent)
        {
            nextSecondEvent();
        }
    }

    public virtual IEnumerator FadeOut(CanvasGroup canvasGroup, bool wait = false, bool secondWait = false, float waitTime = 0.0f, float secondWaitTime = 0.0f, System.Action nextFirstEvent = null, System.Action nextSecondEvent = null)
    {
        if (wait)
        {
            yield return new WaitForSeconds(waitTime);
        }

        float ratio = 1.0f;
        WaitForEndOfFrame waitFrame = new WaitForEndOfFrame();
        while (ratio > 0.0f)
        {
            ratio -= fadeSpeed * Time.deltaTime;

            canvasGroup.alpha = ratio;

            if (canvasGroup.alpha <= 0.0f)
                canvasGroup.alpha = 0.0f;

            yield return waitFrame;
        }

        if (secondWait)
        {
            yield return new WaitForSeconds(secondWaitTime);
        }

        if (null != nextFirstEvent)
        {
            nextFirstEvent();
        }

        if (null != nextSecondEvent)
        {
            nextSecondEvent();
        }
    }

    public virtual IEnumerator FadeOut(CanvasGroup canvasGroup, System.Action nextEvent = null)
    {
        float ratio = 1.0f;
        WaitForEndOfFrame waitFrame = new WaitForEndOfFrame();
        while (ratio > 0.0f)
        {
            ratio -= fadeSpeed * Time.deltaTime;

            canvasGroup.alpha = ratio;

            if (canvasGroup.alpha <= 0.0f)
                canvasGroup.alpha = 0.0f;

            yield return waitFrame;
        }


        if (null != nextEvent)
        {
            nextEvent();
        }
    }

    public virtual IEnumerator FadeOut(CanvasGroup canvasGroup, bool wait = false, float waitTime = 0.0f, System.Action nextEvent = null)
    {
        if (wait)
        {
            yield return new WaitForSeconds(waitTime);
        }

        float ratio = 1.0f;
        WaitForEndOfFrame waitFrame = new WaitForEndOfFrame();
        while (ratio > 0.0f)
        {
            ratio -= fadeSpeed * Time.deltaTime;

            canvasGroup.alpha = ratio;

            if (canvasGroup.alpha <= 0.0f)
                canvasGroup.alpha = 0.0f;

            yield return waitFrame;
        }


        if (wait)
        {
            yield return new WaitForSeconds(waitTime);
        }

        if (null != nextEvent)
        {
            nextEvent();
        }
    }

    public virtual IEnumerator FadeIn(CanvasGroup canvasGroup, System.Action nextFirstEvent, System.Action nextSecondEvent = null)
    {
        float ratio = 0.0f;
        WaitForEndOfFrame waitFrame = new WaitForEndOfFrame();
        while (ratio > 1.0f)
        {
            ratio += fadeSpeed * Time.deltaTime;

            canvasGroup.alpha = ratio;

            if (canvasGroup.alpha >= 1.0f)
                canvasGroup.alpha = 1.0f;

            yield return waitFrame;
        }

        if (null != nextFirstEvent)
        {
            nextFirstEvent();
        }

        if (null != nextSecondEvent)
        {
            nextSecondEvent();
        }
    }

    public virtual IEnumerator FadeIn(CanvasGroup canvasGroup, bool wait, float waitTime, System.Action nextFirstEvent, System.Action nextSecondEvent = null)
    {
        if (wait)
        {
            yield return new WaitForSeconds(waitTime);
        }

        float ratio = 0.0f;
        WaitForEndOfFrame waitFrame = new WaitForEndOfFrame();
        while (ratio > 1.0f)
        {
            ratio += fadeSpeed * Time.deltaTime;

            canvasGroup.alpha = ratio;

            if (canvasGroup.alpha >= 1.0f)
                canvasGroup.alpha = 1.0f;

            yield return waitFrame;
        }

        if (wait)
        {
            yield return new WaitForSeconds(waitTime);
        }

        if (null != nextFirstEvent)
        {
            nextFirstEvent();
        }

        if (null != nextSecondEvent)
        {
            nextSecondEvent();
        }
    }

    public virtual IEnumerator FadeIn(CanvasGroup canvasGroup, bool wait, bool secondWait, float waitTime, float secondWaitTime, System.Action nextFirstEvent = null, System.Action nextSecondEvent = null)
    {
        if (wait)
        {
            yield return new WaitForSeconds(waitTime);
        }

        float ratio = 0.0f;
        WaitForEndOfFrame waitFrame = new WaitForEndOfFrame();
        while (ratio < 1.0f)
        {
            ratio += fadeSpeed * Time.deltaTime;

            canvasGroup.alpha = ratio;

            if (canvasGroup.alpha >= 1.0f)
                canvasGroup.alpha = 1.0f;

            yield return waitFrame;
        }

        if (secondWait)
        {
            yield return new WaitForSeconds(secondWaitTime);
        }

        if (null != nextFirstEvent)
        {
            nextFirstEvent();
        }

        if (null != nextSecondEvent)
        {
            nextSecondEvent();
        }
    }

    public virtual IEnumerator FadeIn(CanvasGroup canvasGroup, System.Action nextEvent = null)
    {
        float ratio = 0.0f;
        WaitForEndOfFrame waitFrame = new WaitForEndOfFrame();
        while (ratio < 1.0f)
        {
            ratio += fadeSpeed * Time.deltaTime;

            canvasGroup.alpha = ratio;

            if (canvasGroup.alpha >= 1.0f)
                canvasGroup.alpha = 1.0f;

            yield return waitFrame;
        }

        if (null != nextEvent)
        {
            nextEvent();
        }
    }

    public virtual IEnumerator FadeIn(CanvasGroup canvasGroup, bool wait, float waitTime, System.Action nextEvent = null)
    {
        if (wait)
        {
            yield return new WaitForSeconds(waitTime);
        }

        float ratio = 0.0f;
        WaitForEndOfFrame waitFrame = new WaitForEndOfFrame();
        while (ratio > 1.0f)
        {
            ratio += fadeSpeed * Time.deltaTime;

            canvasGroup.alpha = ratio;

            if (canvasGroup.alpha >= 1.0f)
                canvasGroup.alpha = 1.0f;

            yield return waitFrame;
        }

        if (wait)
        {
            yield return new WaitForSeconds(waitTime);
        }

        if (null != nextEvent)
        {
            nextEvent();
        }
    }
}
