using UnityEngine;
using UnityEngine.UIElements;

namespace DebugUI.UIElements
{
    [UxmlElement]
    public partial class DebugWindow : VisualElement
    {
        public override VisualElement contentContainer => scrollView.contentContainer;

        string text = "Debug";

        [UxmlAttribute]
        public string Text
        {
            get => text;
            set
            {
                this.text = value;
                label.text = value;
            }
        }

        public void SetDraggable(bool draggable)
        {
            if (dragManipulator != null) this.RemoveManipulator(dragManipulator);
            if (draggable)
            {
                dragManipulator = new DebugWindowDragManipulator(this, label);
                label.AddManipulator(dragManipulator);
            }
        }

        public VisualElement BackgroundElement => background;

        readonly Label label;
        readonly ScrollView scrollView;
        readonly VisualElement background;

        DebugWindowDragManipulator dragManipulator;

        public DebugWindow()
        {
            AddToClassList(UssClasses.debug_ui_window);

            background = new VisualElement();
            background.AddToClassList(UssClasses.debug_ui_window_background);
            hierarchy.Add(background);

            var topbar = new VisualElement();
            topbar.AddToClassList(UssClasses.debug_ui_topbar);

            label = new Label(text);
            label.style.flexGrow = 1;
            topbar.Add(label);

            var buttonContainer = new VisualElement();
            buttonContainer.AddToClassList(UssClasses.debug_ui_topbar_button_container);

            var toggleButton = new Button();
            toggleButton.text = "-";
            toggleButton.AddToClassList(UssClasses.debug_ui_topbar_button);
            toggleButton.RegisterCallback<ClickEvent>((e) =>
            {
                scrollView.style.display = scrollView.style.display == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None;
            });
            buttonContainer.Add(toggleButton);

            var closeButton = new Button();
            closeButton.text = "x";
            closeButton.AddToClassList(UssClasses.debug_ui_topbar_button);
            closeButton.RegisterCallback<ClickEvent>((e) =>
            {
                style.display = DisplayStyle.None;
            });
            buttonContainer.Add(closeButton);

            topbar.Add(buttonContainer);
            background.Add(topbar);

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

            background.Add(scrollView);

            schedule.Execute(() =>
            {
                style.translate = new Translate(
                    Mathf.Clamp(resolvedStyle.translate.x, parent.contentRect.xMin, parent.contentRect.xMax - label.contentRect.width),
                    Mathf.Clamp(resolvedStyle.translate.y, parent.contentRect.yMin, parent.contentRect.yMax - label.contentRect.height)
                );
            })
            .Every(1);
        }
    }
}