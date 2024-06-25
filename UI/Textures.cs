using Blish_HUD.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagOfHolding.UI {
    internal class Textures {

        public List<(AsyncTexture2D Icon, AsyncTexture2D Hover)> CornerIconOptions = new List<(AsyncTexture2D Icon, AsyncTexture2D Hover)>() {
            (AsyncTexture2D.FromAssetId(502087), null),
            (AsyncTexture2D.FromAssetId(156670), AsyncTexture2D.FromAssetId(156671)),
        };

        public void Unload() {
            foreach (var texture in CornerIconOptions) {
                texture.Icon?.Dispose();
                texture.Hover?.Dispose();
            }
        }

    }
}
