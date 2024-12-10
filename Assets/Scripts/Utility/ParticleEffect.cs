using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem effect = null;
    private void Start()
    {
        Play();
    }
    public void Play()
    {
        effect.Play(true);
    }

    private void LateUpdate()
    {
        if (effect.IsAlive(true)) return;

        Destroy(gameObject);
    }
}
