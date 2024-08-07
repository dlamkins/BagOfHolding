﻿using BagOfHolding.UI.Controls;
using Blish_HUD;
using Blish_HUD.Modules;
using Microsoft.Xna.Framework;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Content;
using Blish_HUD.Input;
using Blish_HUD.Graphics.UI;
using BagOfHolding.UI.Views;
using BagOfHolding.UI;

namespace BagOfHolding {
    [Export(typeof(Module))]
    public class BagOfHolding : Module {

        private ModuleState _state;

        [ImportingConstructor]
        public BagOfHolding([Import("ModuleParameters")] ModuleParameters moduleParameters) : base(moduleParameters) { /* NOOP */ }

        protected override async Task LoadAsync() {
            _state = new ModuleState();

            _state.Yoinker = new Yoinker(_state);
            _state.Locker = new Locker(_state);
            _state.Settings = new Settings(_state, ModuleParameters.SettingsManager.ModuleSettings);
            _state.OneOff = new OneOff(_state);
            _state.Textures = new Textures();
            _state.Bag = new Bag(_state) {
                Size = new Point(256, 256),
                Location = new Point(256, 256),
                Parent = GameService.Graphics.SpriteScreen,
                Visible = false,
            };
            _state.Icon = new CornerIcon(AsyncTexture2D.FromAssetId(502087)/*AsyncTexture2D.FromAssetId(156670), AsyncTexture2D.FromAssetId(156671)*/, "Bag of Holding") {
                Priority = int.MaxValue - 1,
                Parent = GameService.Graphics.SpriteScreen
            };

            _state.Icon.Click += ShowBagOfHolding;
        }

        private void ShowBagOfHolding(object sender, MouseEventArgs e) {
            if (_state.Bag.Visible) {
                _state.Bag.Hide();
            } else {
                _state.Bag.Show();
            }
        }

        protected override void OnModuleLoaded(EventArgs e) {
            base.OnModuleLoaded(e);

            _state.Start();
        }

        protected override void Update(GameTime gameTime) {
            _state.Update();

            UpdateAlignment();
            UpdateIcon();
        }

        private void UpdateAlignment() {
            _state.Icon.Priority = _state.Settings.IconHugsLeftSide.Value ? int.MaxValue - 1 : int.MinValue + 3;
        }

        private void UpdateIcon() {
            if (_state.Settings.IconImage.Value < 0 || _state.Settings.IconImage.Value >= _state.Textures.CornerIconOptions.Count) {
                _state.Settings.IconImage.Value = 0;
            }

            var (icon, hover) = _state.Textures.CornerIconOptions[_state.Settings.IconImage.Value];
            _state.Icon.Icon = icon;
            _state.Icon.HoverIcon = hover;
        }

        public override IView GetSettingsView() {
            return new SettingsView(_state);
        }

        protected override void Unload() {
            _state?.Bag?.Dispose();
            _state?.Icon?.Dispose();
            _state?.Stop();
        }
    }
}
