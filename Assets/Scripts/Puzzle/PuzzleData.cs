using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PuzzleData
{
    [JsonProperty]
    public int[,] board { get; set; }
    [JsonProperty]
    public int[] nextBoard { get; set; }
    [JsonProperty]
    public int curScore { get; set; }
    [JsonProperty]
    public int comboCount { get; set; }

    public int row { get; private set; } = 5;
    public int col { get; private set; } = 9;
    public PuzzleData()
    {
        board = new int[col, row];
        nextBoard = new int[row];
        curScore = 0;
        comboCount = 0;
    }
    public void ResetData()
    {
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
                board[i, j] = 0;
        }
        for (int i = 0; i < row; i++)
        {
            nextBoard[i] = 0;
        }
        curScore = 0;
        comboCount = 0;
    }
    public void AddSocre(int score) => curScore += score;
    public void ReSetCombo() => comboCount = 0;
    public bool isCombo => comboCount >= 3;
}
