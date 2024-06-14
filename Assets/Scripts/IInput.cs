using UnityEngine;

public interface IInput
{
    bool LeftMouseButtonPressedInput();
    Vector2Int MapTileUnderMouseCursor(Transform marker);
}