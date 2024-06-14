using UnityEngine;

internal class UnityInput : IInput
{
    private Camera MainCamera => Camera.main;
    public bool LeftMouseButtonPressedInput() => Input.GetKey(KeyCode.Mouse0);

    public Vector2Int MapTileUnderMouseCursor(Transform marker)
    {
        Vector3 mousePos = MainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos = marker.InverseTransformPoint(mousePos);
        Vector2Int tile = new Vector2Int(Mathf.FloorToInt(mousePos.x + .5f),Mathf.FloorToInt(mousePos.z + .5f));
        return tile;
    }

}