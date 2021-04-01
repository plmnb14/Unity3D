using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public enum ProjType { Arrow, MagicMissile, TypeEnd };
    public int CreateCount = 20;

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

    private Projectile[] projPrefabs = new Projectile[(int)ProjType.TypeEnd];
    private Queue<Projectile>[] projQueue = new Queue<Projectile>[(int)ProjType.TypeEnd];

    private void Awake()
    {
        if(instance != this)
        {
            Destroy(gameObject);
        }

        for(int i = 0; i < (int)ProjType.TypeEnd; i++)
        {
            projQueue[i] = new Queue<Projectile>();
        }

        projPrefabs[(int)ProjType.Arrow] = Resources.Load<Arrow>("_Prefabs/Weapon/SM_Arrow_01");
        GenerateProjectile(ProjType.Arrow);

        projPrefabs[(int)ProjType.MagicMissile] = Resources.Load<MagicMissile>("_Prefabs/Weapon/SM_MagicMissile_01");
        GenerateProjectile(ProjType.MagicMissile);
    }

    void GenerateProjectile(ProjType type)
    {
        for (int i = 0; i < CreateCount; i++)
        {
            projQueue[(int)type].Enqueue(CreateProjectile(type));
        }
    }

    Projectile CreateProjectile(ProjType type)
    {
        Projectile newProj = Instantiate(projPrefabs[(int)type], this.transform);
        newProj.gameObject.SetActive(false);
        newProj.transform.position = Vector3.zero;

        return newProj;
    }

    public Projectile GetObject(ProjType type)
    {
        if(projQueue[(int)type].Count > 0)
        {
            Projectile arrow = projQueue[(int)type].Dequeue();
            arrow.gameObject.SetActive(true);

            return arrow;
        }

        else
        {
            Projectile newProj = CreateProjectile(type);
            newProj.gameObject.SetActive(true);

            return newProj;
        }
    }

    public static void ReturnObject(Projectile Object, ProjType type)
    {
        Object.gameObject.SetActive(false);
        Object.transform.position = Vector3.zero;
        instance.projQueue[(int)type].Enqueue(Object);
    }
}
