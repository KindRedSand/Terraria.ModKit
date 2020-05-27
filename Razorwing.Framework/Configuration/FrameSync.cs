using System.ComponentModel;

namespace Razorwing.Framework.Configuration
{
    public enum FrameSync
    {
        VSync,
        [Description("2x refresh rate")]
        Limit2x,
        [Description("4x refresh rate")]
        Limit4x,
        [Description("8x refresh rate")]
        Limit8x,
        [Description("Unlimited")]
        Unlimited,
    }
}
