using App.Enum;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance = null;

    const string fileName = "SaveData.json";
    [SerializeField] BoardMaker board = null;

    PuzzleData puzzleData = null;
    int comboCount = 0;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        bool isLoadData = false;
        if (PlayerPrefs.GetInt(Common.GetPlayerPrefs(App.Enum.LocalData.LOADSAVE), 0) == 0)
        {
            puzzleData = new PuzzleData();
        }
        else
        {
            LoadData();
            isLoadData = true;
        }

        board.initData(puzzleData, isLoadData);
        SetClickStatus(true);
    }

    public Vector2 GetPos(int x, int y)
    {
        return board.posList[x][y];
    }

    void OnClick_Block(int index1, int index2)
    {
        SetClickStatus(false);
        
        var count = board.BreakBlock(index1, index2);
        
        puzzleData.comboCount = count >= 3 ? puzzleData.comboCount + 1 : 0;
        GameEventSubject.SendGameEvent(GameEventType.ChangeCombo, puzzleData.comboCount);
        if (puzzleData.isCombo)
        {
            board.SetDummyBlock(true);
            GameEventSubject.SendGameEvent(GameEventType.EffectCombo);
        }
    }
    public void ReSetStage()
    {
        board.ReSetBlock();
        SetClickStatus(true);
    }
    public void SaveData()
    {
        PlayerPrefs.SetInt(Common.GetPlayerPrefs(App.Enum.LocalData.LOADSAVE), 1);
        string filePath = Application.persistentDataPath + "/" + fileName;
        string jsonStr = NJson.Encode(puzzleData);
        System.IO.File.WriteAllText(filePath, jsonStr);
    }
    public void LoadData()
    {
        string filePath = Application.persistentDataPath + "/" + fileName;

        if (System.IO.File.Exists(filePath) == false) return;

        string fileStr = System.IO.File.ReadAllText(filePath);
        var data = NJson.Decode<PuzzleData>(fileStr);
        if (data != null)
            puzzleData = data;
    }
    public void DeleteData()
    {
        PlayerPrefs.DeleteKey(Common.GetPlayerPrefs(App.Enum.LocalData.LOADSAVE));
    }
    Stack<int> ClickStack = new Stack<int>();
    bool canClick = false;
    public void SetClickStatus(bool onoff)
    {
        if (onoff)
        {
            if (ClickStack.Count > 0)
                ClickStack.Pop();
        }
        else
        {
            ClickStack.Push(0);
        }

        canClick = ClickStack.Count == 0;
    }
    private void Update()
    {
        if (canClick == false) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

            if (hit.collider != null && hit.transform.gameObject.tag == "Block")
            {
                if (hit.transform.TryGetComponent<Blocks>(out var obj) && obj.isBlock)
                    OnClick_Block(obj.GetIndex.Item1, obj.GetIndex.Item2);
            }
        }
    }

    public bool isCombo => puzzleData.comboCount >= 3;
    public int LoadScore => puzzleData.curScore;
    public int LoadCombo => puzzleData.comboCount;
}
