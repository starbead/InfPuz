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
    int col = 7;
    public void initData()
    {
        board = new int[col, row];
        blockList = new List<List<Blocks>>();

        nextBoard = new int[row];
        nextBlockList = new List<Blocks>();

        mode = new ModeHard(board, blockList);

        initSetting();
        mode.initNextBlock(nextBoard, nextBlockList, originBlock);
        mode.TryGetBlock(MAXBlock);
        Rendering_Block();
    }

    void initSetting()
    {
        posList = new List<List<Vector2>>();
        float xPos = -1.3f;
        float yPos = -2f;

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
        for(int i = 0; i < col; i++)
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
    }
    public void BreakBlock(int index1, int index2)
    {
        var value = board[index1, index2];
        board[index1, index2] = 0;
        blockList[index1][index2].Explode();
        BreakBlock_Recursive(index1, index2, value);
        StartCoroutine((mode as PuzzleMode).PlayEffect_Cor(MAXBlock, Rendering_Block, ReSetBlock));
    }
    void BreakBlock_Recursive(int i, int j, int value)
    {
        if (value == 0) return;
        // 좌측
        if (j > 0 && board[i, j - 1] == value)
        {
            board[i, j - 1] = 0;
            blockList[i][j - 1].Explode();
            BreakBlock_Recursive(i, j - 1, value);
        }

        // 우측
        if (j < row - 1 && board[i, j + 1] == value)
        {
            board[i, j + 1] = 0;
            blockList[i][j + 1].Explode();
            BreakBlock_Recursive(i, j + 1, value);
        }

        // 상단
        if (i > 0 && board[i - 1, j] == value)
        {
            board[i - 1, j] = 0;
            blockList[i - 1][j].Explode();
            BreakBlock_Recursive(i - 1, j, value);
        }

        // 하단
        if (i < col - 1 && board[i + 1, j] == value)
        {
            board[i + 1, j] = 0;
            blockList[i + 1][j].Explode();
            BreakBlock_Recursive(i + 1, j, value);
        }
    }
}
