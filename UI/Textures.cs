using Blish_HUD.Content;
using System.Collections.Generic;

namespace BagOfHolding.UI {
    internal class Textures {

        public List<(AsyncTexture2D Icon, AsyncTexture2D Hover)> CornerIconOptions = new List<(AsyncTexture2D Icon, AsyncTexture2D Hover)>() {
            (AsyncTexture2D.FromAssetId(502087), null),
            (AsyncTexture2D.FromAssetId(156670), AsyncTexture2D.FromAssetId(156671)),
        };

    }
}
