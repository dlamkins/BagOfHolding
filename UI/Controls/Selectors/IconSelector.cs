using Blish_HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.WIC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BagOfHolding.UI.Controls.Selectors {
    internal class IconSelector : ItemSelector {

        private ModuleState _state;

        private int _index = 0;

        public IconSelector(ModuleState state) {
            _state = state;
            _index = _state.Settings.IconImage.Value;
        }

        public override void Increment() {
            if (++_index > _state.Textures.CornerIconOptions.Count() - 1) {
                _index = 0;
            }

            _state.Settings.IconImage.Value = _index;
        }

        public override void Decrement() {
            if (--_index < 0) {
                _index = _state.Textures.CornerIconOptions.Count() - 1;
            }

            _state.Settings.IconImage.Value = _index;
        }

        public override void Paint(SettingsItemSelector ctrl, SpriteBatch spriteBatch, Rectangle bounds) {
            spriteBatch.DrawOnCtrl(ctrl, ContentService.Textures.Pixel, bounds, Color.Black * 0.25f);

            var selIcon = _state.Textures.CornerIconOptions[_index].Icon;

            spriteBatch.DrawOnCtrl(ctrl, selIcon, new Rectangle(bounds.Left + bounds.Width / 4, bounds.Top + bounds.Height / 4, bounds.Width / 2, bounds.Height / 2));
        }

    }
}
