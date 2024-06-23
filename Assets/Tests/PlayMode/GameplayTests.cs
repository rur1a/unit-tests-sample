using System.Collections;
using FluentAssertions;
using UnityEngine;
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

    }
}
