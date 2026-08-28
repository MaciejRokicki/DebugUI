using UnityEngine;
using UnityEngine.UIElements;

namespace DebugUI
{
    internal sealed class DebugWindowDragManipulator : MouseManipulator
    {
        readonly VisualElement moveTarget;
        readonly VisualElement rectTarget;
        Vector2 targetStartPosition;
        Vector3 pointerStartPosition;
        bool enabled;

        public DebugWindowDragManipulator(VisualElement moveTarget, VisualElement rectTarget)
        {
            this.moveTarget = moveTarget;
            this.rectTarget = rectTarget;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerDownEvent>(PointerDownHandler, TrickleDown.TrickleDown);
            target.RegisterCallback<PointerMoveEvent>(PointerMoveHandler, TrickleDown.TrickleDown);
            target.RegisterCallback<PointerUpEvent>(PointerUpHandler, TrickleDown.TrickleDown);
            target.RegisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler, TrickleDown.TrickleDown);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerDownEvent>(PointerDownHandler, TrickleDown.TrickleDown);
            target.UnregisterCallback<PointerMoveEvent>(PointerMoveHandler, TrickleDown.TrickleDown);
            target.UnregisterCallback<PointerUpEvent>(PointerUpHandler, TrickleDown.TrickleDown);
            target.UnregisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler, TrickleDown.TrickleDown);
        }

        void PointerDownHandler(PointerDownEvent e)
        {
            targetStartPosition = moveTarget.resolvedStyle.translate;
            pointerStartPosition = e.position;
            target.CapturePointer(e.pointerId);
            enabled = true;
        }

        void PointerMoveHandler(PointerMoveEvent e)
        {
            if (enabled && target.HasPointerCapture(e.pointerId))
            {
                var pointerDelta = e.position - pointerStartPosition;
                moveTarget.style.translate = new Translate(
                    Mathf.Clamp(targetStartPosition.x + pointerDelta.x, moveTarget.parent.contentRect.xMin, moveTarget.parent.contentRect.xMax - rectTarget.contentRect.width),
                    Mathf.Clamp(targetStartPosition.y + pointerDelta.y, moveTarget.parent.contentRect.yMin, moveTarget.parent.contentRect.yMax - rectTarget.contentRect.height)
                );
            }
        }

        void PointerUpHandler(PointerUpEvent e)
        {
            if (enabled && target.HasPointerCapture(e.pointerId))
            {
                target.ReleasePointer(e.pointerId);
            }
        }

        void PointerCaptureOutHandler(PointerCaptureOutEvent e)
        {
            enabled = false;
        }
    }
}