using UnityEngine;

public class Bot : Player
{
    private BallSpawnPosition[,] m_ballSpawnPositions;

    public Bot(BallSpawnPosition[] ballSpawnPositions)
        : base("Bot")
    {
        int dimension = (int)Mathf.Sqrt(ballSpawnPositions.Length);
        m_ballSpawnPositions = new BallSpawnPosition[dimension, dimension];

        foreach (BallSpawnPosition ballSpawnPosition in ballSpawnPositions)
        {
            Vector3Int position = ballSpawnPosition.Position;
            m_ballSpawnPositions[position.x, position.y] = ballSpawnPosition;
        }
    }
}
