namespace Gmcu.Managed.Io.Battery
{
    /// <summary>
    /// Class that represents the power state of the computer.
    /// </summary>
    public class PowerState
    {
        private readonly SystemPowerStatus _powerStatus = new SystemPowerStatus();
        private readonly List<Battery> _batteries = new List<Battery>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PowerState"/> class.
        /// </summary>
        public PowerState()
        {
            _powerStatus.Refresh();
            Refresh(true);
        }

        /// <summary>
        /// Gets the overall system power status
        /// </summary>
        public ISystemPowerStatus SystemPowerStatus => _powerStatus;

        /// <summary>
        /// Gets the batteries present in the system.
        /// </summary>
        public IEnumerable<IBattery> Batteries => _batteries;

        /// <summary>
        /// Refreshes the power state
        /// </summary>
        /// <param name="enumeratedBatteries">If true, the batteries in the system are re-enumerated (slower).</param>
        public void Refresh( bool enumeratedBatteries = false)
        {
            _powerStatus.Refresh();
            if (enumeratedBatteries)
            {
                EnumerateBatteries();
            }
            else
            {
                foreach(var battery in _batteries)
                {
                    battery.Refresh();
                }
            }
        }

        /// <summary>
        /// Enumerates the batteries.
        /// </summary>
        private void EnumerateBatteries()
        {
            _batteries.Clear();
            for(int bIndex=0; bIndex < 100; bIndex++)
            {
                var battery = new Battery(bIndex);
                if( !battery.BatteryPresent)
                {
                    break;
                }
                _batteries.Add(battery);
            }
        }
    }
}
