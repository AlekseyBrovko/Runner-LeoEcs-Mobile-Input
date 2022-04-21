using Leopotam.EcsLite;
using UnityEngine;

namespace Client 
{
    sealed class InitPlayer : IEcsInitSystem 
    {
        public void Init (EcsSystems systems) 
        {
            var go = GameObject.FindGameObjectWithTag("Player"); //поиск игрока на сцене

            var world = systems.GetWorld(); // ссылка на мир
            var entity = world.NewEntity(); // создание новой сущности

            world.GetPool<Player>().Add(entity);

            ref var moveComp = ref world.GetPool<PlayerMoveComponent>().Add(entity); // добавление сущности в пул Move
            moveComp.ForwardSpeed = 15;

            ref var viewComp = ref world.GetPool<ViewComponent>().Add(entity);
            viewComp.Transform = go.transform; // сохранили ссылку на transform объекта

            viewComp.Rigidbody = go.GetComponent<Rigidbody>();

            ref var inputComp = ref world.GetPool<PlayerInputComponent>().Add(entity);
            moveComp.SideSpeed = 10005f;
            
            var coins = go.GetComponent<CoinTrigger>();
            coins.World = world;

            var finish = go.GetComponent<FinishTrigger>();
            finish.World = world;

            
            ref var touchComp = ref world.GetPool<TouchEvent>().Add(entity); //!!!!
            ref var inpComp = ref world.GetPool<InputComponent>().Add(entity);

        }
    }
}