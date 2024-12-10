using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    [Header("SIZE"), Space(5)]
    [SerializeField] int row = 5;
    [SerializeField] int col = 7;

    [Header("Block"), Space(10)]
    [SerializeField] Block originBlock = null;
    [SerializeField] Transform[] floors = null;
    
    public static PuzzleController instance = null;

    int[,] board = null;    // [0,?] => 가장 위층
    List<List<Block>> blockList = null;

    int[] nextBoard = null;
    List<Block> nextBlockList = null;

    PuzzleBase mode;

    bool canClick = true;
    int curScore = 0;
    private void Awake()
    {
        initailize();
    }
    public void initailize()
    {
        if (instance == null)
            instance = this;

        curScore = 0;
        board = new int[col, row];
        blockList = new List<List<Block>>();
        mode = new PuzzleHard(board, blockList);

        nextBoard = new int[row];
        nextBlockList = new List<Block>();

        // 블럭, 프레임 세팅
        initFrame();
        mode.initNextBlock(nextBoard, nextBlockList, originBlock);
        mode.TryGetBlock(originBlock.GetLength);
        mode.UpdateBlock();
        RenderBlock();
    }
    public void ReSetStage()
    {
        GameEventSubject.SendGameEvent(GameEventType.ChangeScore, curScore);
        curScore = 0;
        board = new int[col, row];
        mode.TryGetBlock(originBlock.GetLength);
        mode.UpdateBlock();
        RenderBlock();
    }
    public void initFrame()
    {
        // 프레임은 일단 있다고 생각하자

        // 블록을 모두 배치
        float xPos = -1.5f;
        for (int i = 0; i < col; i++)
        {
            xPos = -1.5f;
            blockList.Add(new List<Block>());
            for(int j = 0; j < row; j++)
            {
                var obj = Instantiate(originBlock, floors[i]);
                obj.transform.localPosition = new Vector3(xPos, 0, 0);
                obj.initData(i, j);
                blockList[i].Add(obj);
                xPos += 0.685f;
            }
        }
    }
    public void RenderBlock()
    {
        for(int i = 0; i < col; i++)
        {
            for(int j = 0; j < row; j++)
            {
                blockList[i][j].SetBlock(board[i, j]);
            }
        }

        for(int i = 0; i < nextBlockList.Count; i++)
        {
            nextBlockList[i].SetDummy(nextBoard[i]);
        }

        GameEventSubject.SendGameEvent(GameEventType.ChangeScore, curScore);
        canClick = true;
    }
    void OnClick_Block(int index1, int index2)
    {
        canClick = false;
        var value = board[index1, index2];
        board[index1, index2] = 0;
        blockList[index1][index2].PlayEffect();
        curScore += 1;
        ClearBlock_Recursive(index1, index2, value);
        StartCoroutine((mode as PuzzleMode).PlayEffect_Cor(originBlock.GetLength, RenderBlock, ReSetStage));
    }
    void ClearBlock_Recursive(int i, int j, int value)
    {
        if (value == 0) return;
        // 좌측
        if(j > 0 && board[i, j - 1] == value)
        {
            board[i, j - 1] = 0;
            blockList[i][j - 1].PlayEffect();
            curScore += 1;
            ClearBlock_Recursive(i, j - 1, value);
        }

        // 우측
        if(j < row - 1 && board[i, j + 1] == value)
        {
            board[i, j + 1] = 0;
            blockList[i][j + 1].PlayEffect();
            curScore += 1;
            ClearBlock_Recursive(i, j + 1, value);
        }

        // 상단
        if(i > 0 && board[i - 1, j] == value)
        {
            board[i - 1, j] = 0;
            blockList[i - 1][j].PlayEffect();
            curScore += 1;
            ClearBlock_Recursive(i - 1, j, value);
        }

        // 하단
        if(i < col - 1 && board[i + 1, j] == value)
        {
            board[i + 1, j] = 0;
            blockList[i + 1][j].PlayEffect();
            curScore += 1;
            ClearBlock_Recursive(i + 1, j, value);
        }
    }
    IEnumerator PlayEffect_Cor()
    {
        yield return null;
        yield return new WaitUntil(() =>
        {
            foreach(var blocks in blockList)
            {
                foreach (var block in blocks)
                {
                    if (block.isMove) return false;
                }
            }

            return true;
        });

        mode.UpdateBlock();
        if (mode.TryGetBlock(originBlock.GetLength) == false)
        {
            ReSetStage();
            yield break;
        }
        RenderBlock();
    }
    private void Update()
    {
        if (canClick == false) return;

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

            if(hit.collider != null && hit.transform.gameObject.tag == "Block")
            {
                if (hit.transform.TryGetComponent<Block>(out var obj))
                    OnClick_Block(obj.GetIndex.Item1, obj.GetIndex.Item2);
            }
        }
    }

    // 테스트용 치트코드
    public void Cheat()
    {
        mode.UpdateBlock();
        mode.TryGetBlock(originBlock.GetLength);
        RenderBlock();
    }
}
