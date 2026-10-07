using UnityEngine;

public static class GameController
{
    private static int collectableCount = 4;

    public static bool gameOver
    {
        get { return collectableCount <= 0; }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void Init()
    {
        collectableCount = 4;
    }

    public static void Collect()
    {
        collectableCount--;
    }

}
