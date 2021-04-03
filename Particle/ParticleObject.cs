using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleObject : MonoBehaviour
{
    string particleName;
    float lifetime;
    ParticleSystem particle;

    public void OnPlay(string name)
    {
        particle = gameObject.GetComponent<ParticleSystem>();
        lifetime = particle.time;
        particleName = name;

        gameObject.GetComponent<ParticleSystem>().Play();
        StartCoroutine(OnPlaying());
    }

    private IEnumerator OnPlaying()
    {
        while(!particle.isStopped)
        {
            yield return new WaitForEndOfFrame();
        }

        OnDead();
    }

    private void OnDead()
    {
        ParticleManager.instance.ReturnParticle(this.gameObject, particleName);
    }
}
