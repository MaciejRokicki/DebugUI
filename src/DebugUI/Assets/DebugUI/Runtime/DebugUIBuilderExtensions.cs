using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace DebugUI
{
    public static class DebugUIBuilderExtensions
    {
        public static IDebugUIBuilder ConfigureWindowOptions(this IDebugUIBuilder builder, Action<DebugWindowOptions> configure)
        {
            var options = builder.Options.OfType<DebugWindowOptions>().FirstOrDefault();
            if (options == null)
            {
                options = new DebugWindowOptions();
                builder.Options.Add(options);
            }
            configure(options);
            return builder;
        }

        public static IDebugUIBuilder AddSpace(this IDebugUIBuilder builder, float height, string name = null)
        {
            builder.Factories.Add(new DebugSpaceFactory()
            {
                Name = name,
                Height = height
            });
            return builder;
        }

        public static IDebugUIBuilder AddLabel(this IDebugUIBuilder builder, string text, string name = null)
        {
            builder.Factories.Add(new DebugLabelFactory()
            {
                Name = name,
                Text = text,
            });
            return builder;
        }

        public static IDebugUIBuilder AddButton(this IDebugUIBuilder builder, string text, Action action, string name = null)
        {
            builder.Factories.Add(new DebugButtonFactory()
            {
                Name = name,
                Text = text,
                Action = action
            });
            return builder;
        }

        public static IDebugUIBuilder AddField<TEnum>(this IDebugUIBuilder builder, string label, Func<TEnum> getter = null, string name = null)
            where TEnum : Enum
        {
            builder.Factories.Add(new DebugEnumFieldFactory<TEnum>()
            {
                Name = name,
                Label = label,
                Getter = getter,
            });
            return builder;
        }


        public static IDebugUIBuilder AddField<TEnum>(this IDebugUIBuilder builder, string label, Func<TEnum> getter = null, Action<TEnum> setter = null, string name = null)
            where TEnum : Enum
        {
            builder.Factories.Add(new DebugEnumFieldFactory<TEnum>()
            {
                Name = name,
                Label = label,
                Getter = getter,
                Setter = setter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddField(this IDebugUIBuilder builder, string label, Func<bool> getter = null, string name = null)
        {
            builder.Factories.Add(new DebugFieldFactory<bool, Toggle>()
            {
                Name = name,
                Label = label,
                Getter = getter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddField(this IDebugUIBuilder builder, string label, Func<bool> getter = null, Action<bool> setter = null, string name = null)
        {
            builder.Factories.Add(new DebugFieldFactory<bool, Toggle>()
            {
                Name = name,
                Label = label,
                Getter = getter,
                Setter = setter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddField(this IDebugUIBuilder builder, string label, Func<int> getter = null, string name = null)
        {
            builder.Factories.Add(new DebugFieldFactory<int, IntegerField>()
            {
                Name = name,
                Label = label,
                Getter = getter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddField(this IDebugUIBuilder builder, string label, Func<int> getter = null, Action<int> setter = null, string name = null)
        {
            builder.Factories.Add(new DebugFieldFactory<int, IntegerField>()
            {
                Name = name,
                Label = label,
                Getter = getter,
                Setter = setter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddField(this IDebugUIBuilder builder, string label, Func<float> getter = null, string name = null)
        {
            builder.Factories.Add(new DebugFieldFactory<float, FloatField>()
            {
                Name = name,
                Label = label,
                Getter = getter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddField(this IDebugUIBuilder builder, string label, Func<float> getter = null, Action<float> setter = null, string name = null)
        {
            builder.Factories.Add(new DebugFieldFactory<float, FloatField>()
            {
                Name = name,
                Label = label,
                Getter = getter,
                Setter = setter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddField(this IDebugUIBuilder builder, string label, Func<string> getter = null, string name = null)
        {
            builder.Factories.Add(new DebugFieldFactory<string, TextField>()
            {
                Name = name,
                Label = label,
                Getter = getter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddField(this IDebugUIBuilder builder, string label, Func<string> getter = null, Action<string> setter = null, string name = null)
        {
            builder.Factories.Add(new DebugFieldFactory<string, TextField>()
            {
                Name = name,
                Label = label,
                Getter = getter,
                Setter = setter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddField(this IDebugUIBuilder builder, string label, Func<Vector2> getter = null, string name = null)
        {
            builder.Factories.Add(new DebugCompositeFieldFactory<Vector2, Vector2Field, FloatField, float>()
            {
                Name = name,
                Label = label,
                Getter = getter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddField(this IDebugUIBuilder builder, string label, Func<Vector2> getter = null, Action<Vector2> setter = null, string name = null)
        {
            builder.Factories.Add(new DebugCompositeFieldFactory<Vector2, Vector2Field, FloatField, float>()
            {
                Name = name,
                Label = label,
                Getter = getter,
                Setter = setter
            });
            return builder;
        }

        public static IDebugUIBuilder AddField(this IDebugUIBuilder builder, string label, Func<Vector3> getter = null, string name = null)
        {
            builder.Factories.Add(new DebugCompositeFieldFactory<Vector3, Vector3Field, FloatField, float>()
            {
                Name = name,
                Label = label,
                Getter = getter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddField(this IDebugUIBuilder builder, string label, Func<Vector3> getter = null, Action<Vector3> setter = null, string name = null)
        {
            builder.Factories.Add(new DebugCompositeFieldFactory<Vector3, Vector3Field, FloatField, float>()
            {
                Name = name,
                Label = label,
                Getter = getter,
                Setter = setter
            });
            return builder;
        }

        public static IDebugUIBuilder AddField(this IDebugUIBuilder builder, string label, Func<Vector4> getter = null, string name = null)
        {
            builder.Factories.Add(new DebugCompositeFieldFactory<Vector4, Vector4Field, FloatField, float>()
            {
                Name = name,
                Label = label,
                Getter = getter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddField(this IDebugUIBuilder builder, string label, Func<Vector4> getter = null, Action<Vector4> setter = null, string name = null)
        {
            builder.Factories.Add(new DebugCompositeFieldFactory<Vector4, Vector4Field, FloatField, float>()
            {
                Name = name,
                Label = label,
                Getter = getter,
                Setter = setter
            });
            return builder;
        }

        public static IDebugUIBuilder AddField(this IDebugUIBuilder builder, string label, Func<Vector2Int> getter = null, string name = null)
        {
            builder.Factories.Add(new DebugCompositeFieldFactory<Vector2Int, Vector2IntField, IntegerField, int>()
            {
                Name = name,
                Label = label,
                Getter = getter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddField(this IDebugUIBuilder builder, string label, Func<Vector2Int> getter = null, Action<Vector2Int> setter = null, string name = null)
        {
            builder.Factories.Add(new DebugCompositeFieldFactory<Vector2Int, Vector2IntField, IntegerField, int>()
            {
                Name = name,
                Label = label,
                Getter = getter,
                Setter = setter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddField(this IDebugUIBuilder builder, string label, Func<Vector3Int> getter = null, string name = null)
        {
            builder.Factories.Add(new DebugCompositeFieldFactory<Vector3Int, Vector3IntField, IntegerField, int>()
            {
                Name = name,
                Label = label,
                Getter = getter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddField(this IDebugUIBuilder builder, string label, Func<Vector3Int> getter = null, Action<Vector3Int> setter = null, string name = null)
        {
            builder.Factories.Add(new DebugCompositeFieldFactory<Vector3Int, Vector3IntField, IntegerField, int>()
            {
                Name = name,
                Label = label,
                Getter = getter,
                Setter = setter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddField(this IDebugUIBuilder builder, string label, Func<Rect> getter = null, string name = null)
        {
            builder.Factories.Add(new DebugCompositeFieldFactory<Rect, RectField, FloatField, float>()
            {
                Name = name,
                Label = label,
                Getter = getter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddField(this IDebugUIBuilder builder, string label, Func<Rect> getter = null, Action<Rect> setter = null, string name = null)
        {
            builder.Factories.Add(new DebugCompositeFieldFactory<Rect, RectField, FloatField, float>()
            {
                Name = name,
                Label = label,
                Getter = getter,
                Setter = setter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddField(this IDebugUIBuilder builder, string label, Func<RectInt> getter = null, Action<RectInt> setter = null, string name = null)
        {
            builder.Factories.Add(new DebugCompositeFieldFactory<RectInt, RectIntField, IntegerField, int>()
            {
                Name = name,
                Label = label,
                Getter = getter,
                Setter = setter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddField(this IDebugUIBuilder builder, string label, Func<RectInt> getter = null, string name = null)
        {
            builder.Factories.Add(new DebugCompositeFieldFactory<RectInt, RectIntField, IntegerField, int>()
            {
                Name = name,
                Label = label,
                Getter = getter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddField(this IDebugUIBuilder builder, string label, Func<Bounds> getter = null, Action<Bounds> setter = null, string name = null)
        {
            builder.Factories.Add(new DebugFieldFactory<Bounds, BoundsField>()
            {
                Name = name,
                Label = label,
                Getter = getter,
                Setter = setter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddField(this IDebugUIBuilder builder, string label, Func<Bounds> getter = null, string name = null)
        {
            builder.Factories.Add(new DebugFieldFactory<Bounds, BoundsField>()
            {
                Name = name,
                Label = label,
                Getter = getter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddField(this IDebugUIBuilder builder, string label, Func<BoundsInt> getter = null, Action<BoundsInt> setter = null, string name = null)
        {
            builder.Factories.Add(new DebugFieldFactory<BoundsInt, BoundsIntField>()
            {
                Name = name,
                Label = label,
                Getter = getter,
                Setter = setter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddField(this IDebugUIBuilder builder, string label, Func<BoundsInt> getter = null, string name = null)
        {
            builder.Factories.Add(new DebugFieldFactory<BoundsInt, BoundsIntField>()
            {
                Name = name,
                Label = label,
                Getter = getter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddSlider(this IDebugUIBuilder builder, string label, float lowValue, float highValue, Func<float> getter = null, string format = "{0:F2}", string name = null)
        {
            builder.Factories.Add(new DebugSliderFactory()
            {
                Name = name,
                Label = label,
                Format = format,
                LowValue = lowValue,
                HighValue = highValue,
                Getter = getter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddSlider(this IDebugUIBuilder builder, string label, float lowValue, float highValue, Func<float> getter = null, Action<float> setter = null, string format = "{0:F2}", string name = null)
        {
            builder.Factories.Add(new DebugSliderFactory()
            {
                Name = name,
                Label = label,
                Format = format,
                LowValue = lowValue,
                HighValue = highValue,
                Getter = getter,
                Setter = setter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddSlider(this IDebugUIBuilder builder, string label, int lowValue, int highValue, Func<int> getter = null, Action<int> setter = null, string format = "{0}", string name = null)
        {
            builder.Factories.Add(new DebugSliderIntFactory()
            {
                Name = name,
                Label = label,
                Format = format,
                LowValue = lowValue,
                HighValue = highValue,
                Getter = getter,
                Setter = setter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddSlider(this IDebugUIBuilder builder, string label, int lowValue, int highValue, Func<int> getter = null, string format = "{0}", string name = null)
        {
            builder.Factories.Add(new DebugSliderIntFactory()
            {
                Name = name,
                Label = label,
                Format = format,
                LowValue = lowValue,
                HighValue = highValue,
                Getter = getter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddProgressBar(this IDebugUIBuilder builder, string label, float lowValue, float highValue, Func<float> getter = null, string format = "[{0:F2}]", string name = null)
        {
            builder.Factories.Add(new DebugProgressBarFactory()
            {
                Name = name,
                Label = label,
                Format = format,
                LowValue = lowValue,
                HighValue = highValue,
                Getter = getter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddProgressBar(this IDebugUIBuilder builder, string label, int lowValue, int highValue, Func<int> getter = null, string format = "[{0}]", string name = null)
        {
            builder.Factories.Add(new DebugProgressBarFactory()
            {
                Name = name,
                Label = label,
                Format = format,
                LowValue = lowValue,
                HighValue = highValue,
                Getter = () => getter(),
            });
            return builder;
        }

        public static IDebugUIBuilder AddImage(this IDebugUIBuilder builder, string label, Texture2D texture2D, string name = null)
        {
            builder.Factories.Add(new DebugStaticImagePreviewFactory()
            {
                Name = name,
                Label = label,
                Background = texture2D
            });
            return builder;
        }

        public static IDebugUIBuilder AddImage(this IDebugUIBuilder builder, string label, Sprite sprite, string name = null)
        {
            builder.Factories.Add(new DebugStaticImagePreviewFactory()
            {
                Name = name,
                Label = label,
                Background = Background.FromSprite(sprite)
            });
            return builder;
        }

        public static IDebugUIBuilder AddImage(this IDebugUIBuilder builder, string label, RenderTexture renderTexture, string name = null)
        {
            builder.Factories.Add(new DebugStaticImagePreviewFactory()
            {
                Name = name,
                Label = label,
                Background = Background.FromRenderTexture(renderTexture)
            });
            return builder;
        }

        public static IDebugUIBuilder AddImage(this IDebugUIBuilder builder, string label, VectorImage vectorImage, string name = null)
        {
            builder.Factories.Add(new DebugStaticImagePreviewFactory()
            {
                Name = name,
                Label = label,
                Background = Background.FromVectorImage(vectorImage)
            });
            return builder;
        }

        public static IDebugUIBuilder AddImage(this IDebugUIBuilder builder, string label, Func<Texture2D> getter = null, string name = null)
        {
            builder.Factories.Add(new DebugTexture2DImagePreviewFactory()
            {
                Name = name,
                Label = label,
                Getter = getter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddImage(this IDebugUIBuilder builder, string label, Func<Sprite> getter = null, string name = null)
        {
            builder.Factories.Add(new DebugSpriteImagePreviewFactory()
            {
                Name = name,
                Label = label,
                Getter = getter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddImage(this IDebugUIBuilder builder, string label, Func<RenderTexture> getter = null, string name = null)
        {
            builder.Factories.Add(new DebugRenderTextureImagePreviewFactory()
            {
                Name = name,
                Label = label,
                Getter = getter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddImage(this IDebugUIBuilder builder, string label, Func<VectorImage> getter = null, string name = null)
        {
            builder.Factories.Add(new DebugVectorImagePreviewFactory()
            {
                Name = name,
                Label = label,
                Getter = getter,
            });
            return builder;
        }

        public static IDebugUIBuilder AddFoldout(this IDebugUIBuilder builder, string label, Action<IDebugUIBuilder> configure, string name = null)
        {
            builder.Factories.Add(new DebugFoldoutFactory()
            {
                Name = name,
                Label = label,
                Configure = configure,
            });
            return builder;
        }

        public static IDebugUIBuilder AddTab(this IDebugUIBuilder builder, string label, Action<IDebugUIBuilder> configure, string name = null)
        {
            builder.Factories.Add(new DebugTabFactory()
            {
                Name = name,
                Label = label,
                Configure = configure,
            });
            return builder;
        }

        public static IDebugUIBuilder AddTabView(this IDebugUIBuilder builder, Action<IDebugUIBuilder> configure, string name = null)
        {
            builder.Factories.Add(new DebugTabViewFactory()
            {
                Name = name,
                Configure = configure,
            });
            return builder;
        }

        public static IDebugUIBuilder AddListView(this IDebugUIBuilder builder, string name, IList collection, Func<VisualElement> makeItem, Action<VisualElement, int> bindItem)
        {
            builder.Factories.Add(new DebugListViewFactory()
            {
                Name = name,
                MaxHeight = -1.0f,
                Collection = collection,
                MakeItem = makeItem,
                BindItem = bindItem
            });
            return builder;
        }

        public static IDebugUIBuilder AddListView(this IDebugUIBuilder builder, string name, float maxHeight, IList collection, Func<VisualElement> makeItem, Action<VisualElement, int> bindItem, Action<VisualElement, int> unbindItem)
        {
            builder.Factories.Add(new DebugListViewFactory()
            {
                Name = name,
                MaxHeight = maxHeight,
                Collection = collection,
                MakeItem = makeItem,
                BindItem = bindItem,
                UnbindItem = unbindItem
            });
            return builder;
        }

        public static IDebugUIBuilder AddSubmitableIntField(this IDebugUIBuilder builder, string label, int defaultValue, Action<int> onSubmit, string name = null)
        {
            builder.Factories.Add(new SubmitableIntFieldFactory()
            {
                Name = name,
                DefaultValue = defaultValue,
                Label = label,
                OnSubmit = onSubmit
            });
            return builder;
        }

        public static IDebugUIBuilder AddSubmitableFloatField(this IDebugUIBuilder builder, string label, float defaultValue, Action<float> onSubmit, string name = null)
        {
            builder.Factories.Add(new SubmitableFloatFieldFactory()
            {
                Name = name,
                DefaultValue = defaultValue,
                Label = label,
                OnSubmit = onSubmit
            });
            return builder;
        }

        public static void BuildWith(this IDebugUIBuilder builder, VisualElement root)
        {
            root.Add(builder.Build());
        }
    }
}