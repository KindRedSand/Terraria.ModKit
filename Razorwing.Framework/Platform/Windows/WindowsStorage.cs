using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Razorwing.Framework.Platform.Windows
{
    public class WindowsStorage : DesktopStorage
    {
        public WindowsStorage(string baseName)
            : base(baseName)
        {
            // allows traversal of long directory/filenames beyond the standard limitations (see https://stackoverflow.com/a/5188559)
            BasePath = Regex.Replace(BasePath, @"^([a-zA-Z]):\\", @"\\?\$1:\");
        }

        public override void OpenInNativeExplorer() => Process.Start("explorer.exe", GetFullPath(string.Empty));

        protected override string LocateBasePath() => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    }
}
