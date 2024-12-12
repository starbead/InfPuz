using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoardMaker : MonoBehaviour
{
    [Space(5)]
    [SerializeField] Transform tileTF = null;
    [SerializeField] Transform blockTF = null;

    [Space(5)]
    [SerializeField] GameObject originTile = null;
    [SerializeField] Blocks originBlock = null;

    int[,] board = null;
    List<Blocks> blockList = null;

    int[] nextBoard = null;
    List<Blocks> nextBlockList = null;

    public List<List<Vector2>> posList { get; private set; }

    int row = 5;
    int col = 7;
    public void initData()
    {
        board = new int[col, row];
        blockList = new List<Blocks>();

        nextBoard = new int[row];
        nextBlockList = new List<Blocks>();

        SetBackGround();
        initBlock();
    }

    void SetBackGround()
    {
        posList = new List<List<Vector2>>();
        float xPos = -1.3f;
        float yPos = -2f;

        for(int i = 0; i < col; i++)
        {
            xPos = -1.3f;
            posList.Add(new List<Vector2>());
            for (int j = 0; j < row; j++)
            {
                var pos = new Vector2(xPos, yPos);
                posList[i].Add(pos);
                var obj = Instantiate(originTile, tileTF);
                obj.transform.localPosition = pos;
                xPos += 0.6f;
            }
            yPos += 0.6f;
        }
    }
    void initBlock()
    {
        float xPos = -1.3f;
        float yPos = -2f;
        for (int i = 0; i < row; i++)
        {
            var obj = Instantiate(originBlock, blockTF);
            obj.transform.localPosition = new Vector3(xPos, yPos, 0);
            xPos += 0.6f;
        }
    }
    public void ReSetBlock()
    {
        for(int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
                board[i, j] = 0;
        }

        foreach(var block in blockList)
        {
            block.HideBlock();
        }

        for(int i = 0; i < row; i++)
        {
            nextBoard[i] = 0;
            nextBlockList[i].HideBlock();
        }
    }
}
