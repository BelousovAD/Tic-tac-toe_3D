using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Line : IDisposable
{
    public static UnityAction<int> OnWinnerFounded;
    public static UnityAction OnStateChecked;
    public static UnityAction OnFilled;

    private int m_rank;
    private Vector3 m_point, m_dirVector;
    private List<BallInfo> m_ballInfos;

    private bool WinnerChecked { get; set; } = false;

    public Line(Vector3 point, Vector3 dirVector, int rank)
    {
        m_rank = rank;
        m_dirVector = dirVector;
        m_ballInfos = new List<BallInfo>();
        m_point = point;

        BallSpawner.OnSpawned += TryAddBall;
    }

    public void Dispose()
    {
        BallSpawner.OnSpawned -= TryAddBall;        
    }

    public void TryAddBall(BallInfo ballInfo)
    {
        if (!IsFull() && IsBallBelongingToLine(ballInfo.Position))
        {
            m_ballInfos.Add(ballInfo);
        }

        CheckWinner();
    }

    public void CheckWinner()
    {
        if (IsFull())
        {
            int playerId = m_ballInfos[0].PlayerId;

            if (!WinnerChecked && IsWinningLine(playerId))
            {
                LineView.EnableOutline(m_ballInfos);
                OnWinnerFounded?.Invoke(playerId);
            }
            else
            {
                OnFilled?.Invoke();
            }
        }
        else
        {
            OnStateChecked?.Invoke();
        }
    }

    public bool IsFull()
    {
        return m_ballInfos.Count == m_rank;
    }

    private bool IsBallBelongingToLine(Vector3Int ballPosition)
    {
        return Vector3.Cross((ballPosition - m_point), m_dirVector) == Vector3.zero; 
    }

    private bool IsWinningLine(int playerId)
    {
        bool winFlag = true;

        for (int i = 1; i < m_ballInfos.Count; ++i)
        {
            if (m_ballInfos[i].PlayerId != playerId)
            {
                winFlag = false;
                break;
            }
        }
        WinnerChecked = true;
        return winFlag;
    }
}
