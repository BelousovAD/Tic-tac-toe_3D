using System;

public class GameSettings : IDisposable
{
    public GameMode GameMode { get { return m_gameMode; } }
    public BallColor BallColor {  get { return m_ballColor; } }

    private GameMode m_gameMode;
    private BallColor m_ballColor = BallColor.WhiteBall;

    public GameSettings()
    {
        MenuGameMode.OnSendData += SetGameMode;
        MenuBallColor.OnSendData += SetBallColor;
    }

    public void Dispose()
    {
        MenuGameMode.OnSendData -= SetGameMode;
        MenuBallColor.OnSendData -= SetBallColor;
    }

    private void SetGameMode(GameMode gameMode)
    {
        m_gameMode = gameMode;
    }

    private void SetBallColor(BallColor ballColor)
    {
        m_ballColor = ballColor;
    }
}
