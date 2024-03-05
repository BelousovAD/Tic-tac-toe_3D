using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    private Vector3 m_start, m_dirVector;
    private List<Line> m_emittedLines;
    private int m_rank;

    public Edge(
        Vector3 start,
        Vector3 dirVector,
        Vector3 lineDirVector,
        int rank)
    {
        m_start = start;
        m_rank = rank;
        m_dirVector = dirVector;
        m_emittedLines = new List<Line>(m_rank);
        for (int i = 0; i < m_rank; ++i)
        {
            m_emittedLines.Add(
                new Line(
                    m_start + m_dirVector * i,
                    lineDirVector,
                    m_rank)
                );
        }
    }
}
