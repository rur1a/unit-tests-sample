using UnityEngine;

public class FakeBotManager : IBotManager
{
    public int SpawnBots { get; private set; }

    public void Init()
    {
    }

    public void TrySpawnBot(Vector2Int tile)
    {
        SpawnBots++;
    }
}