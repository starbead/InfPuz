using App.Enum;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class InGameManager : MonoSingleton<InGameManager>
{
    const string fileName = "SaveData.json";
    [SerializeField] BoardMaker board = null;

    PuzzleData puzzleData = null;
    int comboCount = 0;
    protected override void ChildAwake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        if (PlayerPrefs.GetInt(Common.GetPlayerPrefs(App.Enum.LocalData.LOADSAVE), 0) == 0)
            puzzleData = new PuzzleData();
        else
            LoadData();

        board.initData(puzzleData);
        SetClickStatus(true);
    }
    protected override void ChildOnDestroy()
    {
        
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
        //string jsonStr = JsonUtility.ToJson(puzzleData);
        string jsonStr = NJson.Encode(puzzleData);
        System.IO.File.WriteAllText(filePath, jsonStr);
    }
    public void LoadData()
    {
        string filePath = Application.persistentDataPath + "/" + fileName;

        if (System.IO.File.Exists(filePath) == false) return;

        string fileStr = System.IO.File.ReadAllText(filePath);
        //var data = JsonUtility.FromJson<PuzzleData>(fileStr);
        var data = NJson.Decode<PuzzleData>(fileStr);
        if (data != null)
            puzzleData = data;
    }

    bool canClick = false;
    public void SetClickStatus(bool onoff) => canClick = onoff;
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
}
