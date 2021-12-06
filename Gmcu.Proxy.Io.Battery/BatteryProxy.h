#pragma once

#ifdef __cplusplus
extern "C" {  // only need to export C interface if
              // used by C++ source code
#endif

#define BATTERY_SUCCEESS                            0
#define BATTERY_ERROR_UNABLE_TO_OPEN_HANDLE         -101
#define BATTERY_ERROR_DOES_NOT_EXIST                -102
#define BATTERY_ERROR_MEMORY_ERROR                  -103
#define BATTERY_ERROR_DEVICE_INTERFACE_DETAIL       -104
#define BATTERY_ERROR_DEVICE_INTERFACE_SIZE         -105
#define BATTERY_ERROR_ACCESS_DEVICE                 -106
#define BATTERY_ERROR_QUERY_TAG                     -107
#define BATTERY_ERROR_QUERY_INFORMATION             -108
#define BATTERY_ERROR_UNSUPPORTED_FUNCTION          -109
#define BATTERY_UNKNOWN_ESTIMATED_TIME              -110

__declspec(dllexport) int __cdecl GetBatteryInformation(int batteryIndex, BATTERY_INFORMATION& batteryInfomation);
__declspec(dllexport) int __cdecl GetManufactureDate(int batteryIndex, BATTERY_MANUFACTURE_DATE& batteryManufactureDate);
__declspec(dllexport) int __cdecl GetManufactureName(int batteryIndex, char* name, int nameLength);
__declspec(dllexport) long __cdecl GetTemperature(int batteryIndex);
__declspec(dllexport) int __cdecl GetSerialNumber(int batteryIndex, char* serialNumber, int serialNumberLength);
__declspec(dllexport) int __cdecl GetUniqueId(int batteryIndex, char* uniqueId, int uniqueIdLength);
__declspec(dllexport) int __cdecl GetDeviceName(int batteryIndex, char* deviceName, int deviceNameLength);
__declspec(dllexport) int __cdecl GetEstimatedTime(int batteryIndex);
__declspec(dllexport) int __cdecl GetStatus( int batteryIndex, BATTERY_STATUS& STATUS);
__declspec(dllexport) int __cdecl GetSystemPowerState(SYSTEM_POWER_STATUS& state);

#ifdef __cplusplus
}
#endif