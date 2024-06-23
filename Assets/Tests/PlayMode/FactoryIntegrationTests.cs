using System.Collections;
using FluentAssertions;
using NSubstitute;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    public class FactoryIntegrationTests
    {
        [UnityTest]
        public IEnumerator WhenLeftMouseClicked_AndTileIsEmpty_ThenAtLeast1BotSpawned()
        {
            //ARRANGE
            var sceneLoading = SceneManager.LoadSceneAsync("_Scenes/game", LoadSceneMode.Single);
            yield return new WaitUntil(()=>sceneLoading.isDone);

            var fakeInput = Substitute.For<IInput>();
            fakeInput.LeftMouseButtonPressedInput().Returns(true);
            fakeInput.MapTileUnderMouseCursor(null).ReturnsForAnyArgs(new Vector2Int(1, 1));
            
            var botManager = Object.FindObjectOfType<BotManager>();
            var factoryManager = Object.FindObjectOfType<FactoryManager>();
            factoryManager.Construct(fakeInput, botManager);

            //ACT 
            yield return null;

            //ASSERT
            botManager.Bots.Should().NotBeEmpty();

        }

    }
}