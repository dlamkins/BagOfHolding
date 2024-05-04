using BagOfHolding.UI.Controls;
using Blish_HUD.Controls;
using Blish_HUD.Settings;

namespace BagOfHolding {
    internal class ModuleState {

        public Yoinker Yoinker { get; set; }

        public Locker Locker { get; set; }

        public Settings Settings { get; set; }

        public Bag Bag { get; set; }

        public CornerIcon Icon { get; set; }

        public void Start() {
            this.Yoinker.Start();
            this.Locker.Start();
        }

        public void Update() {
            this.Yoinker.Update();
            this.Locker.Update();
        }

        public void Stop() {
            this.Yoinker.Stop();
            this.Locker.Stop();
        }

    }
}
