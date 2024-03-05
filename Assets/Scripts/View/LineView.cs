using System.Collections.Generic;

public static class LineView
{
    public static void EnableOutline(List<BallInfo> ballInfos)
    {
        foreach (BallInfo ballInfo in ballInfos)
        {
            ballInfo.Ball.gameObject.GetComponent<Outline>().enabled = true;
        }
    }
    
    public static void DisableOutline(List<BallInfo> ballInfos)
    {
        foreach (BallInfo ballInfo in ballInfos)
        {
            ballInfo.Ball.gameObject.GetComponent<Outline>().enabled = false;
        }
    }
}
