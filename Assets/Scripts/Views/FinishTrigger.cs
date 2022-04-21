using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;

namespace Client
{
    public class FinishTrigger : MonoBehaviour
    {
        public EcsWorld World;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Finish"))
            {
                Debug.Log("---Finish---");

                ref var finishComp = ref World.GetPool<FinishComponent>().Add(World.NewEntity()); //добавить в пул новую сущность. Нужно написать систему, которая ловит такие компоненты

                //передадим какую нибудь информацию в ECS 
                //eventComp.CoinValue = 1;

                //other.gameObject.SetActive(false);
            }
        }
    }
}
