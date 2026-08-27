using UnityEngine;
using UnityEngine.UIElements;

namespace DebugUI.UIElements
{
    [UxmlElement]
    public partial class DebugWindow : VisualElement
    {
        public override VisualElement contentContainer => scrollView.contentContainer;

        string text = "Debug";
        bool value;

        [UxmlAttribute]
        public bool Value
        {
            get => value;
            set
            {
                if (this.value == value) return;

                using var evt = ChangeEvent<bool>.GetPooled(this.value, value);
                evt.target = this;
                SetValueWithoutNotify(value);
                SendEvent(evt);
            }
        }

        [UxmlAttribute]
        public string Text
        {
            get => text;
            set
            {
                this.text = value;
                foldout.text = value;
            }
        }

        public void SetValueWithoutNotify(bool newValue)
        {
            value = newValue;
            foldout.value = value;
        }

        public void SetDraggable(bool draggable)
        {
            if (dragManipulator != null) this.RemoveManipulator(dragManipulator);
            if (draggable)
            {
                var toggle = foldout.Q<Toggle>();
                dragManipulator = new DebugWindowDragManipulator(this, toggle, toggle);
                toggle.AddManipulator(dragManipulator);
            }
        }

        public VisualElement BackgroundElement => background;

        readonly Foldout foldout;
        readonly ScrollView scrollView;
        readonly VisualElement background;

        DebugWindowDragManipulator dragManipulator;

        public DebugWindow()
        {
            AddToClassList(UssClasses.debug_ui_window);

            background = new VisualElement();
            background.AddToClassList(UssClasses.debug_ui_window_background);
            hierarchy.Add(background);

            foldout = new Foldout()
            {
                value = value,
                text = text
            };
            foldout.RegisterValueChangedCallback((evt) =>
            {
                if (evt.currentTarget == evt.target)
                {
                    Value = foldout.value;
                    evt.StopPropagation();
                }
            });

            background.Add(foldout);

            scrollView = new(ScrollViewMode.VerticalAndHorizontal);

            static void InitScroller(Scroller scroller)
            {
                scroller.AddToClassList(UssClasses.debug_ui_scroller);
                scroller.slider.AddToClassList(UssClasses.debug_ui_scroller__slider);
                scroller.Remove(scroller.highButton);
                scroller.Remove(scroller.lowButton);
                scroller.Q("unity-tracker").AddToClassList(UssClasses.debug_ui_scroller__tracker);
                scroller.Q("unity-dragger").AddToClassList(UssClasses.debug_ui_scroller__dragger);
            }

            InitScroller(scrollView.verticalScroller);
            InitScroller(scrollView.horizontalScroller);
            scrollView.verticalScroller.AddToClassList(UssClasses.debug_ui_scroller_vertical);
            scrollView.horizontalScroller.AddToClassList(UssClasses.debug_ui_scroller_horizontal);

            scrollView.contentViewport.style.flexGrow = 0f;

            foldout.Add(scrollView);

            var toggle = foldout.Q<Toggle>();
            schedule.Execute(() =>
            {
                style.translate = new Translate(
                    Mathf.Clamp(resolvedStyle.translate.x, parent.contentRect.xMin, parent.contentRect.xMax - toggle.contentRect.width),
                    Mathf.Clamp(resolvedStyle.translate.y, parent.contentRect.yMin, parent.contentRect.yMax - toggle.contentRect.height)
                );
            })
            .Every(1);
        }
    }
}