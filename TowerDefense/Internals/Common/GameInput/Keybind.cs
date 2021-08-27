using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TDGame.GameContent;

namespace TDGame.Internals.Common.GameInput
{
    public class Keybind
    {
        public static List<Keybind> AllKeybinds { get; internal set; } = new();

        public Keybind(string name, Keys defaultKey = Keys.None) {
            Name = name;
            AssignedKey = defaultKey;
            AllKeybinds.Add(this);
            TowerDefense.Instance.Window.KeyDown += Window_KeyDown;
        }

        public bool JustReassigned
        {
            get; private set;
        }

        public bool IsPressed => Input.CurrentKeySnapshot.IsKeyDown(AssignedKey) && !IsReassignPending;
        public bool IsReassignPending
        {
            get; private set;
        }

        public Keys AssignedKey { get; internal set; } = Keys.None;
        public string Name { get; set; } = "Unnamed";

        public Action<Keybind> KeybindPressAction { get; set; } = null;

        public bool PendKeyReassign() {
            static bool isOtherBindAssigning() {
                int reassignCounts = 0;
                for (int i = 0; i < AllKeybinds.Count; i++) {
                    var kBind = AllKeybinds[i];

                    if (kBind.IsReassignPending)
                        reassignCounts++;

                    if (reassignCounts >= 1)
                        return true;
                }
                return false;
            }
            if (!(IsReassignPending || isOtherBindAssigning())) {
                Console.WriteLine($"Reassigning '{Name}'... (Current: {AssignedKey})\nPress {AssignedKey} to stop binding, press Escape to unbind.");
                IsReassignPending = true;
            }
            else
                Console.WriteLine($"Tried reassigning '{Name}' but cannot.");
            return true;
        }

        private void Window_KeyDown(object sender, Microsoft.Xna.Framework.InputKeyEventArgs e) {
            TowerDefense.BaseLogger.Write(e.Key == AssignedKey);
            if (e.Key == AssignedKey)
                KeybindPressAction?.Invoke(this);

            if (IsReassignPending) {
                if (e.Key == AssignedKey) {
                    Console.WriteLine($"Cancelled keybind assign for '{Name}'");
                }
                else if (e.Key == Keys.Escape) {
                    Console.WriteLine($"Unassigned '{Name}'");
                    AssignedKey = Keys.None;
                }
                else {
                    Console.WriteLine($"Keybind of name '{Name}' key assigned from {AssignedKey} to '{e.Key.ParseKey()}'");
                    AssignedKey = e.Key;
                    JustReassigned = true;
                }
                IsReassignPending = false;
            }
        }

        public override string ToString() {
            return $"{Name} = {{ Key: {AssignedKey.ParseKey()} | Pressed: {IsPressed} | ReassignPending: {IsReassignPending} }}";
        }
    }
}