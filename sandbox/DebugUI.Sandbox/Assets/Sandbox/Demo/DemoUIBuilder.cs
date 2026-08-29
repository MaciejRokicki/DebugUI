using System;
using System.Collections.Generic;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace DebugUI.Sandbox
{
    public partial class DemoUIBuilder : DebugUIBuilderBase
    {
        [Serializable]
        class TestCollectionItem
        {
            public string Label;
            public int Value;
        }

        [AutoStaticsCleanup]
        static DemoUIBuilder instance;

        [SerializeField] GameObject prefab;
        [SerializeField] Volume volume;
        [SerializeField] List<TestCollectionItem> collection;

        ColorAdjustments colorAdjustments;
        Bloom bloom;

        float GravityScale
        {
            get
            {
                return Physics2D.gravity.y / -9.81f;
            }
            set
            {
                var x = Physics2D.gravity.x;
                Physics2D.gravity = new Vector2(x, value * -9.81f);
            }
        }

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(instance);

            volume.profile.TryGet(out colorAdjustments);
            volume.profile.TryGet(out bloom);
        }

        protected override void Configure(IDebugUIBuilder builder)
        {
            builder.ConfigureWindowOptions(options =>
            {
                options.Title = "Demo";
            });

            builder.AddTabView(builder =>
            {
                builder.AddTab("Physics", builder =>
                {
                    builder.AddSlider("Time Scale", 0f, 3f, () => Time.timeScale, x => Time.timeScale = x);
                    builder.AddSlider("Gravity Scale", 0f, 3f, () => GravityScale, x => GravityScale = x);
                    builder.AddButton("Add Circle", () => Instantiate(prefab));
                    builder.AddButton("Reload Scene", () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
                });
                builder.AddTab("Post-processing", builder =>
                {
                    builder.AddSlider("Hue Shift", -180f, 180f, () => colorAdjustments.hueShift.value, x => colorAdjustments.hueShift.value = x);
                    builder.AddSlider("Bloom Intensity", 0f, 10f, () => bloom.intensity.value, x => bloom.intensity.value = x);
                });
                builder.AddTab("List View", builder =>
                {
                    builder.AddListView("LV", 200.0f, collection, () =>
                    {
                        VisualElement visualElement = new VisualElement();
                        visualElement.style.display = DisplayStyle.Flex;
                        visualElement.style.flexDirection = FlexDirection.Row;
                        visualElement.style.justifyContent = Justify.SpaceBetween;

                        VisualElement labelContainer = new VisualElement();
                        labelContainer.style.display = DisplayStyle.Flex;
                        labelContainer.style.flexDirection = FlexDirection.Row;
                        var label = new Label();
                        label.name = "Label";
                        labelContainer.Add(label);
                        var value = new Label();
                        value.name = "Value";
                        labelContainer.Add(value);
                        var button = new Button()
                        {
                            text = "Log"
                        };
                        button.name = "Button";
                        button.RegisterCallback<ClickEvent, VisualElement>((e, button) => { Debug.Log(collection[(int)button.userData].Label); }, button);
                        visualElement.Add(labelContainer);
                        visualElement.Add(button);
                        return visualElement;
                    }, (VisualElement visualElement, int index) =>
                    {
                        var element = collection[index];
                        visualElement.Q<Label>("Label").text = element.Label;
                        visualElement.Q<Label>("Value").SetText(element.Value);
                        visualElement.Q<Button>("Button").userData = index;
                    }, (VisualElement visualElement, int index) =>
                    {
                        visualElement.Q<Button>("Button").UnregisterAllRemovableCallbacks();
                    });
                });
                builder.AddButton("Add", () =>
                {
                    collection.Add(new TestCollectionItem() { Label = string.Concat("Test_", UnityEngine.Random.Range(0, 999)), Value = UnityEngine.Random.Range(0, 999) });
                    root.Q<ListView>().Rebuild();
                });
                builder.AddButton("Remove", () =>
                {
                    collection.RemoveAt(collection.Count - 1);
                    root.Q<ListView>().Rebuild();
                });
            });
        }
    }
}