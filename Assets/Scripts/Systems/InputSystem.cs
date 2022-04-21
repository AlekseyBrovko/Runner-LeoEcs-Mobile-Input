using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using InputTouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Client 
{
    sealed class InputSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        GameState _state = null;
        
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<InputComponent>> _filter = default;
        readonly EcsPoolInject<TouchEvent> _touchPool = default; //!!!
        readonly EcsPoolInject<InputComponent> _InputComponentPool = default;

        //private EcsFilterExt<InputComponent>.Exc<DisableInput> _filter;
        //private EcsPool<TouchEvent> _touchPool = null;

        private float _stationaryTimer, _debounceTimer;


        public void Init(EcsSystems systems)
        {
            EnhancedTouchSupport.Enable(); // активировали ввод
            TouchSimulation.Enable(); // клик будет симулировать touch
            Debug.Log($"InputSystem: Init - EnhancedTouchSupport.enabled = {EnhancedTouchSupport.enabled}");

            _state = systems.GetShared<GameState>();
            var PlayerEntity = _InputComponentPool.Value.GetWorld().NewEntity(); 
            ref var unit = ref _InputComponentPool.Value.Add(PlayerEntity); 
        }


        public void Run (EcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                if (Touch.activeTouches.Count == 0) return;
                var activeTouch = Touch.activeTouches[0];
                Debug.Log("Не нажимаем");

                var phase = activeTouch.phase;
                if (phase == InputTouchPhase.Began)
                {
                    // если фаза Began - начать отсчет
                    Debug.Log(Touch.activeTouches.Count);
                    _debounceTimer = 0;
                    if (!_touchPool.Value.Has(entity))
                    {
                        // Debug.Log(Touch.activeTouches.Count);
                        _touchPool.Value.Add(entity);
                    }
                    ref var touchComp = ref _touchPool.Value.Get(entity);
                    touchComp.Phase = TouchPhase.Began;
                    touchComp.Direction = Vector3.zero;
                    touchComp.Velocity = Vector3.zero;
                    return;
                }

                if (phase == InputTouchPhase.Ended || phase == InputTouchPhase.Canceled)
                {
                    _debounceTimer = 0;
                    if (!_touchPool.Value.Has(entity))
                    {
                        _touchPool.Value.Add(entity);
                    }
                    ref var touchComp = ref _touchPool.Value.Get(entity);
                    touchComp.Phase = TouchPhase.Ended;
                    touchComp.Direction = Vector3.zero;
                    touchComp.Velocity = Vector3.zero;
                    return;
                }

                _debounceTimer += Time.deltaTime;
                if (_debounceTimer < .1) return;

                if (phase == InputTouchPhase.Moved)
                {
                    _stationaryTimer = 0;

                    if (!_touchPool.Value.Has(entity)) _touchPool.Value.Add(entity);
                    ref var touchComp = ref _touchPool.Value.Get(entity);

                    //if (activeTouch.delta.x > _state.Config.MinDeltaX)
                    if (activeTouch.delta.x > .1)
                    {
                        touchComp.Phase = TouchPhase.Moved;
                        touchComp.Velocity.x = activeTouch.delta.x; // * _state.Config.UnitInPixel;
                        // activeTouch.delta.x / (float)activeTouch.time * _state.Config.UnitInPixel;
                        touchComp.Direction.x = 1;
                        return;
                    }
                    //else if (activeTouch.delta.x < -_state.Config.MinDeltaX)
                    else if (activeTouch.delta.x < .1)
                    {
                        touchComp.Phase = TouchPhase.Moved;
                        touchComp.Velocity.x = -activeTouch.delta.x; // * _state.Config.UnitInPixel;
                        touchComp.Direction.x = -1;
                        return;
                    }
                }
                else if (phase == InputTouchPhase.Stationary)
                {
                    // чтобы не дергался во время медленного движения пальцем
                    // если зашли в состояние Stationary, то не сразу убирать старый Direction, а через 0.2 сек
                    if (!_touchPool.Value.Has(entity)) _touchPool.Value.Add(entity);
                    ref var touchComp = ref _touchPool.Value.Get(entity);
                    touchComp.Velocity = Vector3.zero;

                    _stationaryTimer += Time.deltaTime;
                    if (_stationaryTimer >= 0.2f)
                    {
                        touchComp.Phase = TouchPhase.Stationary;
                        touchComp.Direction = Vector3.zero;
                    }
                }
            }
        }

        public void Destroy(EcsSystems systems)
        {
            // Debug.Log("InputSystem: Destroy");
            EnhancedTouchSupport.Disable();
            TouchSimulation.Disable();
        }
    }
}