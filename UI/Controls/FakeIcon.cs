using Blish_HUD.Controls;

namespace BagOfHolding.UI.Controls {
    internal class FakeIcon : Image {

        public FakeIcon() {
            this.ZIndex = int.MaxValue - 1;
        }

        protected override CaptureType CapturesInput() {
            return CaptureType.DoNotBlock;
        }

        public void Impersonate(CornerIcon icon) {
            this.Texture = icon.Icon;
            this.Size = icon.Size;
        }

    }
}
