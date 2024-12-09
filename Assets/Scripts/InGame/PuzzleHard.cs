using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PuzzleHard : PuzzleBase, PuzzleMode
{
    public PuzzleHard(int[,] board, List<List<Block>> blockList) : base(board, blockList) { }

    public override bool TryGetBlock(int blockCount)
    {
        if (CheckFail()) return false;

        RandBlock(blockCount);

        return true;
    }
    public override void RandBlock(int blockCount)
    {
        for (int i = 0; i < board.GetLength(1); i++)
        {
            var num = rand.Next(1, blockCount);
            board[0, i] = num;
        }
    }
    public IEnumerator PlayEffect_Cor(int blockCount, Action action1 = null, Action action2 = null)
    {
        yield return null;
        yield return new WaitUntil(() =>
        {
            foreach (var blocks in blockList)
            {
                foreach (var block in blocks)
                {
                    if (block.isMove) return false;
                }
            }

            return true;
        });

        UpdateBlock();
        bool isEnd = TryGetBlock(blockCount);
        if (isEnd == false)
        {
            action2?.Invoke();
            yield break;
        }

        action1?.Invoke();
        yield return null;
        yield return new WaitUntil(() =>
        {
            foreach (var blocks in blockList)
            {
                foreach (var block in blocks)
                {
                    if (block.isMove) return false;
                }
            }

            return true;
        });

        UpdateBlock();
        action1?.Invoke();
    }
}
