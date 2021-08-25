using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TDGame.Internals.UI
{
    public class UIElement
    {
        public delegate void MouseEvent(UIElement affectedElement);

        public static List<UIElement> TotalElements { get; private set; } = new();

        internal List<UIElement> Children { get; private set; } = new();

        public UIElement Parent { get; set; }

        public OuRectangle InteractionBox;

        public OuRectangle InteractionBoxRelative;

        public bool MouseHovering;

        public bool Visible { get; set; } = true;

        public float Rotation { get; set; } = 0;

        public event MouseEvent OnClick;

        public event MouseEvent OnRightClick;

        public event MouseEvent OnMiddleClick;

        public event MouseEvent OnMouseOver;

        public event MouseEvent OnMouseLeave;

        internal UIElement() {
            TotalElements.Add(this);
        }

        public virtual void Draw() {
            if (!Visible)
                return;

            DrawChildren();
        }

        public virtual void MouseClick() {
            OnClick?.Invoke(this);
        }

        public virtual void MouseRightClick() {
            OnRightClick?.Invoke(this);
        }

        public virtual void MouseMiddleClick() {
            OnMiddleClick?.Invoke(this);
        }

        public virtual void MouseOver() {
            OnMouseOver?.Invoke(this);
            MouseHovering = true;
        }

        public virtual void MouseLeave() {
            OnMouseLeave?.Invoke(this);
            MouseHovering = false;
        }

        public void AppendElement(UIElement element) {
            Children.Add(element);
            element.Parent = this;
        }

        public void RemoveElement(UIElement element) {
            Children.Remove(element);
            element.Parent = null;
        }

        internal void DrawChildren() {
            foreach (var element in Children)
                element?.Draw();
        }
    }
}