using System;
using UnityEngine;

public class BallInfo : IDisposable
{
    private int m_playerId;
    private Vector3Int m_position;
    private PooledObject m_ball;

    public BallInfo(int playerId, Vector3Int position, PooledObject ball)
    {
        m_playerId = playerId;
        m_position = position;
        m_ball = ball;
    }

    public int PlayerId {  get { return m_playerId; } }
    public Vector3Int Position {  get { return m_position; } }
    public PooledObject Ball {  get { return m_ball; } }

    public void Dispose()
    {
        m_ball.Deactivate();
    }
}
