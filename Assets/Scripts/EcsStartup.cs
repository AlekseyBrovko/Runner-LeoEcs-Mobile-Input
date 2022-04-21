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
        [SerializeField] private Text coinText;
        [SerializeField] private GameObject finishPanel;

        void Start () 
        {
            
            GameState state = new GameState();
            state.CoinsValueText = coinText;
            state.finishPanel = finishPanel;

            // register your shared data here, for example:
            // var shared = new Shared ();
            // systems = new EcsSystems (new EcsWorld (), shared);
            _systems = new EcsSystems (new EcsWorld (), state);
            _systems
                //updateSystems = new EcsSystems (new EcsWorld(), state)
                .Add(new InitPlayer())
                .Add(new InputSystem())
                .Add(new PlayerMoveSystem())
                
                .Add(new CameraFollowSystem())
                .Add(new CoinsSystem())
                .Add(new FinishSystem())
                


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