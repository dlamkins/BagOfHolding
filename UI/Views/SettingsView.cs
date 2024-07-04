using BagOfHolding.UI.Controls;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using BagOfHolding.UI.Controls.Selectors;

namespace BagOfHolding.UI.Views {
    internal class SettingsView : View {

        private readonly ModuleState _state;

        public SettingsView(ModuleState state) {
            _state = state;
        }

        protected override void Build(Container buildPanel) {
            _ = new SettingsItemSelector(new IconSelector(_state)) {
                Text = "Choose Icon",
                Width = buildPanel.Width / 2,
                Height = buildPanel.Height,
                Parent = buildPanel
            };

            _ = new SettingsItemSelector(new AlignmentSelector(_state)) {
                Text = "Icon Alignment",
                BasicTooltipText = "'L' to keep the Bag of Holding icon placed to the left of all icons.\n'R' to keep the Bag of Holding icon placed to the right of all icons.",
                Left = buildPanel.Width / 2,
                Width = buildPanel.Width / 2,
                Height = buildPanel.Height,
                Parent = buildPanel
            };
        }

    }
}
