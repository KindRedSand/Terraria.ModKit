using System;
using System.Runtime.InteropServices;

namespace Razorwing.Framework.Platform.Windows.Native
{
    internal static class Execution
    {
        [DllImport("kernel32.dll")]
        internal static extern uint SetThreadExecutionState(ExecutionState state);

        [Flags]
        internal enum ExecutionState : uint
        {
            AwaymodeRequired = 0x00000040,
            Continuous = 0x80000000,
            DisplayRequired = 0x00000002,
            SystemRequired = 0x00000001,
            UserPresent = 0x00000004,
        }
    }
}
