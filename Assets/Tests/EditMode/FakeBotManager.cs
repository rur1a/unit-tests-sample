using UnityEngine;

namespace Tests.EditMode
{
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
}