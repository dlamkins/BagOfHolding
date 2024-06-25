using BagOfHolding.UI.Controls.Selectors;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagOfHolding.UI.Controls {
    internal class SettingsItemSelector : Control {

        private const int ITEM_LENGTH  = 64;
        private const int ITEM_PADDING = 16;

        private AsyncTexture2D _leftArrow  = AsyncTexture2D.FromAssetId(255387);
        private AsyncTexture2D _rightArrow = AsyncTexture2D.FromAssetId(255391);

        private Rectangle _leftArrowBounds  = Rectangle.Empty;
        private Rectangle _rightArrowBounds = Rectangle.Empty;

        private Rectangle _itemBounds = Rectangle.Empty;

        private Rectangle _textBounds = Rectangle.Empty;

        public string Text { get; set; }

        private ItemSelector _selector;

        public SettingsItemSelector(ItemSelector selector) {
            _selector = selector;
        }

        public override void RecalculateLayout() {
            base.RecalculateLayout();

            _leftArrowBounds = new Rectangle((int)(this.Width / 2 - (ITEM_LENGTH * 1.5) - ITEM_PADDING), this.Height / 2 - (ITEM_LENGTH / 2), ITEM_LENGTH, ITEM_LENGTH);
            _rightArrowBounds = new Rectangle((int)(this.Width / 2 + (ITEM_LENGTH * 1) + ITEM_PADDING), this.Height / 2 - (ITEM_LENGTH / 2), ITEM_LENGTH, ITEM_LENGTH);

            _itemBounds = new Rectangle(this.Width / 2 - ITEM_LENGTH / 2, this.Height / 2 - ITEM_LENGTH / 2, ITEM_LENGTH, ITEM_LENGTH);

            _textBounds = new Rectangle(_leftArrowBounds.Left, _leftArrowBounds.Top - ITEM_LENGTH - ITEM_PADDING, _rightArrowBounds.Left - _leftArrowBounds.Left + ITEM_LENGTH / 2, ITEM_LENGTH);
        }

        protected override void OnClick(MouseEventArgs e) {
            if (_leftArrowBounds.Contains(this.RelativeMousePosition)) {
                _selector.Decrement();
            } else if (_rightArrowBounds.Contains(this.RelativeMousePosition)) {
                _selector.Increment();
            }

            base.OnClick(e);
        }

        protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds) {
            bool leftOver = _leftArrowBounds.Contains(this.RelativeMousePosition);
            bool rightOver = _rightArrowBounds.Contains(this.RelativeMousePosition);

            spriteBatch.DrawStringOnCtrl(this, this.Text, GameService.Content.DefaultFont18, _textBounds, Color.White, false, HorizontalAlignment.Center, VerticalAlignment.Bottom);

            spriteBatch.DrawOnCtrl(this, _leftArrow, _leftArrowBounds, Color.White * (leftOver ? 1f : 0.5f));
            spriteBatch.DrawOnCtrl(this, _rightArrow, _rightArrowBounds, Color.White * (rightOver ? 1f : 0.5f));

            _selector.Paint(this, spriteBatch, _itemBounds);
        }

    }

}
