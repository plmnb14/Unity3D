using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float Damage = 0.0f;
    private float LifeTime = 10.0f;
    private float speed = 2.0f;

    private int targetLayer = 0;
    private Vector3 direction;

    public void Initialize(Vector3 position, Vector3 Direction, int Layer, float Damage)
    {
        transform.position = position;
        transform.forward = Direction;
        direction = Direction;
        targetLayer = Layer;
    }

    void Flying()
    {
        transform.position += direction * speed * Time.deltaTime;
        //transform.Translate(direction * Time.deltaTime);
    }

    void OnDead()
    {
        ResetStatus();
        ObjectManager.ReturnObject(this);
    }

    IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(LifeTime);
        OnDead();
    }

    void ResetStatus()
    {
        Damage = 0.0f;
        transform.position = Vector3.zero;
        transform.forward = Vector3.forward;
        direction = Vector3.zero;
        targetLayer = 0;
    }

    void Start()
    {
    }

    void Update()
    {
        Flying();
        LifeCycle();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != targetLayer)
            return;

        Creature CollisionTarget = other.GetComponent<Creature>();

        if(null != CollisionTarget && !CollisionTarget.Dead)
        {
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            Vector3 hitNormal = transform.position - other.transform.position;

            CollisionTarget.OnDamage(hitPoint, hitNormal, Damage);

            OnDead();
        }
    }
}
