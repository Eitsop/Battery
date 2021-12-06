namespace Gmcu.Managed.Io.Battery
{
    /// <summary>
    /// The AC power status. This member can be one of the following values.
    /// </summary>
    public enum SystemPowerAcState : byte
    {
        Offline = 0,
        Online = 1,
        UnknownStatus = 255
    }

    /// <summary>
    /// The battery charge status. This member can contain one or more of the following flags.
    /// </summary>
    [Flags]
    public enum SystemPowerBatteryType : byte
    {
        /// <summary>
        /// The battery capacity is at more than 66 percent
        /// </summary>
        High = 1,

        /// <summary>
        /// The battery capacity is at less than 33 percent
        /// </summary>
        Low = 2,

        /// <summary>
        /// The battery capacity is at less than five percent
        /// </summary>
        Critical = 4,

        Charging = 8,

        /// <summary>
        /// No system battery
        /// </summary>
        NoSystemBattery = 128,

        /// <summary>
        /// Unable to read the battery flag information
        /// </summary>
        Unknown = 255
    }

    /// <summary>
    /// The status of battery saver. To participate in energy conservation, 
    /// avoid resource intensive tasks when battery saver is on.
    /// </summary>
    public enum SystemBatteryStatusFlag
    {
        /// <summary>
        /// Battery saver is off.
        /// </summary>
        BatterySaverOff = 0,

        /// <summary>
        /// Battery saver on. Save energy where possible.
        /// </summary>
        BatterySaverOm = 1,
    }

    /// <summary>
    /// Contains information about the power status of the system.
    /// </summary>
    public interface ISystemPowerStatus
    {
        /// <summary>
        /// The AC power status
        /// </summary>
        SystemPowerAcState ACLineStatus { get; }

        /// <summary>
        /// The battery charge status.
        /// </summary>
        SystemPowerBatteryType BatteryFlag { get; }

        /// <summary>
        /// The percentage of full battery charge remaining. This 
        /// member can be a value in the range 0 to 100, or 255 if status is unknown.
        /// </summary>
        int BatteryLifePercent { get; }

        /// <summary>
        /// The status of battery saver. To participate in energy conservation, 
        /// avoid resource intensive tasks when battery saver is on.
        /// </summary>
        SystemBatteryStatusFlag SystemStatusFlag { get; }

        /// <summary>
        /// The life time of battery life remaining, or 0 if remaining seconds are unknown 
        /// or if the device is connected to AC power.
        /// </summary>
        TimeSpan BatteryLifeTime { get; }

        /// <summary>
        /// The time of battery life when at full charge, or 0 
        /// if full battery lifetime is unknown or if the device is connected to AC power.
        /// </summary>
        TimeSpan BatteryFullLifeTime { get; }
    }

    /// <summary>
    /// Contains information about the power status of the system.
    /// </summary>
    internal class SystemPowerStatus : ISystemPowerStatus
    {
        /// <summary>
        /// The AC power status
        /// </summary>
        public SystemPowerAcState ACLineStatus { get; set; }

        /// <summary>
        /// The battery charge status.
        /// </summary>
        public SystemPowerBatteryType BatteryFlag { get; set; }

        /// <summary>
        /// The percentage of full battery charge remaining. This 
        /// member can be a value in the range 0 to 100, or 255 if status is unknown.
        /// </summary>
        public int BatteryLifePercent { get; set; }

        /// <summary>
        /// The status of battery saver. To participate in energy conservation, 
        /// avoid resource intensive tasks when battery saver is on.
        /// </summary>
        public SystemBatteryStatusFlag SystemStatusFlag { get; set; }

        /// <summary>
        /// The life time of battery life remaining, or 0 if remaining seconds are unknown 
        /// or if the device is connected to AC power.
        /// </summary>
        public TimeSpan BatteryLifeTime { get; set; }

        /// <summary>
        /// The time of battery life when at full charge, or 0 
        /// if full battery lifetime is unknown or if the device is connected to AC power.
        /// </summary>
        public TimeSpan BatteryFullLifeTime { get; set; }

        /// <summary>
        /// Updates from a duplicate of this instance.
        /// </summary>
        /// <param name="systemPowerStatus">The power status to update from</param>
        private void UpdateFrom(ISystemPowerStatus systemPowerStatus)
        {
            ACLineStatus = systemPowerStatus.ACLineStatus;
            BatteryFlag = systemPowerStatus.BatteryFlag;
            BatteryLifePercent = systemPowerStatus.BatteryLifePercent;
            SystemStatusFlag = systemPowerStatus.SystemStatusFlag;
            BatteryFullLifeTime = systemPowerStatus.BatteryFullLifeTime;
            BatteryLifeTime = systemPowerStatus.BatteryLifeTime;
        }

        /// <summary>
        /// Refreshes the content of this instance.
        /// </summary>
        public void Refresh()
        {
            UpdateFrom(BatteryInfo.QueryPowerStatus());
        }
    }
}
