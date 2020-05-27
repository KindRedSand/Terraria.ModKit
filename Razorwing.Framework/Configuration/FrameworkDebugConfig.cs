using System.Runtime;
using Razorwing.Framework.Caching;

namespace Razorwing.Framework.Configuration
{
    public class FrameworkDebugConfigManager : IniConfigManager<DebugSetting>
    {
        protected override string Filename => null;

        public FrameworkDebugConfigManager()
            : base(null)
        {
        }

        protected override void InitialiseDefaults()
        {
            base.InitialiseDefaults();

            Set(DebugSetting.ActiveGCMode, GCLatencyMode.SustainedLowLatency);
            Set(DebugSetting.BypassCaching, false).ValueChanged += delegate { StaticCached.BypassCache = Get<bool>(DebugSetting.BypassCaching); };
        }
    }

    public enum DebugSetting
    {
        ActiveGCMode,
        BypassCaching
    }
}
