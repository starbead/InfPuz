using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices.WindowsRuntime;

public class ModeHard : ModeBase, PuzzleMode
{
    public ModeHard(int[,] board, List<List<Blocks>> blockList) : base(board, blockList) { }

    public override bool TryGetBlock(int blockCount)
    {
        var fail = CheckFail();
        var comboing = InGameManager.instance.isCombo;

        if (fail && comboing == false) return false;
        else
        {
            if (comboing) return true;
        }

        RandBlock(blockCount);

        return true;
    }
    public override void RandBlock(int blockCount)
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
                board[0, i] = nextBoard[i];
            }
        }

        for (int i = 0; i < board.GetLength(1); i++)
        {
            var num = rand.Next(1, blockCount);
            nextBoard[i] = num;
        }
    }

    public override void initNextBlock(int[] nextBoard, List<Blocks> nextBlockList, Blocks origin)
    {
        if (waitGravity == null)
            waitGravity = new WaitForSeconds(blockList[0][0].MoveSpeed);

        float xPos = -1.3f;
        for (int i = 0; i < nextBoard.Length; i++)
        {
            var obj = Instantiate(origin);
            obj.transform.position = new Vector3(xPos, 2.5f, 0f);   // 2.8f is original
            obj.initDummy();
            nextBlockList.Add(obj);
            xPos += 0.6f;
        }

        this.nextBlockList = nextBlockList;
        this.nextBoard = nextBoard;
    }

    public IEnumerator PlayEffect_Cor(int blockCount, Action action1 = null, Action action2 = null)
    {
        yield return null;
        // 떨어지기
        if(ApplyGravity())
            yield return waitGravity;

        // 추가하기
        var isEnd = TryGetBlock(blockCount);
        if(isEnd == false)
        {
            action2?.Invoke();
            yield break;
        }

        action1?.Invoke();
        // 떨어지기
        if(ApplyGravity())
            yield return waitGravity;
        action1?.Invoke();

        InGameManager.instance.SetClickStatus(true);
    }
}
