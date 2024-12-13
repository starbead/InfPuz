using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ModeNormal : ModeBase, PuzzleMode
{
    public ModeNormal(int[,] board, List<List<Blocks>> blockList) : base(board, blockList) { }

    public IEnumerator PlayEffect_Cor(int blockCount, Action action1 = null, Action action2 = null)
    {
        yield return null;
        // ¶³¾îÁö±â
        ApplyGravity();

        
        TryGetBlock(blockCount);
        action1?.Invoke();
    }
}
