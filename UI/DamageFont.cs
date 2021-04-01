using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PaybackObject(DamageFont obj);
public class DamageFont : MonoBehaviour
{
    public enum DamageFontTypes { Default, Heal, Critical, Block, DmgFontType_End };

    private event PaybackObject payback;
    const int maxSize = 5;

    SpriteRenderer[] fontChild = new SpriteRenderer[maxSize];
    Sprite[][] fontSprites = new Sprite[(int)DamageFontTypes.DmgFontType_End][];

    DamageFontTypes myType = DamageFontTypes.Default;
    float transformSpeed = 3.0f;
    float scaleSpeed = 8.0f;
    float fontLifeTime;
    Vector3 endPosition = new Vector3();

    public void SetupFont(float number, Vector3 position, DamageFontTypes type, float lifeTime = 1.0f)
    {
        this.gameObject.SetActive(true);
        myType = type;

        payback += StageBattleManager.instance.ReturnDamageFont;

        transform.localScale = Vector3.one * 2.0f;
        transform.position = position + Vector3.up * 1.6f;
        fontLifeTime = lifeTime;

        int integerNum = (int)number;
        int[] num = new int[maxSize];

        num[0] = integerNum / 10000; // 만의 자리
        integerNum -= num[0] * 10000;

        num[1] = integerNum / 1000; // 천의 자리
        integerNum -= num[1] * 1000;

        num[2] = integerNum / 100; // 백의 자리
        integerNum -= num[2] * 100;

        num[3] = integerNum / 10;
        integerNum -= num[3] * 10;

        num[4] = integerNum;

        bool checkZero = false;
        for(int i = 0; i < maxSize; i++)
        {
            if(!checkZero)
            {
                while(i < maxSize)
                {
                    if (num[i] <= 0)
                    {
                        i++;
                    }
                     
                    else
                    {
                        i--;
                        break;
                    }
                }
                checkZero = true;
            }

            else
            {
                fontChild[i].gameObject.SetActive(true);
                fontChild[i].sprite = fontSprites[(int)myType][num[i]];
            }
        }

        StartCoroutine(LifeCycle());
    }

    public void SpriteLoad(string path)
    {
        for(int i = 0; i < 2; i++)
        {
            fontSprites[i] = Resources.LoadAll<Sprite>(path + i);
        }
    }

    WaitForEndOfFrame waitFrame = new WaitForEndOfFrame();
    private IEnumerator LifeCycle()
    {
        while (fontLifeTime > 0)
        {
            if (transform.localScale.x > 0.5f)
                transform.localScale -= Vector3.one * Time.deltaTime * scaleSpeed;
            else
            {
                transform.position += Vector3.up * Time.deltaTime * transformSpeed;
                transformSpeed -= Time.deltaTime * 1.5f;

                foreach (var font in fontChild)
                {
                    font.color = new Color(font.color.r, font.color.g, font.color.b, font.color.a - Time.deltaTime * 1.5f);
                }
            }

            fontLifeTime -= Time.deltaTime;
            yield return waitFrame;
        } 

        foreach(var font in fontChild)
        {
            font.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            font.gameObject.SetActive(false);
        }

        fontLifeTime = 0.0f;
        payback(this);
    }

    private void Awake()
    {
        fontChild = gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach(var child in fontChild)
        {
            child.gameObject.SetActive(false);
        }
    }
}
