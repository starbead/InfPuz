using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoardMaker : MonoBehaviour
{
    const int MAXBlock = 5;

    [Space(5)]
    [SerializeField] Transform tileTF = null;
    [SerializeField] Transform blockTF = null;

    [Space(5)]
    [SerializeField] GameObject originTile = null;
    [SerializeField] Blocks originBlock = null;

    int[,] board = null;
    List<List<Blocks>> blockList = null;

    int[] nextBoard = null;
    List<Blocks> nextBlockList = null;

    ModeBase mode;

    public List<List<Vector2>> posList { get; private set; }

    int row = 5;
    int col = 9;
    int curScore = 0;
    int breakCount = 0;
    public void initData()
    {
        curScore = 0;
        board = new int[col, row];
        blockList = new List<List<Blocks>>();

        nextBoard = new int[row];
        nextBlockList = new List<Blocks>();

        mode = new ModeHard(board, blockList);

        initSetting();
        mode.initNextBlock(nextBoard, nextBlockList, originBlock);
        mode.TryGetBlock(MAXBlock);
        Rendering_Block();

        GameEventSubject.SendGameEvent(GameEventType.ChangeScore, curScore);
    }

    void initSetting()
    {
        posList = new List<List<Vector2>>();
        float xPos = -1.3f;
        float yPos = -3.2f;

        for(int i = 0; i < col; i++)
        {
            posList.Add(new List<Vector2>());
            blockList.Add(new List<Blocks>());
        }

        for (int i = col - 1; i >= 0 ; i--)
        {
            xPos = -1.3f;
            for (int j = 0; j < row; j++)
            {
                var pos = new Vector2(xPos, yPos);
                posList[i].Add(pos);

                var obj = Instantiate(originTile, tileTF);
                obj.transform.localPosition = pos;

                var obj2 = Instantiate(originBlock, blockTF);
                obj2.transform.localPosition = pos;
                obj2.SetBlock(i, j, 0);
                blockList[i].Add(obj2);

                xPos += 0.6f;
            }
            yPos += 0.6f;
        }
    }
    public void ReSetBlock()
    {
        curScore = 0;
        GameEventSubject.SendGameEvent(GameEventType.ChangeScore, curScore);

        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
                board[i, j] = 0;
        }

        foreach(var blocks in blockList)
        {
            foreach(var block in blocks)
                block.HideBlock();
        }

        for(int i = 0; i < row; i++)
        {
            nextBoard[i] = 0;
            nextBlockList[i].HideBlock();
        }

        mode.TryGetBlock(MAXBlock);
        Rendering_Block();
    }
    void EndGame()
    {
        GameEventSubject.SendGameEvent(GameEventType.ChangeScore, curScore);
        GameEventSubject.SendGameEvent(GameEventType.GameEnd, curScore);
    }
    public void Rendering_Block()
    {
        for(int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
                blockList[i][j].SetBlock(i, j, board[i, j]);
        }

        for (int i = 0; i < nextBlockList.Count; i++)
            nextBlockList[i].SetDummy(nextBoard[i]);

        GameEventSubject.SendGameEvent(GameEventType.ChangeScore, curScore);
    }
    public int BreakBlock(int index1, int index2)
    {
        breakCount = 1;
        var value = board[index1, index2];
        board[index1, index2] = 0;
        blockList[index1][index2].Explode();
        curScore += 1;
        BreakBlock_Recursive(index1, index2, value);
        CheckCombo();
        StartCoroutine((mode as PuzzleMode).PlayEffect_Cor(MAXBlock, Rendering_Block, EndGame));
        return breakCount;
    }
    public void SetDummyBlock(bool onoff)
    {
        foreach (var dummy in nextBlockList)
            dummy.SetEnableIce(onoff);
    }
    void CheckCombo()
    {
        if (breakCount < 3)
            SetDummyBlock(false);
        else
        {
            int sum = 0;
            for(int i = 0; i < col; i++)
            {
                for(int j = 0; j < row; j++)
                {
                    sum += board[i, j];
                }
            }
            if (sum == 0)
            {
                SetDummyBlock(false);
                InGameManager.Instance.ReSetCombo();
            }
        }
    }
    void BreakBlock_Recursive(int i, int j, int value)
    {
        if (value == 0) return;
        // 좌측
        if (j > 0 && board[i, j - 1] == value)
        {
            board[i, j - 1] = 0;
            blockList[i][j - 1].Explode();
            curScore += 1;
            breakCount += 1;
            BreakBlock_Recursive(i, j - 1, value);
        }

        // 우측
        if (j < row - 1 && board[i, j + 1] == value)
        {
            board[i, j + 1] = 0;
            blockList[i][j + 1].Explode();
            curScore += 1;
            breakCount += 1;
            BreakBlock_Recursive(i, j + 1, value);
        }

        // 상단
        if (i > 0 && board[i - 1, j] == value)
        {
            board[i - 1, j] = 0;
            blockList[i - 1][j].Explode();
            curScore += 1;
            breakCount += 1;
            BreakBlock_Recursive(i - 1, j, value);
        }

        // 하단
        if (i < col - 1 && board[i + 1, j] == value)
        {
            board[i + 1, j] = 0;
            blockList[i + 1][j].Explode();
            curScore += 1;
            breakCount += 1;
            BreakBlock_Recursive(i + 1, j, value);
        }
    }
}
