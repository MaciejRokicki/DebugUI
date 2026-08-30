using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace DebugUI
{
    public abstract class DebugUIBuilderBase : MonoBehaviour
    {
        [SerializeField] PanelRenderer panelRenderer;
        [SerializeField] InputAction toggleAction;
        [SerializeField] bool developmentBuildOnly;

        protected VisualElement root;
        protected VisualElement debugWindowVisualElement;

        protected abstract void Configure(IDebugUIBuilder builder);

        protected virtual void Awake()
        {
            if (developmentBuildOnly)
            {
                if (!Debug.isDebugBuild)
                {
                    Destroy(gameObject);
                }
            }
        }

        protected virtual void OnEnable()
        {
            panelRenderer.RegisterUIReloadCallback(OnUIReload);
            toggleAction.Enable();
            toggleAction.performed += ToggleAction_performed;
        }

        protected virtual void OnDisable()
        {
            panelRenderer.UnregisterUIReloadCallback(OnUIReload);
            toggleAction.performed -= ToggleAction_performed;
            toggleAction.Disable();
        }

        protected virtual void OnUIReload(PanelRenderer renderer, VisualElement rootElement, int version)
        {
            root = rootElement;
            var builder = new DebugUIBuilder();
            builder.ConfigureWindowOptions(options =>
            {
                options.Title = GetType().Name;
            });

            Configure(builder);
            builder.BuildWith(rootElement);
            debugWindowVisualElement = root.Q<VisualElement>("DebugWindow");
        }

        private void ToggleAction_performed(InputAction.CallbackContext obj)
        {
            if (obj.performed)
            {
                debugWindowVisualElement.style.display = debugWindowVisualElement.style.display == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }
    }
}