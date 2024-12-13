using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks : MonoBehaviour
{
    [Header("UI"), Space(5)]
    [SerializeField] SpriteRenderer icon = null;
    [SerializeField] Sprite[] iconList = null;

    [Space(5)]
    [SerializeField] ParticleEffect[] effects = null;
    [SerializeField] Animator _animator = null;

    int xIndex = -1;
    int yIndex = -1;
    float speed = 0.3f;
    int curIdx = -1;
    public void SetBlock(int x, int y, int blockNum)
    {
        this.gameObject.SetActive(true);
        if (InGameManager.instance != null)
            this.transform.localPosition = InGameManager.instance.GetPos(x, y);
        xIndex = x;
        yIndex = y;
        curIdx = blockNum;
        icon.sprite = iconList[blockNum];
    }
    public void SetDummy(int blockNum)
    {
        this.gameObject.SetActive(true);
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
        if (curIdx == 0) return;
        // Æø¹ß ¿¬Ãâ
        var obj = Instantiate(effects[curIdx]);
        obj.transform.position = InGameManager.instance.GetPos(xIndex, yIndex);
        HideBlock();
    }
    public void HideBlock()
    {
        curIdx = 0;
        icon.sprite = iconList[0];
        this.gameObject.SetActive(false);
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
