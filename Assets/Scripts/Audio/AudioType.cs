
public enum AudioType
{
    None = 0,

    #region BG Music 1 - 10
    
    bg1 = 1,
    bg2 = 2,
    bg3 = 3,
    bg4 = 4,
    bg5 = 5,
    bg6 = 6,
    bg7 = 7,
    bg8 = 8,
    bg9 = 9,
    bg10 = 10,
    
    #endregion

    #region UI 11-20
    
    Click = 11,
    Hover = 12,
    PanelPopUp = 13,
    GameStart = 14,
    #endregion
    
    #region ItemCollection 21-40
    
    MassGainerCollect = 11,
    MassBurnerCollect = 12,
    ShieldCollect = 13,
    SpeedUpCollect = 14,
    ScoreBoostCollect = 15,
    
    #endregion

    #region GameResult 41-50
    
    OnWin = 31,
    OnLoss = 32,
    OnDraw = 33,
    
    #endregion
}
