using System.Runtime.InteropServices;
using System.Text;

namespace Gmcu.Managed.Io.Battery
{
    internal static class BatteryInfo
    {
        #region Enums

        [Flags]
        private enum BATTERY_INFORMATION_CAPABILITY_FLAGS : uint
        {
            BATTERY_CAPACITY_RELATIVE = 0x40000000,
            BATTERY_IS_SHORT_TERM = 0x20000000,
            BATTERY_SET_CHARGE_SUPPORTED = 0x00000001,
            BATTERY_SET_DISCHARGE_SUPPORTED = 0x00000002,
            BATTERY_SYSTEM_BATTERY = 0x80000000,
        }

        #endregion

        #region Structures

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto,Pack =1)]
        struct BATTERY_INFORMATION
        {
            public uint Capabilities;
            public byte Technology;
            private byte Reserved1;
            private byte Reserved2;
            private byte Reserved3;
            public byte Chemistry1;
            public byte Chemistry2;
            public byte Chemistry3;
            public byte Chemistry4;
            public uint DesignedCapacity;
            public uint FullChargedCapacity;
            public uint DefaultAlert1;
            public uint DefaultAlert2;
            public uint CriticalBias;
            public uint CycleCount;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
        struct BATTERY_STATUS
        {
            public uint PowerState;
            public uint Capacity;
            public uint Voltage;
            public int Rate;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
        private struct BATTERY_MANUFACTURE_DATE
        {
            public byte Day;
            public byte Month;
            public ushort Year;
        }

        private struct SYSTEM_POWER_STATUS
        {
            public byte ACLineStatus;
            public byte BatteryFlag;
            public byte BatteryLifePercent;
            public byte SystemStatusFlag;
            public int BatteryLifeTime;
            public int BatteryFullLifeTime;
        };

        #endregion

        #region Dll Imports

        [DllImport("Gmcu.Proxy.Io.Battery.dll", SetLastError = true)]
        static extern BatteryResult GetBatteryInformation(int index, ref BATTERY_INFORMATION batteryInformation);

        [DllImport("Gmcu.Proxy.Io.Battery.dll", SetLastError = true)]
        static extern BatteryResult GetManufactureDate(int index, ref BATTERY_MANUFACTURE_DATE batteryManufactureDate);

        [DllImport("Gmcu.Proxy.Io.Battery.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        static extern BatteryResult GetManufactureName(int batteryIndex, StringBuilder name, int nameLength);

        [DllImport("Gmcu.Proxy.Io.Battery.dll", SetLastError = true)]
        static extern int GetTemperature(int batteryIndex);

        [DllImport("Gmcu.Proxy.Io.Battery.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        static extern BatteryResult GetSerialNumber(int batteryIndex, StringBuilder serialNumber, int serialNumberLength);

        [DllImport("Gmcu.Proxy.Io.Battery.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        static extern BatteryResult GetUniqueId(int batteryIndex, StringBuilder uniqueId, int uniqueIdLength);

        [DllImport("Gmcu.Proxy.Io.Battery.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        static extern BatteryResult GetDeviceName(int batteryIndex, StringBuilder deviceName, int deviceNameLength);

        [DllImport("Gmcu.Proxy.Io.Battery.dll", SetLastError = true)]
        static extern int GetEstimatedTime(int batteryIndex);

        [DllImport("Gmcu.Proxy.Io.Battery.dll", SetLastError = true)]
        static extern int GetStatus(int batteryIndex, ref BATTERY_STATUS status);

        [DllImport("Gmcu.Proxy.Io.Battery.dll", SetLastError = true)]
        static extern int GetSystemPowerState(ref SYSTEM_POWER_STATUS status);

        #endregion

        private static void throwExceptionForError(BatteryResult result)
        {
            throw new BatteryInfoException(result);
        }

        public static BatteryStatus QueryBatteryStatus(int index)
        {
            var query_status= new BATTERY_STATUS();
            int result = GetStatus(index, ref query_status);
            if( result < 0 )
            {
                throwExceptionForError((BatteryResult)result);
            }
            return new BatteryStatus {  
                Capacity = query_status.Capacity, 
                DischargeRate = query_status.Rate, 
                PowerState = (BatteryPowerState)query_status.PowerState, 
                Voltage = query_status.Voltage };
        }

        public static float QueryTemperature(int index)
        {
            var result = GetTemperature(index);
            if( result < 0)
            {
                throwExceptionForError((BatteryResult)result);
            }
            return result / 10.0f;
        }

        public static TimeSpan QueryEstimatedTime(int index)
        {
            var result = GetEstimatedTime(index);
            if( result < 0 )
            {
                if ( result == -110 )
                {
                    return TimeSpan.Zero;
                }
                throwExceptionForError((BatteryResult)result);
            }
            return TimeSpan.FromSeconds(result);
        }

        public static string QueryDeviceName(int index)
        {
            StringBuilder sb = new StringBuilder(512);
            var result = GetDeviceName(index, sb, sb.Capacity + 1);
            if (result != BatteryResult.Success)
            {
                throwExceptionForError(result);
            }

            return sb.ToString();
        }

        public static string QueryManufactureName(int index)
        {
            StringBuilder sb = new StringBuilder(512);
            var result = GetManufactureName(index, sb, sb.Capacity+1);
            if( result != BatteryResult.Success )
            {
                throwExceptionForError(result);
            }

            return sb.ToString();
        }

        public static string QuerySerialNumber(int index)
        {
            StringBuilder sb = new StringBuilder(512);
            var result = GetSerialNumber(index, sb, sb.Capacity + 1);
            if (result != BatteryResult.Success)
            {
                throwExceptionForError(result);
            }

            return sb.ToString();
        }

        public static string QueryUniqueId(int index)
        {
            StringBuilder sb = new StringBuilder(512);
            var result = GetUniqueId(index, sb, sb.Capacity + 1);
            if (result != BatteryResult.Success)
            {
                throwExceptionForError(result);
            }

            return sb.ToString();
        }

        public static DateTime QueryManufactureDate(int index)
        {
            var bmd = new BATTERY_MANUFACTURE_DATE();
            var result = GetManufactureDate(index, ref bmd);
            if (result == BatteryResult.Success)
            {
                return new DateTime((int)bmd.Year, (int)bmd.Month, (int)bmd.Day);
            }
            throwExceptionForError(result);
            return DateTime.MinValue;
        }


        public static BatteryInformation QueryBatteryInformation(int index)
        {
            var bi = new BATTERY_INFORMATION();
            var result = GetBatteryInformation(index, ref bi);

            if (result == BatteryResult.Success)
            {
                var bicf = (BATTERY_INFORMATION_CAPABILITY_FLAGS)bi.Capabilities;

                var info = new BatteryInformation();
                info.IsCapacityRelative = (bicf & BATTERY_INFORMATION_CAPABILITY_FLAGS.BATTERY_CAPACITY_RELATIVE) == BATTERY_INFORMATION_CAPABILITY_FLAGS.BATTERY_CAPACITY_RELATIVE;
                info.IsSystemBattery = (bicf & BATTERY_INFORMATION_CAPABILITY_FLAGS.BATTERY_SYSTEM_BATTERY) == BATTERY_INFORMATION_CAPABILITY_FLAGS.BATTERY_SYSTEM_BATTERY;
                info.IsShortTerm = (bicf & BATTERY_INFORMATION_CAPABILITY_FLAGS.BATTERY_IS_SHORT_TERM) == BATTERY_INFORMATION_CAPABILITY_FLAGS.BATTERY_IS_SHORT_TERM;
                info.CycleCount = bi.CycleCount;
                info.DefaultAlert1 = bi.DefaultAlert1;
                info.DefaultAlert2 = bi.DefaultAlert2;
                info.CriticalBias = bi.CriticalBias;
                info.DesignedCapacity = bi.DesignedCapacity;
                info.FullChargedCapacity = bi.FullChargedCapacity;
                info.TechnologyType = (BatteryTechnologyType)bi.Technology;

                var chemlist = new byte[] { bi.Chemistry1, bi.Chemistry2, bi.Chemistry3, bi.Chemistry4 };
                var chemListString = ASCIIEncoding.ASCII.GetString(chemlist);
                switch (chemListString)
                {
                    case "PbAc":
                        info.Chemistry = BatteryChemistryType.PbAc;
                        break;

                    case "LION":
                        info.Chemistry = BatteryChemistryType.LION;
                        break;

                    case "Li-I":
                        info.Chemistry = BatteryChemistryType.LiI;
                        break;

                    case "NiCd":
                        info.Chemistry = BatteryChemistryType.NiCd;
                        break;

                    case "NiMH":
                        info.Chemistry = BatteryChemistryType.NiMH;
                        break;

                    case "NiZn":
                        info.Chemistry = BatteryChemistryType.NiZn;
                        break;

                    case "RAM":
                        info.Chemistry = BatteryChemistryType.RAM;
                        break;

                    default:
                        info.Chemistry = BatteryChemistryType.Unknown;
                        break;
                }

                return info;
            }
            throwExceptionForError(result);
            return new BatteryInformation();
        }

        public static SystemPowerStatus QueryPowerStatus()
        {
            var sps = new SYSTEM_POWER_STATUS();
            var result = GetSystemPowerState(ref sps);
            if( result == 0)
            {
                throwExceptionForError(BatteryResult.UnsupportedFunction);
            }

            return new SystemPowerStatus
            {
                ACLineStatus = (SystemPowerAcState)sps.ACLineStatus,
                BatteryFlag = (SystemPowerBatteryType)sps.BatteryFlag,
                BatteryFullLifeTime = sps.BatteryFullLifeTime == -1 ? TimeSpan.Zero : TimeSpan.FromSeconds(sps.BatteryFullLifeTime),
                BatteryLifePercent = sps.BatteryLifePercent,
                BatteryLifeTime = sps.BatteryLifeTime == -1 ? TimeSpan.Zero: TimeSpan.FromSeconds(sps.BatteryLifeTime),
                SystemStatusFlag = (SystemBatteryStatusFlag)sps.SystemStatusFlag,
            };
        }
    }
}
