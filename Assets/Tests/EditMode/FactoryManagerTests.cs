using FluentAssertions;
using NUnit.Framework;
using Tests.EditMode;
using UnityEngine;

public class FactoryManagerTests
{
    [Test]
    public void WhenLeftMouseButtonCLicked_ThenSpawn3Bots()
    {
        //ARRANGE
        ServiceLocator.Configure(new InputWithLeftButtonPressed());
        var gameObject = new GameObject();
        var factoryManager = gameObject.AddComponent<FactoryManager>();
        var botManager = gameObject.AddComponent<FakeBotManager>();
        factoryManager.Awake();

        //ACT 
        factoryManager.Update();

        //ASSERT
        botManager.SpawnBots.Should().Be(3);
    }
}

public class FakeBotManager : MonoBehaviour, IBotManager
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