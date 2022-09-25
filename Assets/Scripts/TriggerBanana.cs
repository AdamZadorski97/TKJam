using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBanana : MonoBehaviour
{
    public ParticleSystem particleSystemBanana;

    public void SpawnParticles()
    {
        particleSystemBanana.Play();
    }
}
