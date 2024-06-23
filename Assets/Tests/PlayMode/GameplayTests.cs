using System.Collections;
using System.Xml.Schema;
using FluentAssertions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    public class GameplayTests
    {
        [UnityTest]
        public IEnumerator When1FramePassed_ThenDeltaTimeShouldBePositive()
        {
            //ARRANGE
            yield return null;

            //ASSERT
            Time.deltaTime.Should().BePositive();
        }

        [UnityTest]
        public IEnumerator WhenGameSceneLoaded_ThenItContainsFactoryManager()
        {
            //ARRANGE
            var sceneLoading = SceneManager.LoadSceneAsync("_Scenes/game", LoadSceneMode.Single);
            yield return new WaitUntil(()=>sceneLoading.isDone);
            
            //ASSERT
            Object.FindObjectOfType<FactoryManager>().Should().NotBeNull();
        }


    }
}
