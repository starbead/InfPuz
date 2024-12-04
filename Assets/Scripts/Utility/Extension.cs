using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    public static void SetActive(this Component to, bool isAlive)
    {
        to.gameObject.SetActive(isAlive);
    }
}
