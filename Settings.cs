using Blish_HUD.Settings;

namespace BagOfHolding {
    internal class Settings {

        private readonly ModuleState _state;

        public SettingCollection SettingsRoot { get; }

        public SettingEntry<bool> IconHugsLeftSide { get; private set; }

        //public SettingEntry<bool> HoldShiftToToggleCapture { get; private set; }

        public Settings(ModuleState state, SettingCollection root) {
            _state = state;

            this.SettingsRoot = root;

            DefineSettings(root);
        }

        private void DefineSettings(SettingCollection settings) {
            this.IconHugsLeftSide = settings.DefineSetting(nameof(IconHugsLeftSide), true, () => "Keep icon to the left", () => "If checked, the Bag of Holding icon is placed on the far left.  If unchecked, the Bag of Holding icon is placed on the far right.");
            //this.HoldShiftToToggleCapture = settings.DefineSetting(nameof(HoldShiftToToggleCapture), false, () => "Shift key modifier", () => "If checked, icons can be placed into (or removed from) the Bag of Holding by holding SHIFT and clicking an icon.");
        }

    }
}
