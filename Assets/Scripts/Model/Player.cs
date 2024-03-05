using UnityEngine;

public class Player
{
    private static int m_idCount = 0;
    private int m_id;
    private string m_name;

    public Player()
    {
        m_id = m_idCount++;
        m_name = "Player_" + m_id;
    }

    public Player(string name) : this()
    {
        m_name = name;
    }

    public string Name { get { return m_name; } }

    public int Id { get { return m_id; } }
}
