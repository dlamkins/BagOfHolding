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
    internal class AlignmentSelector : ItemSelector {

        private ModuleState _state;

        public AlignmentSelector(ModuleState state) {
            _state = state;
        }

        public override void Increment() {
            _state.Settings.IconHugsLeftSide.Value = !_state.Settings.IconHugsLeftSide.Value;
        }

        public override void Decrement() {
            Increment();
        }

        public override void Paint(SettingsItemSelector ctrl, SpriteBatch spriteBatch, Rectangle bounds) {
            spriteBatch.DrawOnCtrl(ctrl, ContentService.Textures.Pixel, bounds, Color.Black * 0.25f);

            spriteBatch.DrawStringOnCtrl(ctrl, _state.Settings.IconHugsLeftSide.Value ? "L" : "R", GameService.Content.DefaultFont18, bounds, Color.White, false, Blish_HUD.Controls.HorizontalAlignment.Center);
        }

    }
}
