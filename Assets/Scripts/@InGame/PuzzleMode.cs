using System;
using System.Collections;

public interface PuzzleMode
{
    // 구현 필요
    public IEnumerator PlayEffect_Cor(int blockCount, Action action1 = null, Action action2 = null);
}