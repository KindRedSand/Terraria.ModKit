using System;
using System.Runtime.InteropServices;

namespace Razorwing.Framework
{
    public static class RuntimeInfo
    {
        [DllImport(@"kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        internal static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport(@"kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        /// <summary>
        /// Returns the absolute path of Razorwing.Framework.dll.
        /// </summary>
        public static string GetFrameworkAssemblyPath() =>
            System.Reflection.Assembly.GetAssembly(typeof(RuntimeInfo)).Location;

        public static bool Is32Bit { get; }
        public static bool Is64Bit { get; }
        public static Platform OS { get; }
        public static bool IsUnix => OS == Platform.Linux || OS == Platform.MacOsx;
        public static bool IsWine { get; }

        static RuntimeInfo()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                OS = Platform.Windows;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                OS = OS == 0 ? Platform.MacOsx : throw new InvalidOperationException($"Tried to set OS Platform to {nameof(Platform.MacOsx)}, but is already {Enum.GetName(typeof(Platform), OS)}");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                OS = OS == 0 ? Platform.Linux : throw new InvalidOperationException($"Tried to set OS Platform to {nameof(Platform.Linux)}, but is already {Enum.GetName(typeof(Platform), OS)}");

            if (OS == 0)
                throw new PlatformNotSupportedException("Operating system could not be detected correctly.");

            Is32Bit = IntPtr.Size == 4;
            Is64Bit = IntPtr.Size == 8;

            if (OS == Platform.Windows)
            {
                IntPtr hModule = GetModuleHandle(@"ntdll.dll");
                if (hModule == IntPtr.Zero)
                    IsWine = false;
                else
                {
                    IntPtr fptr = GetProcAddress(hModule, @"wine_get_version");
                    IsWine = fptr != IntPtr.Zero;
                }
            }
        }

        public enum Platform
        {
            Windows = 1,
            Linux = 2,
            MacOsx = 3,
        }
    }
}
