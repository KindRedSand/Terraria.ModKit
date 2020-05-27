using Microsoft.Xna.Framework.Input;
using Razorwing.Framework.Configuration;
using Razorwing.Framework.Platform;

namespace Terraria.ModKit
{
    public class CreativeInputConfig : IniConfigManager<InputConfig>
    {
        public CreativeInputConfig(Storage storage) : base(storage) {}

        protected override string Filename => @"CreativeInput.ini";

        protected override void InitialiseDefaults()
        {
            Set(InputConfig.CycleMode, Keys.NumPad0);
            Set(InputConfig.FlyMode, Keys.NumPad4);
            Set(InputConfig.UnlockAllItems, Keys.NumPad5);
            Set(InputConfig.UnlockAllBestiary, Keys.NumPad3);
            Set(InputConfig.LockBestiary, Keys.NumPad7);
            Set(InputConfig.IncreaseFlySpeed, Keys.OemOpenBrackets);
            Set(InputConfig.DecreaseFlySpeed, Keys.OemCloseBrackets);
            Set(InputConfig.InstantRespawn, Keys.NumPad9);
        }
    }

    public enum InputConfig
    {
        CycleMode,
        FlyMode,
        UnlockAllItems,
        UnlockAllBestiary,
        LockBestiary,
        IncreaseFlySpeed,
        DecreaseFlySpeed,
        InstantRespawn,
    }
}