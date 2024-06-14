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
        var factoryManager = new GameObject().AddComponent<FactoryManager>();

        //ACT 
        factoryManager.Update();

        //ASSERT
    }
}