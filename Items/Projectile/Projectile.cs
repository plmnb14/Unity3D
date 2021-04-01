using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected enum ParticleType { Head, Tail, TypeEnd };
    protected ParticleSystem[] Particles = new ParticleSystem[(int)ParticleType.TypeEnd];

    protected float projectileDamage;
    protected float lifeTime;
    protected float speed;
    protected int targetLayer;
    protected Vector3 direction;

    public Creature Owner { get; set; }

    public void Initialize(Vector3 position, Vector3 Direction, int Layer, float Damage, float ProjectileSpeed = 5.0f)
    {
        lifeTime = 5.0f;
        transform.position = position;
        transform.forward = Direction;
        direction = Direction;
        targetLayer = Layer;
        projectileDamage = Damage;
        speed = ProjectileSpeed;

        foreach(var particle in Particles)
        {
            if(particle != null)
            {
                particle.Play();
            }
        }
    }

    protected void AwakeSetUp()
    {
        Particles = new ParticleSystem[(int)ParticleType.TypeEnd];
    }

    protected void Flying()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    protected IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(lifeTime);

        OnDead();
    }

    protected virtual void OnDead()
    {
    }

    protected void ResetStatus()
    {
        Owner = null;
        transform.position = Vector3.zero;
        transform.forward = Vector3.forward;
        direction = Vector3.forward;
        targetLayer = 0;
        projectileDamage = 0.0f;
        speed = 0.0f;

        //this.gameObject.SetActive(false);
    }
}
