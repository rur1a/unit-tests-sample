public static class ServiceLocator
{
    private static IInput _input;
    public static void Configure(IInput input) => _input = input;
    public static IInput Input() => _input ??= new UnityInput();
}