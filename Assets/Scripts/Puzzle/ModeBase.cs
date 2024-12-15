using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeBase : MonoBehaviour
{
    protected int[,] board = null;    // [0,?] => 가장 위층
    protected List<List<Blocks>> blockList = null;
    protected System.Random rand = new System.Random();

    protected List<Blocks> nextBlockList = null;
    protected int[] nextBoard = null;

    protected WaitForSeconds waitGravity = null;
    public ModeBase(int[,] board, List<List<Blocks>> blockList)
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
            for (int i = 0; i < board.GetLength(1); i++)
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
    public virtual void initNextBlock(int[] nextBoard, List<Blocks> nextBlockList, Blocks origin)
    {
        if(waitGravity == null)
            waitGravity = new WaitForSeconds(blockList[0][0].MoveSpeed);

        float xPos = -1.3f;
        for (int i = 0; i < nextBoard.Length; i++)
        {
            var obj = Instantiate(origin);
            obj.transform.position = new Vector3(xPos, -2.75f, 0f); // -2.69f is original
            obj.initDummy();
            nextBlockList.Add(obj);
            xPos += 0.6f;
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
    public bool ApplyGravity()
    {
        bool needWait = false;
        int col = board.GetLength(0);
        int row = board.GetLength(1);

        Queue<(int, int, int)> queue = new Queue<(int, int, int)>();

        for(int i = 0; i < row; i++)
        {
            for(int j = col - 1; j >= 0; j--)
            {
                if (board[j, i] != 0)
                    queue.Enqueue((j, i, board[j, i]));
            }

            for (int j = col - 1; j >= 0; j--)
            {
                if (queue.Count > 0)
                {
                    var tuple = queue.Dequeue();
                    board[j, i] = tuple.Item3;
                    if (j == tuple.Item1 && i == tuple.Item2)
                        continue;
                    blockList[j][i].SetBlock(tuple.Item1, tuple.Item2, 0);
                    blockList[tuple.Item1][tuple.Item2].FallenAni(j, i);

                    var temp = blockList[j][i];
                    blockList[j][i] = blockList[tuple.Item1][tuple.Item2];
                    blockList[tuple.Item1][tuple.Item2] = temp;
                    needWait = true;
                }
                else
                    board[j, i] = 0;
            }
        }

        return needWait;
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
