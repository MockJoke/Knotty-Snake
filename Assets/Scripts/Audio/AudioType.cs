
public enum AudioType
{
    None = 0,

    #region BG Music 1 - 50
    
    MenuBgM = 1,
    
    GamePlayBgM1 = 21,
    GamePlayBgM2 = 22,
    GamePlayBgM3 = 23,
    GamePlayBgM4 = 24,
    GamePlayBgM5 = 25,
    
    #endregion

    #region UI 51-100
    
    Click = 51,
    Hover = 52,
    PanelPopUp = 53,
    SceneTransition = 54,
    
    #endregion
    
    #region ItemCollection 101-150
    
    MassGainerCollect = 101,
    MassBurnerCollect = 102,
    ShieldCollect = 103,
    SpeedUpCollect = 104,
    ScoreBoostCollect = 105,
    
    ItemSpawn = 126,
    
    #endregion

    #region GameResult 151-200
    
    OnWin = 151,
    OnLoss = 152,
    OnDraw = 153,
    
    #endregion
}
