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

    int xIndex = -1;
    int yIndex = -1;
    float speed = 0.3f;
    public void SetBlock(int x, int y, int blockNum)
    {
        if (InGameManager.instance != null)
            this.transform.localPosition = InGameManager.instance.GetPos(x, y);
        xIndex = x;
        yIndex = y;
        icon.sprite = iconList[blockNum];
    }
    public void SetDummy(int blockNum)
    {
        _animator.enabled = false;
        xIndex = -1;
        yIndex = -1;
        icon.sprite = iconList[blockNum];

        Color c = icon.color;
        c.a = 0.7f;
        icon.color = c;
    }
    public void Explode()
    {
        // Æø¹ß ¿¬Ãâ
        HideBlock();
    }
    public void HideBlock()
    {
        icon.sprite = iconList[0];
    }
    public void FallenAni(int x, int y)
    {
        xIndex = x;
        yIndex = y;

        var tween = LeanTween.move(this.gameObject, InGameManager.instance.GetPos(x, y), speed);
        tween.setEase(LeanTweenType.easeInQuad);
        tween.setOnComplete(() =>
        {
            _animator.SetTrigger("Falling");
        });
    }
    public (int, int) GetIndex => (xIndex, yIndex);
    public float MoveSpeed => speed;
}
