using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager instance
    {
        get
        {
            if (null == m_instance)
            {
                m_instance = FindObjectOfType<ParticleManager>();
            }

            return m_instance;
        }
    }

    private static ParticleManager m_instance;


    private Dictionary<string, Queue<GameObject>> particleDic;
    private int particleCount;

    public void ReturnParticle(GameObject particle, string name)
    {
        if (particleDic.ContainsKey(name))
        {
            particle.transform.SetParent(this.transform);
            particle.transform.localPosition = Vector3.zero;
            particle.SetActive(false);
            particle.GetComponent<ParticleSystem>().time = 0.0f;
            particle.GetComponent<ParticleSystem>().Clear();
            particleDic[name].Enqueue(particle);
        }
    }

    public GameObject GetParticle(string name)
    {
        if(particleDic.ContainsKey(name))
        {
            if (particleDic[name].Count == 1)
            {
                GameObject obj = Instantiate(particleDic[name].Peek());
                particleDic[name].Peek().SetActive(true);
                CreatreParticle(particleDic[name], obj);

                return particleDic[name].Dequeue();
            }

            else if (particleDic[name].Count > 0)
            {
                particleDic[name].Peek().SetActive(true);
                return particleDic[name].Dequeue();
            }
        }

        return null;
    }

    public void AddParticle(string name, string path)
    {
        if(!particleDic.ContainsKey(name))
        {
            Queue<GameObject> particleQueue = new Queue<GameObject>();
            CreatreParticle(particleQueue, path);

            particleDic.Add(name, particleQueue);
        }
    }

    private void CreatreParticle(Queue<GameObject> queue, string path)
    {
        for (int i = 0; i < particleCount; i++)
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>(path));
            obj.transform.SetParent(this.transform);
            obj.transform.position = Vector3.zero;
            obj.SetActive(false);
            queue.Enqueue(obj);
        }
    }

    private void CreatreParticle(Queue<GameObject> queue, GameObject gameObject)
    {
        for (int i = 0; i < particleCount; i++)
        {
            GameObject obj = Instantiate(gameObject);
            obj.transform.SetParent(this.transform);
            obj.transform.position = Vector3.zero;
            obj.SetActive(false);
            queue.Enqueue(obj);
        }
    }

    private void CommonParticle()
    {
        //blood
        string path = "_Prefabs/Effect/Hit_Blood_Effect_01";
        AddParticle("Blood_Small", path);

        //heal
        path = "Polygon Arsenal/Prefabs/Interactive/Healing/HealOnce";
        AddParticle("Heal_Once", path);

        //heal_Under
        path = "Polygon Arsenal/Prefabs/Combat/Buff/BuffGreen";
        AddParticle("Heal_Under", path);

        //hit
        path = "_Prefabs/Effect/Hit_Effect_01";
        AddParticle("Hit_Small", path);
    }

    private void Awake()
    {
        particleCount = 10;
        particleDic = new Dictionary<string, Queue<GameObject>>();

        CommonParticle();
    }
}
