public enum GameObjectType
{
    /// <summary>
    /// 골드 아이콘이 날아가는 위치
    /// </summary>
    GOLDICON_GET_POSITION,
    /// <summary>
    /// 캡슐 아이콘이 날아가는 위치
    /// </summary>
    KARMAICON_GET_POSITION,
    ///// <summary>
    ///// 업그레이드 화면의 현재 메인 Panel
    ///// </summary>
    //GAMEINFO_PANEL,

    /// <summary>
    /// 케릭터 배치시 사용 불가 리스트
    /// </summary>
    CHARACTER_SPAWN_BAN_POSITION,
    /// <summary>
    /// 케릭터 배치시 해당 케릭터 리스트가 없을 경우 저장이 되지 않는다
    /// </summary>
    CHARACTER_SPAWN_FORCE_POSITION,

    /// <summary>
    /// 보스 드랍 상자 날아가는 위치
    /// </summary>
    ARMSBOX_GET_FLY_POSITION,

    NewItemAlram,
}