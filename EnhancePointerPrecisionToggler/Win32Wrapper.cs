using System;
using System.Runtime.InteropServices;

namespace EnhancePointerPrecisionToggler
{
    public static class Win32Wrapper
    {
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo", SetLastError = true)]
        public static extern bool SystemParametersInfoGet(uint action, uint param, IntPtr vparam, Spif fWinIni);

        public const UInt32 SpiGetmouse = 0x0003;

        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo", SetLastError = true)]
        public static extern bool SystemParametersInfoSet(uint action, uint param, IntPtr vparam, Spif fWinIni);

        public const UInt32 SpiSetmouse = 0x0004;

        public static bool IsToggled
        {
            get
            {
                int[] mouseParams = new int[3];
                SystemParametersInfoGet(SpiGetmouse, 0,
                    GCHandle.Alloc(mouseParams, GCHandleType.Pinned).AddrOfPinnedObject(), 0);
                return mouseParams[2] == 1;
            }
            set
            {
                int[] mouseParams = new int[3];
                SystemParametersInfoGet(SpiGetmouse, 0,
                    GCHandle.Alloc(mouseParams, GCHandleType.Pinned).AddrOfPinnedObject(), 0);
                mouseParams[2] = value ? 1 : 0;
                SystemParametersInfoSet(SpiSetmouse, 0,
                    GCHandle.Alloc(mouseParams, GCHandleType.Pinned).AddrOfPinnedObject(), Spif.SendChange);
            }
        }

        private static bool ToggleEnhancePointerPrecision(bool b)
        {
            //https://stackoverflow.com/questions/24737775/toggle-enhance-pointer-precision
            int[] mouseParams = new int[3];
            // Get the current values.
            SystemParametersInfoGet(SpiGetmouse, 0,
                GCHandle.Alloc(mouseParams, GCHandleType.Pinned).AddrOfPinnedObject(), 0);
            // Modify the acceleration value as directed.
            mouseParams[2] = b ? 1 : 0;
            // Update the system setting.
            return SystemParametersInfoSet(SpiSetmouse, 0,
                GCHandle.Alloc(mouseParams, GCHandleType.Pinned).AddrOfPinnedObject(), Spif.SendChange);
        }
    }

    [Flags]
    public enum Spif
    {
        None = 0x00,

        /// <summary>Writes the new system-wide parameter setting to the user profile.</summary>
        UpdateIniFile = 0x01,

        /// <summary>Broadcasts the WM_SETTINGCHANGE message after updating the user profile.</summary>
        SendChange = 0x02,

        /// <summary>Same as SendChange.</summary>
        SendWinIniChange = 0x02
    }
}