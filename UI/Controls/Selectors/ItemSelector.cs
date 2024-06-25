using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BagOfHolding.UI.Controls.Selectors {
    internal abstract class ItemSelector {

        public abstract void Increment();

        public abstract void Decrement();

        public abstract void Paint(SettingsItemSelector ctrl, SpriteBatch spriteBatch, Rectangle bounds);

    }
}
