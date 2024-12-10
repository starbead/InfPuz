using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class PuzzleBase : MonoBehaviour
{
    protected int[,] board = null;    // [0,?] => 가장 위층
    protected List<List<Block>> blockList = null;
    protected System.Random rand = new System.Random();

    protected List<Block> nextBlockList = null;
    protected int[] nextBoard = null;

    public PuzzleBase(int[,] board, List<List<Block>> blockList)
    {
        this.board = board;
        this.blockList = blockList;
    }
    public virtual void RandBlock(int blockCount)
    {
        if (nextBoard[0] == 0)
        {
            for (int i = 0; i < board.GetLength(1); i++)
            {
                var num = rand.Next(1, blockCount);
                board[board.GetLength(0) - 1, i] = num;
            }
        }
        else
        {
            for(int i = 0; i < board.GetLength(1); i++)
            {
                board[board.GetLength(0) - 1, i] = nextBoard[i];
            }
        }
        
        for (int i = 0; i < board.GetLength(1); i++)
        {
            var num = rand.Next(1, blockCount);
            nextBoard[i] = num;
        }
    }
    public virtual void initNextBlock(int[] nextBoard, List<Block> nextBlockList, Block origin)
    {
        float xPos = -1.4f;
        for(int i = 0; i < nextBoard.Length; i++)
        {
            var obj = Instantiate(origin);
            obj.transform.position = new Vector3(xPos, -2.75f, 0f); // -2.69f is original
            obj.SetDummy(0);
            nextBlockList.Add(obj);
            xPos += 0.685f;
        }

        this.nextBlockList = nextBlockList;
        this.nextBoard = nextBoard;
    }
    // 블록을 부순뒤에 위치 정리
    public virtual void UpdateBlock()
    {
        int col = board.GetLength(0);
        int row = board.GetLength(1);

        Queue<int> queue = new Queue<int>();
        for (int i = 0; i < row; i++)
        {
            for (int j = col - 1; j >= 0; j--)
            {
                if (board[j, i] != 0)
                    queue.Enqueue(board[j, i]);
            }

            for (int j = col - 1; j >= 0; j--)
            {
                if (queue.Count > 0)
                    board[j, i] = queue.Dequeue();
                else
                    board[j, i] = 0;
            }
        }
    }

    public virtual bool TryGetBlock(int blockCount)
    {
        if (CheckFail()) return false;

        int col = board.GetLength(0);
        int row = board.GetLength(1);

        bool needUp = false;
        for (int i = 0; i < row; i++)
        {
            if (board[col - 1, i] == 0)
                continue;
            needUp = true;
            break;
        }

        if (needUp)
        {
            for (int i = 1; i < col; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    if (board[i, j] == 0)
                        continue;
                    board[i - 1, j] = board[i, j];
                }
            }

            for (int i = 0; i < row; i++)
                board[col - 1, i] = 0;

            RandBlock(blockCount);
        }
        else
        {
            RandBlock(blockCount);
        }

        return true;
    }

    protected bool CheckFail()
    {
        bool result = false;

        for (int i = 0; i < board.GetLength(1); i++)
        {
            if (board[0, i] == 0)
                continue;

            result = true;
            break;
        }

        return result;
    }
}
