using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Blish_HUD.Input;
using BagOfHolding.UI.Controls;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace BagOfHolding {
    internal class Yoinker {

        private readonly ModuleState _state;

        private List<int> _ignoreList = new List<int>() {
            2147483647, // Blish HUD icon
        };

        public CornerIcon ActiveIcon { get; private set; } = null;

        private Point _iconOffset = Point.Zero;
        private FakeIcon _fakeIcon = null;

        public Yoinker(ModuleState state) {
            _state = state;
        }

        public void Start() {
            _fakeIcon = new FakeIcon() {
                Parent = GameService.Graphics.SpriteScreen,
                Visible = false
            };

            GameService.Input.Mouse.LeftMouseButtonPressed += GlobalLeftMouseButtonPressed;
            GameService.Input.Mouse.LeftMouseButtonReleased += GlobalLeftMouseButtonReleased;
        }

        private void GlobalLeftMouseButtonReleased(object sender, MouseEventArgs e) {
            if (this.ActiveIcon != null) {
                if (_state.Bag.AbsoluteBounds.Contains(GameService.Input.Mouse.Position)) {
                    _state.Locker.LockUp(this.ActiveIcon);
                } else {
                    _state.Locker.Release(this.ActiveIcon);
                    this.ActiveIcon.Show();
                }
            }
            this.ActiveIcon = null;
        }

        private void GlobalLeftMouseButtonPressed(object sender, MouseEventArgs e) {
            CornerIcon icon = null;

            if (GameService.Input.Mouse.ActiveControl == _state.Bag) {
                icon = _state.Bag.GetIconFromRelativePosition().Icon;
                _iconOffset = new Point(icon.Size.X / 2, icon.Size.Y / 2);
            } else if (GameService.Input.Mouse.ActiveControl is CornerIcon ci) {
                icon = ci;
                _iconOffset = icon.RelativeMousePosition;
            }

            if (icon != null) {
                if (_ignoreList.Contains(icon.Priority) || icon == _state.Icon) {
                    return;
                }

                // SHIFT toggle
                //if (_state.Settings.HoldShiftToToggleCapture.Value && GameService.Input.Keyboard.ActiveModifiers.HasFlag(ModifierKeys.Shift)) {
                //    if (icon.Visible) {
                //        _state.Locker.LockUp(icon);
                //    } else {
                //        _state.Locker.Release(icon);
                //    }
                //    return;
                //}

                // Normal yoink
                this.ActiveIcon = icon;

                _fakeIcon.Impersonate(this.ActiveIcon);
            } else if (GameService.Input.Mouse.ActiveControl != _state.Bag) {
                _state.Bag.Hide();
            }
        }

        public void Update() {
            if (this.ActiveIcon != null) {
                if (GameService.Input.Mouse.Position.Y > this.ActiveIcon.Bottom + _iconOffset.Y 
                    || (!_state.OneOff.NexusShimIsRunning && GameService.Input.Mouse.Position.X < this.ActiveIcon.Left)
                    || (!_state.OneOff.NexusShimIsRunning && GameService.Input.Mouse.Position.X > this.ActiveIcon.Right)) {
                    this.ActiveIcon.Visible = false;
                    _fakeIcon.Visible = true;
                    _fakeIcon.Location = GameService.Input.Mouse.Position - _iconOffset;

                    if (!_state.Bag.Visible) {
                        _state.Bag.Show();
                    }
                } else {
                    this.ActiveIcon.Visible = true;
                    _fakeIcon.Visible = false;
                }
            } else {
                _fakeIcon.Visible = false; ;
            }
        }

        public void Stop() {
            _fakeIcon?.Dispose();

            this.ActiveIcon = null;

            GameService.Input.Mouse.LeftMouseButtonPressed -= GlobalLeftMouseButtonPressed;
            GameService.Input.Mouse.LeftMouseButtonReleased -= GlobalLeftMouseButtonReleased;
        }

    }
}
