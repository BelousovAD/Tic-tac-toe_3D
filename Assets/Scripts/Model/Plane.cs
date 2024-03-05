using System.Collections.Generic;
using UnityEngine;

public class Plane // 3*3 Line
{
    private int m_rank;
    private Vector3 m_start, m_firstDirVector, m_secondDirVector;
    private List<List<Line>> m_emittedLines;

    public bool IsFull { get; private set; } = false;

    public Plane(
        Vector3 start,
        Vector3 firstDirVector,
        Vector3 secondDirVecor,
        Vector3 lineDirVector,
        int rank)
    {
        m_rank = rank;
        m_start = start;
        m_firstDirVector = firstDirVector;
        m_secondDirVector = secondDirVecor;
        m_emittedLines = new List<List<Line>>(rank);

        for (int i = 0; i < m_rank; ++i)
        {
            m_emittedLines.Add(new List<Line>(rank));

            for (int j = 0; j < m_rank; ++j)
            {
                m_emittedLines[i].Add(
                    new Line(
                        m_start + m_firstDirVector * i + m_secondDirVector * j,
                        lineDirVector,
                        rank)
                    );
            }
        }
    }
}
