using FluentAssertions;
using Moq;
using NSubstitute;
using NUnit.Framework;
using Tests.EditMode;
using UnityEngine;

public class FactoryManagerTests
{
    [Test]
    public void WhenLeftMouseButtonClicked_ThenSpawn3Bots()
    {
        //ARRANGE
        var fakeInput = new FakeInputWithLeftButtonPressed();
        var fakeBotManager = new FakeBotManager();
        
        var factoryManager = new GameObject().AddComponent<FactoryManager>();
        factoryManager.Construct(fakeInput, fakeBotManager);

        //ACT 
        factoryManager.HandleInput();

        //ASSERT
        fakeBotManager.SpawnBots.Should().Be(3);
    }
    
    [Test]
    public void WhenLeftMouseButtonClicked_ThenSpawn3Bots_Moq()
    {
        //ARRANGE
        var fakeInput = Mock.Of<IInput>(x=>x.LeftMouseButtonPressedInput() == true);
        var fakeBotManager = new Mock<IBotManager>();
        
        var factoryManager = new GameObject().AddComponent<FactoryManager>();
        factoryManager.Construct(fakeInput, fakeBotManager.Object);

        //ACT 
        factoryManager.HandleInput();

        //ASSERT
        fakeBotManager.Verify(
            x=>x.TrySpawnBot(It.IsAny<Vector2Int>()),
            Times.Exactly(3));
    }

    [Test]
    public void WhenLeftMouseButtonClicked_ThenSpawn3Bots_NSubstitute()
    {
        //ARRANGE
        var fakeInput = Substitute.For<IInput>();
        fakeInput.LeftMouseButtonPressedInput().Returns(true);
        var fakeBotManager = Substitute.For<IBotManager>();
        
        var factoryManager = new GameObject().AddComponent<FactoryManager>();
        factoryManager.Construct(fakeInput, fakeBotManager);

        //ACT 
        factoryManager.HandleInput();

        //ASSERT
        fakeBotManager.Received(3).TrySpawnBot(Arg.Any<Vector2Int>());
    }

}