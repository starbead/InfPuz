using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem effect = null;
    private void OnEnable()
    {
        Play();
    }
    public void Play()
    {
        isDestroy = false;
        effect.Play(true);
    }
    private bool isDestroy = false;
    public void DestroyObject()
    {
        if (isDestroy) return;

        isDestroy = true;
        if (Resource.Release(gameObject))
            return;

        Destroy(gameObject);
    }
    private void LateUpdate()
    {
        if (effect.IsAlive(true)) return;

        DestroyObject();
    }
}
