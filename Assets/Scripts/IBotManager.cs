using UnityEngine;

public interface IBotManager
{
    void Init ();
    void TrySpawnBot(Vector2Int tile);
}