using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    [Header("SIZE"), Space(5)]
    [SerializeField] int row = 5;
    [SerializeField] int col = 7;

    [Header("Block"), Space(10)]
    [SerializeField] Block originBlock = null;
    [SerializeField] Transform[] floors = null;

    public static PuzzleController instance = null;

    int[,] board = null;    // [0,?] => 가장 위층
    List<List<Block>> blockList = null;
    System.Random rand = new System.Random();

    int curScore = 0;
    private void Awake()
    {
        initailize();
    }
    public void initailize()
    {
        if (instance == null)
            instance = this;

        curScore = 0;
        board = new int[col, row];
        blockList = new List<List<Block>>();
        // 블럭, 프레임 세팅
        initFrame();

        TryGetBlock();
        UpdateBlock();
        TryGetBlock();
        UpdateBlock();
        RenderBlock();
    }
    public void ReSetStage()
    {
        curScore = 0;
        board = new int[col, row];
        TryGetBlock();
        UpdateBlock();
        RenderBlock();
    }
    public void initFrame()
    {
        // 프레임은 일단 있다고 생각하자

        // 블록을 모두 배치
        float xPos = -1.5f;
        for (int i = 0; i < col; i++)
        {
            xPos = -1.5f;
            blockList.Add(new List<Block>());
            for(int j = 0; j < row; j++)
            {
                var obj = Instantiate(originBlock, floors[i]);
                obj.transform.localPosition = new Vector3(xPos, 0, 0);
                obj.initData(i, j);
                blockList[i].Add(obj);
                xPos += 0.685f;
            }
        }
    }
    public void RenderBlock()
    {
        for(int i = 0; i < col; i++)
        {
            for(int j = 0; j < row; j++)
            {
                blockList[i][j].SetBlock(board[i, j]);
            }
        }

        GameEventSubject.SendGameEvent(GameEventType.ChangeScore, curScore);
    }
    public bool TryGetBlock()
    {
        if (CheckFail()) return false;

        bool needUp = false;
        for(int i = 0; i < row; i++)
        {
            if (board[col - 1, i] == 0)
                continue;
            needUp = true;
            break;
        }

        if(needUp)
        {
            for(int i = 1; i < col; i++)
            {
                for(int j = 0; j < row; j++)
                {
                    if (board[i, j] == 0)
                        continue;
                    board[i - 1, j] = board[i, j];
                }
            }

            for (int i = 0; i < row; i++)
                board[col - 1, i] = 0;

            RandBlock();
        }
        else
        {
            RandBlock();
        }

        return true;
    }
    void RandBlock()
    {
        for(int i = 0; i < row; i++)
        {
            var num = rand.Next(1, originBlock.GetLength);
            board[col - 1, i] = num;
        }
    }
    public bool CheckFail()
    {
        bool result = false;

        for(int i = 0; i < row; i++)
        {
            if (board[0, i] == 0)
                continue;

            result = true;
            break;
        }

        return result;
    }
    // 블록을 부순뒤에 위치 정리
    public void UpdateBlock()
    {
        Queue<int> queue = new Queue<int>();
        for(int i = 0; i < row; i++)
        {
            for(int j = col - 1; j >= 0; j--)
            {
                if (board[j, i] != 0)
                    queue.Enqueue(board[j, i]);
            }

            for(int j = col - 1; j >= 0; j--)
            {
                if (queue.Count > 0)
                    board[j, i] = queue.Dequeue();
                else
                    board[j, i] = 0;
            }
        }
    }
    void OnClick_Block(int index1, int index2)
    {
        var value = board[index1, index2];
        board[index1, index2] = 0;
        blockList[index1][index2].SetBlock(0);
        curScore += 1;
        ClearBlock_Recursive(index1, index2, value);
        //UpdateBlock();
        if(TryGetBlock() == false)
        {
            GameEventSubject.SendGameEvent(GameEventType.ChangeScore, curScore);
            ReSetStage();
            return;
        }
        //RenderBlock();
    }
    void ClearBlock_Recursive(int i, int j, int value)
    {
        if (value == 0) return;
        // 좌측
        if(j > 0 && board[i, j - 1] == value)
        {
            board[i, j - 1] = 0;
            blockList[i][j - 1].SetBlock(0);
            curScore += 1;
            ClearBlock_Recursive(i, j - 1, value);
        }

        // 우측
        if(j < row - 1 && board[i, j + 1] == value)
        {
            board[i, j + 1] = 0;
            blockList[i][j + 1].SetBlock(0);
            curScore += 1;
            ClearBlock_Recursive(i, j + 1, value);
        }

        // 상단
        if(i > 0 && board[i - 1, j] == value)
        {
            board[i - 1, j] = 0;
            blockList[i - 1][j].SetBlock(0);
            curScore += 1;
            ClearBlock_Recursive(i - 1, j, value);
        }

        // 하단
        if(i < col - 1 && board[i + 1, j] == value)
        {
            board[i + 1, j] = 0;
            blockList[i + 1][j].SetBlock(0);
            curScore += 1;
            ClearBlock_Recursive(i + 1, j, value);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

            if(hit.collider != null && hit.transform.gameObject.tag == "Block")
            {
                if (hit.transform.TryGetComponent<Block>(out var obj))
                    OnClick_Block(obj.GetIndex.Item1, obj.GetIndex.Item2);
            }
        }
    }

    // 테스트용 치트코드
    public void Cheat()
    {
        UpdateBlock();
        TryGetBlock();
        RenderBlock();
    }
}
