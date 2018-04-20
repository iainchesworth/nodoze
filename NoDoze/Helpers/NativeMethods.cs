using System;
using System.Runtime.InteropServices;

namespace NoDoze.Helpers
{
    internal static class NativeMethods
    {
        [Flags]
        public enum ExecutionState : uint
        {
            // EsAwayModeRequired = 0x00000040,
            EsContinuous = 0x80000000,
            EsDisplayRequired = 0x00000002,
            // EsSystemRequired = 0x00000001

            // Legacy flag, should not be used.
            // EsUserPresent = 0x00000004
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern ExecutionState SetThreadExecutionState(ExecutionState esFlags);

        public static void PreventSleep()
        {
            SetThreadExecutionState(ExecutionState.EsDisplayRequired | ExecutionState.EsContinuous);
        }

        public static void PermitSleep()
        {
            SetThreadExecutionState(ExecutionState.EsContinuous);
        }
    }
}