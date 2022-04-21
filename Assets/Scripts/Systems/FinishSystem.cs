using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.UI;

namespace Client 
{
    sealed class FinishSystem : IEcsRunSystem 
    {
        readonly EcsFilterInject<Inc<FinishComponent>> _filter = default;

        public void Run(EcsSystems systems)
        {
            var gameState = systems.GetShared<GameState>();
            

            foreach (int entity in _filter.Value)
            {
                Time.timeScale = 0;

                Debug.Log("Finish");

                gameState.finishPanel.SetActive(true);
            }
        }
    }
}