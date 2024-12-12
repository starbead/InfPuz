using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    public static InGameManager instance = null;

    [SerializeField] BoardMaker board = null;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        board.initData();
    }

    public Vector2 GetPos(int x, int y)
    {
        return board.posList[x][y];
    }

    bool canClick = true;
    void OnClick_Block(int index1, int index2)
    {
        canClick = false;
        
    }
    private void Update()
    {
        if (canClick == false) return;

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

            if (hit.collider != null && hit.transform.gameObject.tag == "Block")
            {
                if (hit.transform.TryGetComponent<Block>(out var obj))
                    OnClick_Block(obj.GetIndex.Item1, obj.GetIndex.Item2);
            }
        }
    }
}
