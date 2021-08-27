using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TDGame.Internals.Common;

namespace TDGame.Internals.UI
{
    public class UIElement
    {
        public delegate void MouseEvent(UIElement affectedElement);

        public static List<UIElement> TotalElements { get; private set; } = new();

        internal List<UIElement> Children { get; private set; } = new();

        public UIElement Parent { get; private set; }

        /// <summary>
        /// If <c>InteractionBoxRelative</c> has any values set, changing this after constructing the <c>UIElement</c> will have no effect
        /// </summary>
        public OuRectangle InteractionBox;

        public OuRectangle InteractionBoxRelative;

        public bool MouseHovering;

        public bool Visible { get; set; } = true;

        public float Rotation { get; set; } = 0;

        public event MouseEvent OnClick;

        public event MouseEvent OnRightClick;

        public event MouseEvent OnMiddleClick;

        public event MouseEvent OnMouseLeftRelease;

        public event MouseEvent OnMouseRightRelease;

        public event MouseEvent OnMouseMiddleRelease;

        public event MouseEvent OnMouseOver;

        public event MouseEvent OnMouseLeave;

        private OuRectangle originalBox;

        internal UIElement() {
            TotalElements.Add(this);
        }

        public virtual void Draw() {
            if (!Visible)
                return;

            if (InteractionBoxRelative != default) {
                if (originalBox == null) {
                    originalBox = InteractionBox;
                }
                InteractionBox = originalBox;
                if (InteractionBoxRelative.X != default) {
                    InteractionBox.X += Utils.WindowTopLeft.X + (Utils.WindowWidth * InteractionBoxRelative.X);
                }
                if (InteractionBoxRelative.Y != default) {
                    InteractionBox.Y += Utils.WindowTopLeft.Y + (Utils.WindowHeight * InteractionBoxRelative.Y);
                }
                if (InteractionBoxRelative.Width != default) {
                    InteractionBox.Width += Utils.WindowWidth * InteractionBoxRelative.Width;
                }
                if (InteractionBoxRelative.Height != default) {
                    InteractionBox.Height += Utils.WindowHeight * InteractionBoxRelative.Height;
                }
            }

            DrawChildren();
        }

        public virtual void MouseClick() {
            OnClick?.Invoke(this);
        }

        public virtual void MouseRightClick() {
            OnRightClick?.Invoke(this);
        }

        public virtual void MouseLeftRelease() {
            OnMouseLeftRelease?.Invoke(this);
        }

        public virtual void MouseRightRelease() {
            OnMouseRightRelease?.Invoke(this);
        }

        public virtual void MouseMiddleRelease() {
            OnMouseMiddleRelease?.Invoke(this);
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

        public void RemoveAllElements() {
            foreach (var element in Children)
                element.Parent = null;
            Children.Clear();
        }

        internal void DrawChildren() {
            foreach (var element in Children)
                element?.Draw();
        }
    }
}