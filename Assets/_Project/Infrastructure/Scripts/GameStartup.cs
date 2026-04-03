using UnityEngine;
using Zenject;

namespace ArmyCommander.Infrastructure
{
    public class GameStartup : IInitializable
    {
        public void Initialize()
        {
            Debug.Log("Game started");
        }
    }
}