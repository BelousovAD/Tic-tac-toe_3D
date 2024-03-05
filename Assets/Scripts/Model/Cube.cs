using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cube : IDisposable
{
    private const int k_dimention = 3;
    private const int k_planesNumber = k_dimention;
    private const int k_edgesNumber = k_dimention * 2;
    private const int k_verticesNumber = k_dimention + 1;

    private int m_checkedLinesCount = 0;
    private int m_filledLinesCount = 0;
    private int m_winnerLinesCount = 0;
    private int m_linesNumber;
    private int m_rank;
    private List<Plane> m_planes;
    private List<Edge> m_edges;
    private List<Line> m_emittedLinesFromVertices;

    private WaitUntil m_waitUntilGameStateIsValid;

    public bool? IsGameOver { get; private set; } = false;
    public int WinnerId { get; private set; } = -1;

    public Cube(int rank)
    {
        Line.OnStateChecked += AddCheckedLine;
        Line.OnFilled += AddFilledLine;
        Line.OnWinnerFounded += SetWinnerId;

        m_waitUntilGameStateIsValid = new WaitUntil(() => IsGameOver != null);

        m_rank = rank;
        m_linesNumber = k_planesNumber * m_rank * m_rank + k_edgesNumber * m_rank + k_verticesNumber;
        m_planes = new List<Plane>(k_planesNumber);
        m_edges = new List<Edge>(k_edgesNumber);
        m_emittedLinesFromVertices = new List<Line>(k_verticesNumber);

        Vector3 first = Vector3.right;
        Vector3 second = Vector3.up;
        Vector3 third = Vector3.forward;
        for (int i = 0; i < k_planesNumber; ++i)
        {
            m_planes.Add(
                new Plane(
                    Vector3.zero,
                    first,
                    second,
                    third,
                    m_rank)
                );
            m_edges.Add(
                new Edge(
                    Vector3.zero,
                    first,
                    third + second,
                    m_rank)
                );
            m_edges.Add(
                new Edge(
                    Vector3.zero + second * (m_rank - 1),
                    first,
                    third - second,
                    m_rank)
                );
            m_emittedLinesFromVertices.Add(
                new Line(
                    first * (m_rank - 1),
                    third + second - first,
                    m_rank)
                );

            Vector3 tmp = first;
            first = second;
            second = third;
            third = tmp;
        }

        m_emittedLinesFromVertices.Add(
            new Line(
                Vector3.zero,
                third + second + first,
                m_rank)
            );
    }

    public void Dispose()
    {
        Line.OnStateChecked -= AddCheckedLine;
        Line.OnFilled -= AddFilledLine;
        Line.OnWinnerFounded -= SetWinnerId;
    }

    public IEnumerator WaitGameStateRoutine()
    {

        yield return m_waitUntilGameStateIsValid;
    }

    public void ResetGameState()
    {
        IsGameOver = null;
    }

    private void AddCheckedLine()
    {
        ++m_checkedLinesCount;

        CheckGameState();
    }

    private void AddFilledLine()
    {
        ++m_filledLinesCount;

        CheckGameState();
    }

    private void AddWinnerLine()
    {
        ++m_winnerLinesCount;

        CheckGameState();
    }

    private void SetWinnerId(int playerId)
    {
        WinnerId = playerId;
        AddWinnerLine();

        CheckGameState();
    }

    private void CheckGameState()
    {
        if (m_filledLinesCount == m_linesNumber)
        {
            IsGameOver = true;
        }
        else if (m_checkedLinesCount + m_filledLinesCount == m_linesNumber)
        {
            IsGameOver = false;
            m_checkedLinesCount = 0;
            m_filledLinesCount = 0;
            m_winnerLinesCount = 0;
        }
        else if (m_winnerLinesCount + m_filledLinesCount + m_checkedLinesCount == m_linesNumber)
        {
            IsGameOver = true;
        }
    }
}
