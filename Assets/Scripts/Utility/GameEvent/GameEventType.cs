
public enum GameEventType
{
    /// <summary>
    /// 테이블 파싱 끝
    /// </summary>
    EndTableParse,

    /// <summary>
    /// 초기화 진행도 param: ((int)FirebaseManager.eState)
    /// </summary>
    InitializeProgress,

    /// <summary>
    /// 초기화 끝
    /// </summary>
    InitializeFinish,

    /// <summary>
    /// 게임 스코어 변경
    /// </summary>
    ChangeScore,

    /// <summary>
    /// 게임 종료
    /// </summary>
    GameEnd,

    /// <summary>
    /// 콤보 연출
    /// </summary>
    EffectCombo,
}