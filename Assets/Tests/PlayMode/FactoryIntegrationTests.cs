using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    public class FactoryIntegrationTests
    {
        [UnitySetUp]
        public IEnumerator SetUp()
        {
            var sceneLoading = SceneManager.LoadSceneAsync("_Scenes/game", LoadSceneMode.Single);
            yield return new WaitUntil(()=>sceneLoading.isDone);
        }
        
        [UnityTest]
        public IEnumerator WhenLeftMouseClicked_AndTileIsEmpty_ThenAtLeast1BotSpawned()
        {
            //ARRANGE
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

        [UnityTest]
        public IEnumerator When1BotSpawned_AndTimePasses_ThenBotMovesClockwise()
        {
            //ARRANGE
            var waitFor1Second = new WaitForSeconds(1);
            var botManager = Object.FindObjectOfType<BotManager>();
            botManager.TrySpawnBot(new Vector2Int(1,25));

            //ACT
            var botAngles = new List<float>();
            for(int i =0; i<10; i++)
            {
                yield return waitFor1Second;
                var centerPosition = new Vector2(15, 15);
                var botRelativePosition = botManager.Bots[0].position - centerPosition;
                botAngles.Add(Vector2.SignedAngle(botRelativePosition, Vector2.zero));
            }

            //ASSERT
            botAngles.Should().BeInAscendingOrder();
        }


    }
}