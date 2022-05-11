using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client 
{
    sealed class PlayerMoveSystem : IEcsRunSystem 
    {
        readonly EcsFilterInject<Inc<PlayerMoveComponent, Player>> _filter = default; //������ �������� ���������� �� ������� ���� Move � Player
        
        readonly EcsPoolInject<PlayerMoveComponent> _movePool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<PlayerInputComponent> _inputPool = default;
        
        readonly EcsPoolInject<TouchEvent> _touchPool = default;
        readonly EcsPoolInject<InputComponent> _InputComponentPool = default;


        public void Run(EcsSystems systems)
        {
            foreach (int entity in _filter.Value)
            {
                ref PlayerMoveComponent moveComp = ref _movePool.Value.Get(entity);
                ref ViewComponent viewComp = ref _viewPool.Value.Get(entity);
                ref PlayerInputComponent inputComp = ref _inputPool.Value.Get(entity);
                ref InputComponent inpComp = ref _InputComponentPool.Value.Get(entity);
                ref TouchEvent touchEvent = ref _touchPool.Value.Get(entity);



                //inputComp.moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);                
                //viewComp.Rigidbody.MovePosition(viewComp.Transform.position + Vector3.forward * moveComp.ForwardSpeed * Time.deltaTime + inputComp.moveInput * moveComp.SideSpeed * Time.deltaTime);

                //���������
                inputComp.moveInput = new Vector3(touchEvent.Direction.x * touchEvent.Velocity.x * moveComp.SideSpeed, 0, 0);
                viewComp.Rigidbody.MovePosition(viewComp.Transform.position + Vector3.forward * moveComp.ForwardSpeed * Time.deltaTime + inputComp.moveInput * Time.deltaTime);
            }
        }
    }
}