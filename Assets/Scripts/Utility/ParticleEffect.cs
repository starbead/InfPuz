using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem effect = null;

    public void Play()
    {
        effect.Play(true);
    }
}
