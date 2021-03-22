using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public int CreateCount = 10;

    public static ObjectManager instance
    {
        get
        {
            if(m_instnace == null)
            {
                m_instnace = FindObjectOfType<ObjectManager>();
            }

            return m_instnace;
        }
    }
    private static ObjectManager m_instnace;

    public Arrow ArrowPrefab;
    private Queue<Arrow> ArrowQueue = new Queue<Arrow>();

    private void Awake()
    {
        if(instance != this)
        {
            Destroy(gameObject);
        }

        else
        {
            CreateArrow(CreateCount);
        }
    }

    void CreateArrow(int Count)
    {
        for (int i = 0; i < Count; i++)
        {
            ArrowQueue.Enqueue(CreateNewArrow());
        }
    }

    Arrow CreateNewArrow()
    {
        Arrow newArrow = Instantiate(ArrowPrefab).GetComponent<Arrow>();
        newArrow.gameObject.SetActive(false);
        newArrow.transform.position = Vector3.zero;

        return newArrow;
    }

    public static Arrow GetObject()
    {
        if(instance.ArrowQueue.Count > 0)
        {
            Arrow arrow = instance.ArrowQueue.Dequeue();
            arrow.gameObject.SetActive(true);

            return arrow;
        }

        else
        {
            Arrow arrow = instance.CreateNewArrow();
            arrow.gameObject.SetActive(true);

            return arrow;
        }
    }

    public static void ReturnObject(Arrow Object)
    {
        Object.gameObject.SetActive(false);
        Object.transform.position = Vector3.zero;
        instance.ArrowQueue.Enqueue(Object);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
