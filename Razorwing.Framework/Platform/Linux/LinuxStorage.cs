using System;
using System.IO;

namespace Razorwing.Framework.Platform.Linux
{
    public class LinuxStorage : DesktopStorage
    {
        public LinuxStorage(string baseName)
            : base(baseName)
        {
        }

        protected override string LocateBasePath()
        {
            string home = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string xdg = Environment.GetEnvironmentVariable("XDG_DATA_HOME");
            string[] paths =
            {
                xdg ?? Path.Combine(home, ".local", "share"),
                Path.Combine(home)
            };

            foreach (string path in paths)
            {
                if (Directory.Exists(path))
                    return path;
            }

            return paths[0];
        }
    }
}
