using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks : MonoBehaviour
{
    [Header("UI"), Space(5)]
    [SerializeField] SpriteRenderer icon = null;
    [SerializeField] Sprite[] iconList = null;

    [Space(5)]
    [SerializeField] Animator _animator = null;

    bool _isUse = false;
    int xIndex = 0;
    int yIndex = 0;

    public void SetBlock(int x, int y, int blockNum)
    {
        gameObject.SetActive(true);
        if (InGameManager.instance != null)
            this.gameObject.transform.localPosition = InGameManager.instance.GetPos(x, y);

        _isUse = true;
        xIndex = x;
        yIndex = y;
        icon.sprite = iconList[blockNum];
    }
    public void Explode()
    {
        HideBlock();
    }
    public void HideBlock()
    {
        _isUse = false;
        icon.sprite = iconList[0];
        gameObject.SetActive(false);
    }

    public bool IsUse => _isUse;
}
