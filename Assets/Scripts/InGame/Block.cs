using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteIcon = null;
    [SerializeField] Sprite[] blocks = null;
    [SerializeField] Collider2D col = null;
    [SerializeField] ParticleEffect[] effects = null;

    RaycastHit2D ray;
    LayerMask mask;
    Vector2 blockSize = new Vector2(0.7f, 0.7f);
    Vector2 originPos = new Vector2(0, 0);
    Vector2 effectPos = new Vector2(0, 0);
    bool isEnd = true;
    bool isDummy = false;
    (int, int) BlockIndex = (0, 0);

    int curIdx = 0;
    private void Start()
    {
        mask = LayerMask.GetMask("Block");
        transform.localScale = blockSize;
    }
    public void initData(int x, int y)
    {
        isDummy = false;
        BlockIndex.Item1 = x;
        BlockIndex.Item2 = y;
        originPos = this.transform.localPosition;
        effectPos = this.transform.position;
    }
    public void SetDummy(int block)
    {
        isDummy = true;
        spriteIcon.sprite = blocks[block];
        Color c = spriteIcon.color;
        c.a = 0.5f;
        spriteIcon.color = c;
        transform.tag = "Untagged";
    }
    public void SetBlock(int block)
    {
        curIdx = block;
        spriteIcon.sprite = blocks[curIdx];
        col.SetActive(curIdx != 0);
        this.transform.localPosition = originPos;
    }
    public void PlayEffect()
    {
        if (curIdx == 0) return;
        var obj = Instantiate(effects[curIdx]);
        obj.transform.position = effectPos;
        SetBlock(0);
    }
    private void Update()
    {
        if (isDummy) return;

        var pos = this.transform.position;
        var hit = Physics2D.LinecastAll(pos + (Vector3.down * 0.35f), pos + (Vector3.up * 0.35f));
        if (hit != null && hit.Length > 1)
        {
            foreach (var col in hit)
            {
                if (col.collider == null && col.collider.transform.position.x != this.transform.position.x)
                    continue;
                if (col.collider.transform.position.y == this.transform.position.y)
                    continue;
                isEnd = col.collider.transform.position.y < this.transform.position.y ? true : false;
                if (isEnd) return;
            }
        }
        else
            isEnd = false;

        if (isEnd) return;

        transform.position = transform.position + (Vector3.down * 0.685f * Time.deltaTime * 7.5f);
    }
    public int GetLength => blocks.Length;
    public (int, int) GetIndex => BlockIndex;
    public bool isMove => !isEnd;
}