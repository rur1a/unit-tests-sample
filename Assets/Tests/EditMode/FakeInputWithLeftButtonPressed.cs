using UnityEngine;

namespace Tests.EditMode
{
    public class FakeInputWithLeftButtonPressed : IInput
    {
        public bool LeftMouseButtonPressedInput() => true;
        public Vector2Int MapTileUnderMouseCursor(Transform marker) => Vector2Int.zero;
    }
}