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
    bool isEnd = true;
    (int, int) BlockIndex = (0, 0);

    int curIdx = 0;
    float speed = 3.5f;
    private void Start()
    {
        mask = LayerMask.GetMask("Block");
        transform.localScale = blockSize;
    }
    public void initData(int x, int y)
    {
        BlockIndex.Item1 = x;
        BlockIndex.Item2 = y;
        originPos = this.transform.position;
    }
    public void SetBlock(int block)
    {
        curIdx = block;
        this.transform.position = originPos;
        spriteIcon.sprite = blocks[curIdx];
        col.SetActive(curIdx != 0);
    }
    public void PlayEffect()
    {
        if (curIdx == 0) return;
        var obj = Instantiate(effects[curIdx]);
        obj.transform.position = originPos;
        SetBlock(0);
    }
    private void Update()
    {
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

        transform.position = transform.position + (Vector3.down * 0.685f * Time.deltaTime * Random.Range(7f, 8f));
    }

    public int GetLength => blocks.Length;
    public (int, int) GetIndex => BlockIndex;
    public bool isMove => !isEnd;
}