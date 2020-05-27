using Razorwing.Framework;
using Razorwing.Framework.Platform;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terraria.ModKit.Razorwing.Overrides
{
    public class ModStorage : DesktopStorage
    {
        public ModStorage(string baseName = "") : base(baseName)
        {

        }

        public override void OpenInNativeExplorer()
        {
            if (RuntimeInfo.OS ==  RuntimeInfo.Platform.Windows)
            {
                Process.Start("explorer.exe", GetFullPath(string.Empty));
            }
        }
    }
}
