using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PuzzleNormal : PuzzleBase, PuzzleMode
{
    public PuzzleNormal(int[,] board, List<List<Block>> blockList) : base(board, blockList) { }
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
        if (TryGetBlock(blockCount) == false)
        {
            action2?.Invoke();
            yield break;
        }
        action1?.Invoke();
    }
}
