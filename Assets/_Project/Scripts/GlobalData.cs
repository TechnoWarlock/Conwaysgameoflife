public class GlobalData
{
    public static bool BoardHasCells { get; private set; }

    public static void SetPlay(SpawnManager spawnManager, bool value)
    {
        if (!spawnManager) return;
        BoardHasCells = value;
    }
}