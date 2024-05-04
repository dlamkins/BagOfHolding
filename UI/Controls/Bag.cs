using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace BagOfHolding.UI.Controls {
    internal class Bag : Control {

        const int ICON_SIZE = 32;
        const int CELL_SIZE = 32;

        const int MAX_COLUMNS = 5;

        private ModuleState _state;

        private int _activeRow = -1;
        private int _activeCol = -1;

        public Bag(ModuleState state) {
            _state = state;
        }

        protected override void OnShown(EventArgs e) {
            base.OnShown(e);

            UpdateSizeAndLocation();
        }

        public void UpdateSizeAndLocation() {
            this.Size = new Point(CELL_SIZE * MAX_COLUMNS, CELL_SIZE * Math.Max(_state.Locker.Icons.Count / MAX_COLUMNS + 1, 3));
            this.Location = new Point(_state.Icon.Left + (_state.Icon.Width / 2) - this.Width / 2, _state.Icon.Bottom + CELL_SIZE / 4);
        }

        public override void DoUpdate(GameTime gameTime) {
            base.DoUpdate(gameTime);

            if (this.MouseOver) {
                _activeCol = this.RelativeMousePosition.X / CELL_SIZE;
                _activeRow = this.RelativeMousePosition.Y / CELL_SIZE;
            } else {
                _activeRow = -1;
                _activeCol = -1;
            }
        }

        private CornerIcon IconFromCell(int col, int row) {
            int pos = row * MAX_COLUMNS + col;

            if (pos <= _state.Locker.Icons.Count && _state.Locker.Icons[pos].Icon.TryGetTarget(out var icon)) {
                return icon;
            }

            return null;
        }

        public (CornerIcon Icon, Point Location) GetIconFromRelativePosition() {
            int row = 0;
            int col = 0;
            foreach (var cell in _state.Locker.Icons) {
                if (cell.Icon.TryGetTarget(out var icon)) {
                    if (new Rectangle(col * CELL_SIZE, row * CELL_SIZE, CELL_SIZE, CELL_SIZE).Contains(this.RelativeMousePosition)) {
                        return (icon, new Point(col * CELL_SIZE, row * CELL_SIZE));
                    }
                } else {
                    continue;
                }

                if ((++col + 1) * CELL_SIZE > this.Width) {
                    col = 0;
                    row++;
                }
            }

            return (null, Point.Zero);
        }

        #region "Icon Proxying"

        protected override void OnMouseMoved(MouseEventArgs e) {
            base.OnMouseMoved(e);

            var activeCell = GetIconFromRelativePosition();

            if (activeCell.Icon != null) {
                if (this.Tooltip != activeCell.Icon.Tooltip) {
                    this.Tooltip?.Hide();
                }
                this.Tooltip = activeCell.Icon.Tooltip;
            } else {
                this.Tooltip?.Hide();
                this.Tooltip = null;
            }
        }

        protected override void OnLeftMouseButtonPressed(MouseEventArgs e) {
            base.OnLeftMouseButtonPressed(e);

            ProxyInput(MouseEventType.LeftMouseButtonPressed);
        }

        protected override void OnLeftMouseButtonReleased(MouseEventArgs e) {
            base.OnLeftMouseButtonReleased(e);

            ProxyInput(MouseEventType.LeftMouseButtonReleased);
        }

        protected override void OnRightMouseButtonPressed(MouseEventArgs e) {
            base.OnRightMouseButtonPressed(e);

            ProxyInput(MouseEventType.RightMouseButtonPressed);
        }

        protected override void OnRightMouseButtonReleased(MouseEventArgs e) {
            base.OnRightMouseButtonReleased(e);

            ProxyInput(MouseEventType.RightMouseButtonReleased);
        }

        private void ProxyInput(MouseEventType eventType) {
            //if (_state.Settings.HoldShiftToToggleCapture.Value && GameService.Input.Keyboard.ActiveModifiers.HasFlag(ModifierKeys.Shift)) return;

            var activeCell = GetIconFromRelativePosition();

            if (activeCell.Icon != null) {
                activeCell.Icon.Location = activeCell.Location + this.Location;
                activeCell.Icon.TriggerMouseInput(eventType, GameService.Input.Mouse.State);
            }
        }

        #endregion

        protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds) {
            spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, bounds, Color.Black * 0.5f);

            int row = 0;
            int col = 0;
            foreach (var cell in _state.Locker.Icons) {
                if (cell.Icon.TryGetTarget(out var icon)) {
                    DrawIcon(spriteBatch, 
                            icon,
                            row == _activeRow && col == _activeCol,
                            col * CELL_SIZE, 
                            row * CELL_SIZE, 
                            CELL_SIZE, 
                            CELL_SIZE);
                } else {
                    continue;
                }

                if ((++col + 1) * CELL_SIZE > this.Width) {
                    col = 0;
                    row++;
                }
            }

            if (_state.Locker.Icons.Count == 0 || (_state.Yoinker.ActiveIcon != null && !this.MouseOver)) {
                spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, bounds, Color.Black * 0.5f);
                spriteBatch.DrawStringOnCtrl(this, "Drop Icon Here", GameService.Content.DefaultFont18, bounds, Color.White, false, HorizontalAlignment.Center, VerticalAlignment.Middle);
            }
        }

        private void DrawIcon(SpriteBatch spriteBatch, CornerIcon icon, bool hovered, int x, int y, int width, int height) {
            int offset = width / 2 - ICON_SIZE / 2;

            if (hovered && icon.Enabled) {
                spriteBatch.DrawOnCtrl(this, icon.HoverIcon ?? icon.Icon, new Rectangle(x + offset, y + offset, ICON_SIZE, ICON_SIZE));
            } else {
                spriteBatch.DrawOnCtrl(this, icon.Icon, new Rectangle(x + offset, y + offset, ICON_SIZE, ICON_SIZE), Color.White * 0.65f);
            }
        }
    }
}
