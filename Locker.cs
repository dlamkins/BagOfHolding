using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BagOfHolding {
    internal class Locker {

        public const int LOCKED_PRIORITY = int.MinValue + 1;

        private readonly ModuleState _state;

        private SettingEntry<HashSet<int>> _lockedUp;

        public List<(WeakReference<CornerIcon> Icon, int Priority)> Icons { get; private set; } = new List<(WeakReference<CornerIcon> Icon, int Priority)>();

        public Locker(ModuleState state) {
            _state = state;
        }

        public void Start() {
            _lockedUp = _state.Settings.SettingsRoot.DefineSetting("_lockedUp", new HashSet<int>());
        }

        public void LockUp(CornerIcon icon) {
            if (icon == null) return;
            if (icon.Priority == LOCKED_PRIORITY) return;

            _lockedUp.Value.Add(icon.Priority);
            GameService.Settings.Save();

            this.Icons.Add((new WeakReference<CornerIcon>(icon), icon.Priority));

            icon.Hide();
            icon.Priority = LOCKED_PRIORITY;

            _state.Bag.UpdateSizeAndLocation();
        }

        public void Release(CornerIcon icon) {
            if (icon == null) return;

            foreach (var cell in this.Icons) {
                if (cell.Icon.TryGetTarget(out var cellIcon)) {
                    if (cellIcon == icon) {
                        cellIcon.Priority = cell.Priority;
                        cellIcon.Show();

                        this.Icons.Remove(cell);

                        _lockedUp.Value.Remove(icon.Priority);
                        GameService.Settings.Save();
                        break;
                    }
                }
            }

            _state.Bag.UpdateSizeAndLocation();
        }

        public void Update() {
            // Find any that should be in the bag, but aren't.
            try {
                foreach (var controls in GameService.Graphics.SpriteScreen.Children) {
                    if (controls is CornerIcon icon) {
                        if (_lockedUp.Value.Contains(icon.Priority)) {
                            LockUp(icon);
                        }
                    }
                }
            } catch (InvalidOperationException) { /* NOOP - it's not 100% safe to enumerate the children. */ }

            // Throw out the dead ones.
            foreach (var cell in this.Icons.ToArray()) {
                if (!cell.Icon.TryGetTarget(out var icon) || icon.Parent == null) {
                    this.Icons.Remove(cell);
                } else {
                    // Ensure it stays invisible.
                    icon.Hide();
                }
            }

            // Sort what remains.
            this.Icons = this.Icons.OrderByDescending(l => l.Priority).ToList();
        }

        public void Stop() {
            foreach (var cell in this.Icons) {
                if (cell.Icon.TryGetTarget(out var cellIcon)) {
                    cellIcon.Priority = cell.Priority;
                    cellIcon.Show();
                }
            }

            this.Icons.Clear();
        }

    }
}
