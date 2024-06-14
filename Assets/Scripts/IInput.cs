using UnityEngine;

public interface IInput
{
    bool LeftMouseButtonPressedInput();
}

internal class UnityInput : IInput
{
    public bool LeftMouseButtonPressedInput() => Input.GetKey(KeyCode.Mouse0);
}