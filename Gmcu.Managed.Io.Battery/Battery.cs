namespace Gmcu.Managed.Io.Battery
{
    public interface IBatteryInformation
    {
        /// <summary>
        /// Indicates that the battery capacity and rate information are relative, and not in any specific units. 
        /// If this bit is not set, the reporting units are milliwatt-hours (mWh) for capacity and milliwatts 
        /// (mW) for rate. If this bit is set, all references to units in the other battery documentation can 
        /// be ignored. All rate information is reported in units per hour. For example, if the fully charged 
        /// capacity is reported as 100, a rate of 200 indicates that the battery will use all of its capacity 
        /// in half an hour.
        /// </summary>
        bool IsCapacityRelative { get; }

        /// <summary>
        /// Indicates that the normal operation is for a fail-safe function. If this bit is not set the battery 
        /// is expected to be used during normal system usage.
        /// </summary>
        bool IsShortTerm { get; }

        /// <summary>
        /// Indicates that the battery can provide general power to run the system.
        /// </summary>
        bool IsSystemBattery { get; }

        /// <summary>
        /// The battery technology
        /// </summary>
        BatteryTechnologyType TechnologyType { get; }

        /// <summary>
        /// Indicates the battery's chemistry
        /// </summary>
        BatteryChemistryType Chemistry { get; }

        /// <summary>
        /// The theoretical capacity of the battery when new, in mWh unless 
        /// IsCapacityRelative is set. In that case, the units are undefined.
        /// </summary>
        uint DesignedCapacity { get; }

        /// <summary>
        /// The battery's current fully charged capacity in mWh (or relative). 
        /// Compare this value to DesignedCapacity to estimate the battery's wear.
        /// </summary>
        uint FullChargedCapacity { get; }

        /// <summary>
        /// The manufacturer's suggested capacity, in mWh, at which a low battery alert 
        /// should occur. Definitions of low vary from manufacturer to manufacturer. In 
        /// general, a warning state will occur before a low state, but you should not 
        /// assume that it always will. To reduce risk of data loss, this value is usually 
        /// used as the default setting for the critical battery alarm.
        /// </summary>
        uint DefaultAlert1 { get; }

        /// <summary>
        /// The manufacturer's suggested capacity, in mWh, at which a warning battery alert 
        /// should occur. Definitions of warning vary from manufacturer to manufacturer. In 
        /// general, a warning state will occur before a low state, but you should not 
        /// assume that it always will. To reduce risk of data loss, this value is usually 
        /// used as the default setting for the low battery alarm.
        /// </summary>
        uint DefaultAlert2 { get; }

        /// <summary>
        /// A bias from zero, in mWh, which is applied to battery reporting. Some batteries 
        /// reserve a small charge that is biased out of the battery's capacity values to 
        /// show "0" as the critical battery level. Critical bias is analogous to setting a 
        /// fuel gauge to show "empty" when there are several liters of fuel left.
        /// </summary>
        uint CriticalBias { get; }

        /// <summary>
        /// The number of charge/discharge cycles the battery has experienced. This provides 
        /// a means to determine the battery's wear. If the battery does not support a cycle 
        /// counter, this member is zero.
        /// </summary>
        uint CycleCount { get; }
    }

    /// <summary>
    /// Contains battery information.
    /// </summary>
    internal class BatteryInformation : IBatteryInformation
    {
        /// <summary>
        /// Indicates that the battery capacity and rate information are relative, and not in any specific units. 
        /// If this bit is not set, the reporting units are milliwatt-hours (mWh) for capacity and milliwatts 
        /// (mW) for rate. If this bit is set, all references to units in the other battery documentation can 
        /// be ignored. All rate information is reported in units per hour. For example, if the fully charged 
        /// capacity is reported as 100, a rate of 200 indicates that the battery will use all of its capacity 
        /// in half an hour.
        /// </summary>
        public bool IsCapacityRelative { get; internal set; }

        /// <summary>
        /// Indicates that the normal operation is for a fail-safe function. If this bit is not set the battery 
        /// is expected to be used during normal system usage.
        /// </summary>
        public bool IsShortTerm { get; internal set; }

        /// <summary>
        /// Indicates that the battery can provide general power to run the system.
        /// </summary>
        public bool IsSystemBattery { get; internal set; }

        /// <summary>
        /// The battery technology
        /// </summary>
        public BatteryTechnologyType TechnologyType { get; internal set; }

        /// <summary>
        /// Indicates the battery's chemistry
        /// </summary>
        public BatteryChemistryType Chemistry { get; internal set; }

        /// <summary>
        /// The theoretical capacity of the battery when new, in mWh unless 
        /// IsCapacityRelative is set. In that case, the units are undefined.
        /// </summary>
        public uint DesignedCapacity { get; internal set; }

        /// <summary>
        /// The battery's current fully charged capacity in mWh (or relative). 
        /// Compare this value to DesignedCapacity to estimate the battery's wear.
        /// </summary>
        public uint FullChargedCapacity { get; internal set; }

        /// <summary>
        /// The manufacturer's suggested capacity, in mWh, at which a low battery alert 
        /// should occur. Definitions of low vary from manufacturer to manufacturer. In 
        /// general, a warning state will occur before a low state, but you should not 
        /// assume that it always will. To reduce risk of data loss, this value is usually 
        /// used as the default setting for the critical battery alarm.
        /// </summary>
        public uint DefaultAlert1 { get; internal set; }

        /// <summary>
        /// The manufacturer's suggested capacity, in mWh, at which a warning battery alert 
        /// should occur. Definitions of warning vary from manufacturer to manufacturer. In 
        /// general, a warning state will occur before a low state, but you should not 
        /// assume that it always will. To reduce risk of data loss, this value is usually 
        /// used as the default setting for the low battery alarm.
        /// </summary>
        public uint DefaultAlert2 { get; internal set; }

        /// <summary>
        /// A bias from zero, in mWh, which is applied to battery reporting. Some batteries 
        /// reserve a small charge that is biased out of the battery's capacity values to 
        /// show "0" as the critical battery level. Critical bias is analogous to setting a 
        /// fuel gauge to show "empty" when there are several liters of fuel left.
        /// </summary>
        public uint CriticalBias { get; internal set; }

        /// <summary>
        /// The number of charge/discharge cycles the battery has experienced. This provides 
        /// a means to determine the battery's wear. If the battery does not support a cycle 
        /// counter, this member is zero.
        /// </summary>
        public uint CycleCount { get; internal set; }
    }

    /// <summary>
    /// Indicates the battery's chemistry
    /// </summary>
    public enum BatteryChemistryType
    {
        /// <summary>
        /// An unknown chemistry type.
        /// </summary>
        Unknown,

        /// <summary>
        /// Lead Acid
        /// </summary>
        PbAc,

        /// <summary>
        /// Lithium Ion
        /// </summary>
        LION,

        /// <summary>
        /// Lithium Ion
        /// </summary>
        LiI,

        /// <summary>
        /// Nickel Cadmium
        /// </summary>
        NiCd,

        /// <summary>
        /// Nickel Metal Hydride
        /// </summary>
        NiMH,

        /// <summary>
        /// Nickel Zinc
        /// </summary>
        NiZn,

        /// <summary>
        /// Rechargeable Alkaline-Manganese
        /// </summary>
        RAM
    }

    /// <summary>
    /// The battery technology. This member can be one of the following values.
    /// </summary>
    public enum BatteryTechnologyType
    {
        /// <summary>
        /// An unknown technology type.
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// Nonrechargeable battery, for example, alkaline.
        /// </summary>
        Nonrechargeable = 0,

        /// <summary>
        /// Rechargeable battery, for example, lead acid.
        /// </summary>
        Rechargeable = 1,
    }

    /// <summary>
    /// The battery state. 
    /// </summary>
    [Flags]
    public enum BatteryPowerState : uint
    {
        /// <summary>
        /// Indicates that the battery is currently charging.
        /// </summary>
        Charging = 0x00000004,

        /// <summary>
        /// Indicates that battery failure is imminent.
        /// </summary>
        Critical = 0x00000008,

        /// <summary>
        /// Indicates that the battery is currently discharging.
        /// </summary>
        Discharging = 0x00000002,

        /// <summary>
        /// Indicates that the system has access to AC power, so no batteries are being discharged.
        /// </summary>
        PowerOnline = 0x00000001
    }

    public interface IBatteryStatus
    {
        /// <summary>
        /// The battery state
        /// </summary>
        BatteryPowerState PowerState { get; }

        /// <summary>
        /// The current battery capacity, in mWh (or relative). This value can be used to generate a "gas gauge" 
        /// display by dividing it by FullChargedCapacity member of the BatteryInformation structure
        /// </summary>
        uint Capacity { get; }

        /// <summary>
        /// The current battery voltage across the battery terminals, in millivolts (mv)
        /// </summary>
        uint Voltage { get; }

        /// <summary>
        /// The current rate of battery charge or discharge. This value will be in milliwatts unless the battery 
        /// rate information is relative, in which case it will be in arbitrary units per hour. To determine if 
        /// battery information is relative, examine the CapacityRelative flag in the Capabilities member 
        /// of the BatteryInformation structure. A nonzero, positive rate indicates charging; a negative rate 
        /// indicates discharging. Some batteries report only discharging rates
        /// </summary>
        int DischargeRate { get; }
    }

    internal class BatteryStatus : IBatteryStatus
    {
        /// <summary>
        /// The battery state
        /// </summary>
        public BatteryPowerState PowerState { get; set; }

        /// <summary>
        /// The current battery capacity, in mWh (or relative). This value can be used to generate a "gas gauge" 
        /// display by dividing it by FullChargedCapacity member of the BatteryInformation structure
        /// </summary>
        public uint Capacity { get; set; }

        /// <summary>
        /// The current battery voltage across the battery terminals, in millivolts (mv)
        /// </summary>
        public uint Voltage { get; set; }

        /// <summary>
        /// The current rate of battery charge or discharge. This value will be in milliwatts unless the battery 
        /// rate information is relative, in which case it will be in arbitrary units per hour. To determine if 
        /// battery information is relative, examine the CapacityRelative flag in the Capabilities member 
        /// of the BatteryInformation structure. A nonzero, positive rate indicates charging; a negative rate 
        /// indicates discharging. Some batteries report only discharging rates
        /// </summary>
        public int DischargeRate { get; set; }
    }

    /// <summary>
    /// Interface that represents a battery 
    /// </summary>
    public interface IBattery
    {
        /// <summary>
        /// Gets the status of the battery
        /// </summary>
        IBatteryStatus Status { get; }

        /// <summary>
        /// Gets the battery temperature
        /// </summary>
        float Temperature { get; }

        /// <summary>
        /// Gets the estimated remaining time for the battery.
        /// (TimeSpan.Zero indicates not applicable)
        /// </summary>
        TimeSpan EstimatedTime { get; }

        /// <summary>
        /// Gets the device name
        /// </summary>
        string DeviceName { get; }

        /// <summary>
        /// Gets the manufacturer name
        /// </summary>
        string ManufacturerName { get; }

        /// <summary>
        /// Gets the serial number
        /// </summary>
        string SerialNumber { get; }

        /// <summary>
        /// Gets the unique identifier of the battery
        /// </summary>
        string UniqueIdentifier { get; }

        /// <summary>
        /// Gets the manufacture date
        /// (DateTime.MinValue indicates undefined)
        /// </summary>
        DateTime ManufactureDate { get; }

        /// <summary>
        /// Gets the battery information
        /// </summary>
        IBatteryInformation Information { get; }
    }

    /// <summary>
    /// Class that represents an instance of the 
    /// </summary>
    internal class Battery : IBattery
    {
        private readonly int _batteryIndex;

        public Battery(int batteryIndex)
        {
            _batteryIndex = batteryIndex;
             BatteryPresent = Refresh();
        }

        public IBatteryStatus Status { get; set; } = new BatteryStatus();
        public float Temperature { get; set; }
        public TimeSpan EstimatedTime { get; set; } = TimeSpan.Zero;
        public string DeviceName { get; set; } = string.Empty;
        public string ManufacturerName { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string UniqueIdentifier { get; set; } = string.Empty;
        public DateTime ManufactureDate { get; set; } = DateTime.MinValue;
        public IBatteryInformation Information { get; set; } = new BatteryInformation();
        public bool BatteryPresent { get; }

        public bool Refresh()
        {
            var result = true;

            try
            {
                Status = BatteryInfo.QueryBatteryStatus(_batteryIndex);
            }
            catch(BatteryInfoException bie)
            {
                if( bie.Result == BatteryResult.BatteryDoesNotExist )
                {
                    result = false;
                }
                Status = new BatteryStatus();
            }
            catch (Exception)
            {
                Status = new BatteryStatus();
            }

            try
            {
                Temperature = BatteryInfo.QueryTemperature(_batteryIndex);
            }
            catch(Exception)
            {
                Temperature = 0;
            }

            try
            {
                EstimatedTime = BatteryInfo.QueryEstimatedTime(_batteryIndex);
            }
            catch (Exception)
            {
                EstimatedTime = TimeSpan.Zero;
            }

            try
            {
                DeviceName = BatteryInfo.QueryDeviceName(_batteryIndex);
            }
            catch(Exception)
            {
                DeviceName= string.Empty;
            }

            try
            {
                ManufacturerName = BatteryInfo.QueryManufactureName(_batteryIndex);
            }
            catch(Exception)
            {
                ManufacturerName = string.Empty;
            }

            try
            {
                SerialNumber = BatteryInfo.QuerySerialNumber(_batteryIndex);
            }
            catch( Exception)
            {
                SerialNumber = string.Empty;
            }

            try
            {
                UniqueIdentifier = BatteryInfo.QueryUniqueId(_batteryIndex);
            }
            catch(Exception)
            {
                UniqueIdentifier = string.Empty;
            }

            try
            {
                ManufactureDate = BatteryInfo.QueryManufactureDate(_batteryIndex);
            }
            catch(Exception)
            {
                ManufactureDate = DateTime.MinValue;
            }

            try
            {
                Information = BatteryInfo.QueryBatteryInformation(_batteryIndex);
            }
            catch(Exception)
            {
                Information = new BatteryInformation();
            }

            return result;
        }
    }
}
