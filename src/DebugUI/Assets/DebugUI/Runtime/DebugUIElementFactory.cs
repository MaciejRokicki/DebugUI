using DebugUI.UIElements;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DebugUI
{
    public interface IDebugUIElementFactory
    {
        public VisualElement CreateVisualElement(ICollection<IDisposable> disposables);
    }

    internal sealed class DebugSpaceFactory : IDebugUIElementFactory
    {
        public string Name { get; set; }
        public float Height { get; set; }

        public VisualElement CreateVisualElement(ICollection<IDisposable> disposables)
        {
            var space = new VisualElement() { style = { height = Height } };

            if (!string.IsNullOrEmpty(Name))
                space.name = Name;

            return space;
        }
    }

    internal sealed class DebugLabelFactory : IDebugUIElementFactory
    {
        public string Name { get; set; }
        public string Text { get; set; }

        public VisualElement CreateVisualElement(ICollection<IDisposable> disposables)
        {
            var label = new Label(Text);

            if (!string.IsNullOrEmpty(Name))
                label.name = Name;

            return label;
        }
    }

    internal sealed class DebugButtonFactory : IDebugUIElementFactory
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public Action Action { get; set; }

        public VisualElement CreateVisualElement(ICollection<IDisposable> disposables)
        {
            var button = new Button(Action)
            {
                text = Text
            };

            if (!string.IsNullOrEmpty(Name))
                button.name = Name;

            return button;
        }
    }

    internal sealed class DebugSliderFactory : IDebugUIElementFactory
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public string Format { get; set; }
        public float LowValue { get; set; }
        public float HighValue { get; set; }
        public Func<float> Getter { get; set; }
        public Action<float> Setter { get; set; }

        public VisualElement CreateVisualElement(ICollection<IDisposable> disposables)
        {
            var field = new FillSlider()
            {
                label = Label,
                Format = Format,
                lowValue = LowValue,
                highValue = HighValue,
            };

            if (!string.IsNullOrEmpty(Name))
                field.name = Name;

            if (Setter == null)
            {
                field.SetEnabled(false);
            }
            else
            {
                field.RegisterValueChangedCallback(x => Setter(x.newValue));
            }

            if (Getter != null)
            {
                field.ForceUpdateValue(Getter());

                MinimalRx.EveryValueChanged(this, x => x.Getter())
                    .Subscribe(x =>
                    {
                        field.value = x;
                    })
                    .AddTo(disposables);
            }

            return field;
        }
    }

    internal sealed class DebugSliderIntFactory : IDebugUIElementFactory
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public string Format { get; set; }
        public int LowValue { get; set; }
        public int HighValue { get; set; }
        public Func<int> Getter { get; set; }
        public Action<int> Setter { get; set; }

        public VisualElement CreateVisualElement(ICollection<IDisposable> disposables)
        {
            var field = new FillSliderInt()
            {
                label = Label,
                Format = Format,
                lowValue = LowValue,
                highValue = HighValue,
            };

            if (!string.IsNullOrEmpty(Name))
                field.name = Name;

            if (Setter == null)
            {
                field.SetEnabled(false);
            }
            else
            {
                field.RegisterValueChangedCallback(x => Setter(x.newValue));
            }

            if (Getter != null)
            {
                field.ForceUpdateValue(Getter());

                MinimalRx.EveryValueChanged(this, x => x.Getter())
                    .Subscribe(x =>
                    {
                        field.value = x;
                    })
                    .AddTo(disposables);
            }

            return field;
        }
    }

    internal sealed class DebugProgressBarFactory : IDebugUIElementFactory
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public string Format { get; set; }
        public float LowValue { get; set; }
        public float HighValue { get; set; }
        public Func<float> Getter { get; set; }

        public VisualElement CreateVisualElement(ICollection<IDisposable> disposables)
        {
            var field = new ProgressBar()
            {
                lowValue = LowValue,
                highValue = HighValue,
            };

            if (!string.IsNullOrEmpty(Name))
                field.name = Name;

            field.schedule.Execute(() =>
            {
                var x = Getter();
                field.value = x;
                field.title = Label + " " + string.Format(Format, x);
            });

            if (Getter != null)
            {
                MinimalRx.EveryValueChanged(this, x => x.Getter())
                    .Subscribe(x =>
                    {
                        field.value = x;
                        field.title = Label + " " + string.Format(Format, x);
                    })
                    .AddTo(disposables);
            }

            return field;
        }
    }

    internal sealed class DebugFoldoutFactory : IDebugUIElementFactory
    {
        sealed class Builder : IDebugUIBuilder
        {
            readonly List<IDebugUIElementFactory> factories = new();
            readonly List<IDebugUIOptions> options = new();

            public ICollection<IDebugUIElementFactory> Factories => factories;
            public ICollection<IDebugUIOptions> Options => options;

            public VisualElement Build()
            {
                var root = new VisualElement();

                List<IDisposable> disposables = new();
                foreach (var factory in factories)
                {
                    root.Add(factory.CreateVisualElement(disposables));
                }

                root.RegisterCallback<DetachFromPanelEvent>(eventData =>
                {
                    foreach (var item in disposables) item.Dispose();
                    disposables.Clear();
                });

                return root;
            }
        }
        public string Name { get; set; }
        public string Label { get; set; }
        public Action<IDebugUIBuilder> Configure { get; set; }

        public VisualElement CreateVisualElement(ICollection<IDisposable> disposables)
        {
            var foldout = new Foldout()
            {
                text = Label
            };

            if (!string.IsNullOrEmpty(Name))
                foldout.name = Name;

            var builder = new Builder();
            Configure(builder);
            foldout.Add(builder.Build());

            foldout.value = false;

            return foldout;
        }
    }

    internal sealed class DebugEnumFieldFactory<TEnum> : IDebugUIElementFactory
        where TEnum : Enum
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public Func<TEnum> Getter { get; set; }
        public Action<TEnum> Setter { get; set; }

        public VisualElement CreateVisualElement(ICollection<IDisposable> disposables)
        {
            var field = new EnumField(Getter())
            {
                label = Label,
            };

            if (!string.IsNullOrEmpty(Name))
                field.name = Name;

            if (Setter == null)
            {
                field.SetEnabled(false);
            }
            else
            {
                field.RegisterValueChangedCallback(x => Setter((TEnum)x.newValue));
            }

            if (Getter != null)
            {
                MinimalRx.EveryValueChanged(this, x => x.Getter())
                    .Subscribe(x =>
                    {
                        field.value = x;
                    })
                    .AddTo(disposables);
            }

            return field;
        }
    }

    internal sealed class DebugFieldFactory<TValue, TField> : IDebugUIElementFactory
        where TField : BaseField<TValue>, new()
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public Func<TValue> Getter { get; set; }
        public Action<TValue> Setter { get; set; }

        public VisualElement CreateVisualElement(ICollection<IDisposable> disposables)
        {
            var field = new TField()
            {
                label = Label,
                value = Getter()
            };

            if (!string.IsNullOrEmpty(Name))
                field.name = Name;

            if (Setter == null)
            {
                field.SetEnabled(false);
            }
            else
            {
                field.RegisterValueChangedCallback(x => Setter(x.newValue));
            }

            if (Getter != null)
            {
                MinimalRx.EveryValueChanged(this, x => x.Getter())
                    .Subscribe(x =>
                    {
                        field.value = x;
                    })
                    .AddTo(disposables);
            }

            return field;
        }
    }

    internal sealed class DebugCompositeFieldFactory<TValue, TCompositeField, TField, TFieldValue> : IDebugUIElementFactory
        where TCompositeField : BaseCompositeField<TValue, TField, TFieldValue>, new()
        where TField : TextValueField<TFieldValue>, new()
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public Func<TValue> Getter { get; set; }
        public Action<TValue> Setter { get; set; }

        public VisualElement CreateVisualElement(ICollection<IDisposable> disposables)
        {
            var field = new TCompositeField()
            {
                label = Label,
                value = Getter()
            };

            if (!string.IsNullOrEmpty(Name))
                field.name = Name;

            if (Setter == null)
            {
                field.SetEnabled(false);
            }
            else
            {
                field.RegisterValueChangedCallback(x => Setter(x.newValue));
            }

            if (Getter != null)
            {
                MinimalRx.EveryValueChanged(this, x => x.Getter())
                    .Subscribe(x =>
                    {
                        field.value = x;
                    })
                    .AddTo(disposables);
            }

            return field;
        }
    }
    internal sealed class DebugStaticImagePreviewFactory : IDebugUIElementFactory
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public Background Background { get; set; }

        public VisualElement CreateVisualElement(ICollection<IDisposable> disposables)
        {
            var field = new PreviewField()
            {
                Text = Label,
                BackgroundImage = Background
            };

            if (!string.IsNullOrEmpty(Name))
                field.name = Name;

            return field;
        }
    }

    internal sealed class DebugTexture2DImagePreviewFactory : IDebugUIElementFactory
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public Func<Texture2D> Getter { get; set; }

        public VisualElement CreateVisualElement(ICollection<IDisposable> disposables)
        {
            var field = new PreviewField()
            {
                Text = Label,
                BackgroundImage = Getter()
            };

            if (!string.IsNullOrEmpty(Name))
                field.name = Name;

            MinimalRx.EveryValueChanged(this, x => x.Getter())
                .Subscribe(x =>
                {
                    field.BackgroundImage = x;
                })
                .AddTo(disposables);

            return field;
        }
    }

    internal sealed class DebugSpriteImagePreviewFactory : IDebugUIElementFactory
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public Func<Sprite> Getter { get; set; }

        public VisualElement CreateVisualElement(ICollection<IDisposable> disposables)
        {
            var field = new PreviewField()
            {
                Text = Label,
                BackgroundImage = Background.FromSprite(Getter())
            };

            if (!string.IsNullOrEmpty(Name))
                field.name = Name;

            MinimalRx.EveryValueChanged(this, x => x.Getter())
                .Subscribe(x =>
                {
                    field.BackgroundImage = Background.FromSprite(Getter());
                })
                .AddTo(disposables);

            return field;
        }
    }

    internal sealed class DebugRenderTextureImagePreviewFactory : IDebugUIElementFactory
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public Func<RenderTexture> Getter { get; set; }

        public VisualElement CreateVisualElement(ICollection<IDisposable> disposables)
        {
            var field = new PreviewField()
            {
                Text = Label,
                BackgroundImage = Background.FromRenderTexture(Getter())
            };

            if (!string.IsNullOrEmpty(Name))
                field.name = Name;

            MinimalRx.EveryValueChanged(this, x => x.Getter())
                .Subscribe(x =>
                {
                    field.BackgroundImage = Background.FromRenderTexture(Getter());
                })
                .AddTo(disposables);

            return field;
        }
    }

    internal sealed class DebugVectorImagePreviewFactory : IDebugUIElementFactory
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public Func<VectorImage> Getter { get; set; }

        public VisualElement CreateVisualElement(ICollection<IDisposable> disposables)
        {
            var field = new PreviewField()
            {
                Text = Label,
                BackgroundImage = Background.FromVectorImage(Getter())
            };

            if (!string.IsNullOrEmpty(Name))
                field.name = Name;

            MinimalRx.EveryValueChanged(this, x => x.Getter())
                .Subscribe(x =>
                {
                    field.BackgroundImage = Background.FromVectorImage(Getter());
                })
                .AddTo(disposables);

            return field;
        }
    }

    internal sealed class DebugTabFactory : IDebugUIElementFactory
    {
        sealed class TabBuilder : IDebugUIBuilder
        {
            readonly VisualElement root;
            readonly List<IDebugUIElementFactory> factories = new();
            readonly List<IDebugUIOptions> options = new();

            public ICollection<IDebugUIElementFactory> Factories => factories;
            public ICollection<IDebugUIOptions> Options => options;

            public TabBuilder(VisualElement root)
            {
                this.root = root;
            }

            public VisualElement Build()
            {
                List<IDisposable> disposables = new();
                foreach (var factory in factories)
                {
                    root.Add(factory.CreateVisualElement(disposables));
                }

                root.RegisterCallback<DetachFromPanelEvent>(eventData =>
                {
                    foreach (var item in disposables) item.Dispose();
                    disposables.Clear();
                });

                return root;
            }
        }
        public string Name { get; set; }
        public string Label { get; set; }
        public Action<IDebugUIBuilder> Configure { get; set; }

        public VisualElement CreateVisualElement(ICollection<IDisposable> disposables)
        {
            var tab = new Tab(Label);

            if (!string.IsNullOrEmpty(Name))
                tab.name = Name;

            tab.AddToClassList(UssClasses.debug_ui_tab);
            var builder = new TabBuilder(tab);
            Configure(builder);
            builder.Build();

            return tab;
        }
    }

    internal sealed class DebugTabViewFactory : IDebugUIElementFactory
    {
        sealed class TabViewBuilder : IDebugUIBuilder
        {
            readonly VisualElement root;
            readonly List<IDebugUIElementFactory> factories = new();
            readonly List<IDebugUIOptions> options = new();

            public ICollection<IDebugUIElementFactory> Factories => factories;
            public ICollection<IDebugUIOptions> Options => options;

            public TabViewBuilder(VisualElement root)
            {
                this.root = root;
            }

            public VisualElement Build()
            {
                List<IDisposable> disposables = new();
                foreach (var factory in factories)
                {
                    root.Add(factory.CreateVisualElement(disposables));
                }

                root.RegisterCallback<DetachFromPanelEvent>(eventData =>
                {
                    foreach (var item in disposables) item.Dispose();
                    disposables.Clear();
                });

                return root;
            }
        }

        public string Name { get; set; }
        public Action<IDebugUIBuilder> Configure { get; set; }

        public VisualElement CreateVisualElement(ICollection<IDisposable> disposables)
        {
            var tabView = new TabView();

            if (!string.IsNullOrEmpty(Name))
                tabView.name = Name;

            tabView.AddToClassList(UssClasses.debug_ui_tab_view);
            var builder = new TabViewBuilder(tabView);
            Configure(builder);
            builder.Build();

            return tabView;
        }
    }

    internal sealed class DebugListViewFactory : IDebugUIElementFactory
    {
        public string Name { get; set; }
        public float MaxHeight { get; set; }
        public IList Collection { get; set; }
        public Func<VisualElement> MakeItem { get; set; }
        public Action<VisualElement, int> BindItem { get; set; }
        public Action<VisualElement, int> UnbindItem { get; set; }

        public VisualElement CreateVisualElement(ICollection<IDisposable> disposables)
        {
            var listView = new ListView(Collection, -1, MakeItem, BindItem);
            listView.unbindItem = UnbindItem;
            listView.name = Name;
            listView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            var styleLength = MaxHeight == -1.0f ? new StyleLength(StyleKeyword.Auto) : new StyleLength(MaxHeight);
            listView.style.minHeight = styleLength;
            listView.style.maxHeight = styleLength;
            listView.focusable = false;
            listView.selectionType = SelectionType.None;
            return listView;
        }
    }

    internal sealed class SubmitableIntFieldFactory : IDebugUIElementFactory
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public Action<int> OnSubmit { get; set; }

        public VisualElement CreateVisualElement(ICollection<IDisposable> disposables)
        {
            var container = new VisualElement();

            if (!string.IsNullOrEmpty(Name))
                container.name = Name;

            container.AddToClassList(UssClasses.debug_ui_submitable_field_container);

            var label = new Label(Label);
            label.AddToClassList(UssClasses.debug_ui_submitable_field_label);
            container.Add(label);

            var intField = new IntegerField();
            intField.AddToClassList(UssClasses.debug_ui_submitable_field);
            container.Add(intField);

            var submitButton = new Button();
            submitButton.AddToClassList(UssClasses.debug_ui_submitable_field_button);
            submitButton.text = ">";
            container.Add(submitButton);

            submitButton.RegisterCallback<ClickEvent, IntegerField>((e, x) =>
            {
                OnSubmit(x.value);
            }, intField);

            return container;
        }
    }

    internal sealed class SubmitableFloatFieldFactory : IDebugUIElementFactory
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public Action<float> OnSubmit { get; set; }

        public VisualElement CreateVisualElement(ICollection<IDisposable> disposables)
        {
            var container = new VisualElement();

            if (!string.IsNullOrEmpty(Name))
                container.name = Name;

            container.AddToClassList(UssClasses.debug_ui_submitable_field_container);

            var label = new Label(Label);
            label.AddToClassList(UssClasses.debug_ui_submitable_field_label);
            container.Add(label);

            var floatField = new FloatField();
            floatField.AddToClassList(UssClasses.debug_ui_submitable_field);
            container.Add(floatField);

            var submitButton = new Button();
            submitButton.AddToClassList(UssClasses.debug_ui_submitable_field_button);
            submitButton.text = ">";
            container.Add(submitButton);

            submitButton.RegisterCallback<ClickEvent, FloatField>((e, x) =>
            {
                OnSubmit(x.value);
            }, floatField);

            return container;
        }
    }
}