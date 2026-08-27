using UnityEngine.UIElements;

namespace DebugUI.UIElements
{
    [UxmlElement]
    public partial class PreviewField : VisualElement
    {
        public PreviewField()
        {
            label = new Label();
            Add(label);

            image = new VisualElement();
            image.AddToClassList(UssClasses.debug_ui_image_preview__image);
            Add(image);

            AddToClassList(UssClasses.debug_ui_image_preview);
        }

        public StyleBackground BackgroundImage
        {
            get => image.style.backgroundImage;
            set => image.style.backgroundImage = value;
        }

        public string Text
        {
            get => label.text;
            set => label.text = value;
        }

        readonly Label label;
        readonly VisualElement image;
    }
}