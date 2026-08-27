using Unity.Scripting.LifecycleManagement;
using UnityEngine;

namespace DebugUI
{
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    internal sealed partial class UpdateDispatcher : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            instance = new GameObject(nameof(UpdateDispatcher)).AddComponent<UpdateDispatcher>();
            DontDestroyOnLoad(instance);
        }

        [AutoStaticsCleanup]
        static UpdateDispatcher instance;

        readonly UpdateRunner updateRunner = new(ex => Debug.LogException(ex));

        public static void Register(IUpdateRunnerItem item)
        {
            instance.updateRunner.Add(item);
        }

        void Update()
        {
            updateRunner.Run();
        }
    }
}