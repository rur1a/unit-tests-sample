using FluentAssertions;
using NUnit.Framework;
using Tests.EditMode;
using UnityEngine;

public class FactoryManagerTests
{
    [Test]
    public void WhenLeftMouseButtonClicked_ThenSpawn3Bots()
    {
        //ARRANGE
        var factoryManager = new GameObject().AddComponent<FactoryManager>();
        var fakeBotManager = new FakeBotManager();
        factoryManager.Construct(new InputWithLeftButtonPressed(), fakeBotManager);

        //ACT 
        factoryManager.HandleInput();

        //ASSERT
        fakeBotManager.SpawnBots.Should().Be(3);
    }
}

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