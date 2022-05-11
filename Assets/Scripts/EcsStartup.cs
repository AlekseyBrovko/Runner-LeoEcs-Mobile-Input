using UnityEngine;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using UnityEngine.UI;

namespace Client 
{
    sealed class EcsStartup : MonoBehaviour
    {
        EcsSystems _systems;
        EcsSystems _fixedUpdate;
        [SerializeField] private Text coinText;
        [SerializeField] private GameObject finishPanel;

        void Start () 
        {
            EcsWorld world = new EcsWorld ();
            GameState state = new GameState();
            state.CoinsValueText = coinText;
            state.finishPanel = finishPanel;

            // register your shared data here, for example:
            // var shared = new Shared ();
            // systems = new EcsSystems (new EcsWorld (), shared);

            _systems = new EcsSystems (world, state);
            _fixedUpdate = new EcsSystems (world, state);
            _fixedUpdate.Add (new PlayerMoveSystem());
            _fixedUpdate.Add (new FinishSystem());
            _fixedUpdate.Add (new CameraFollowSystem());
            _fixedUpdate.Inject();
            _fixedUpdate.Init ();
            _systems
                //updateSystems = new EcsSystems (new EcsWorld(), state)
                
                .Add(new InitPlayer())
                .Add(new InputSystem())                
                .Add(new CoinsSystem())
                
                //.Add(new CameraFollowSystem())
                //.Add(new PlayerMoveSystem())
                //.Add(new FinishSystem())
                // .Add (new TestSystem2 ())
                // register additional worlds here, for example:
                // .AddWorld (new EcsWorld (), "events")
#if UNITY_EDITOR
                // add debug systems for custom worlds here, for example:
                // .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ())
#endif
                .DelHere<GetBonusEvent>()
                .Inject()
                .Init ();
        }

        void Update () 
        {
            _systems?.Run();
        }

        private void FixedUpdate()
        {
            _fixedUpdate.Run();
        }

        void OnDestroy () {
            if (_systems != null) {
                _systems.Destroy ();
                // add here cleanup for custom worlds, for example:
                // _systems.GetWorld ("events").Destroy ();
                _systems.GetWorld ().Destroy ();
                _systems = null;
            }
        }
    }
}