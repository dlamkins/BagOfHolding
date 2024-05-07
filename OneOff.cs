
using Blish_HUD;

namespace BagOfHolding {
    internal class OneOff {

        public bool NexusShimIsRunning { get; private set; }

        private readonly ModuleState _state;

        public OneOff(ModuleState state) {
            _state = state;
        }

        public void Update() {
            foreach (var module in GameService.Module.Modules) {
                if (module.Enabled && module.Manifest.Namespace == "fs.nexusshim") {
                    this.NexusShimIsRunning = true;
                    return;
                }
            }

            this.NexusShimIsRunning = false;
        }

    }
}
