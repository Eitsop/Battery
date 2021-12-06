namespace Gmcu.Managed.Io.Battery
{
    /// <summary>
    /// Defines the possible result values when performing battery device queries.
    /// </summary>
    internal enum BatteryResult : int
    {
        /// <summary>
        /// There was no error - success
        /// </summary>
        Success = 0,

        /// <summary>
        /// Failed to open the device component interface
        /// </summary>
        FailedToOpenBattery = -101,

        /// <summary>
        /// The battery index selected doesn't exist.
        /// </summary>
        BatteryDoesNotExist = -102,

        /// <summary>
        /// Local memory used to retrieve data has failed to be allocated.
        /// </summary>
        FailedToAllocateMemory = -103,

        /// <summary>
        /// Failed to determine if the selected interface is supported.
        /// </summary>
        FailedToDetermineBatteryInterface = -104,

        /// <summary>
        /// Failed to determine the size of the selected interface.
        /// </summary>
        FailedToDetermineBatteryInterfaceSize = -105,

        /// <summary>
        /// Failed to open the battery device to perform the query
        /// </summary>
        FailedToAccessBattery = -106,

        /// <summary>
        /// The system-wide battery identifier could not be determined.
        /// </summary>
        UnableToDetermineBatteryIdentifier = -107,

        /// <summary>
        /// The query performed on the data interface could not be performed.
        /// </summary>
        FailedToRetrieveQueryData = -108,

        /// <summary>
        /// The query to be performed is not supported by the device.
        /// </summary>
        UnsupportedFunction = -109,
    }


    /// <summary>
    /// Exception raised when an error occurs accessing the battery properties.
    /// </summary>
    internal class BatteryInfoException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BatteryInfoException"/> class.
        /// </summary>
        /// <param name="result">The definition why the exception occurred.</param>
        public BatteryInfoException(BatteryResult result)
        {
            Result = result;
        }

        /// <summary>
        /// Gets the result of the property definition.
        /// </summary>
        public BatteryResult Result { get; }
    }
}
