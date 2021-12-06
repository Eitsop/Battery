#include "pch.h"
#include "framework.h"
#include <setupapi.h>
#include <poclass.h>
#include <BatClass.h>
#include <winioctl.h>
#include <WinBase.h>
#include "BatteryProxy.h"

DEFINE_GUID(GUID_DEVCLASS_BATTERY, 0x72631e54, 0x78a4, 0x11d0,
    0xbc, 0xf7, 0x00, 0xaa, 0x00, 0xb7, 0xb3, 0x2a);

typedef int (*battery_func_ptr)(HANDLE hBattery, ULONG batteryTag, void* query_args);
typedef struct _BATTERY_INFO_STRUCT
{
    char* struct_addr;
    DWORD size_of_struct;
    BATTERY_QUERY_INFORMATION_LEVEL bqil;
} BATTERY_INFO_STRUCT;

static int __cdecl queryBattery(int batteryIndex, battery_func_ptr query_func, void* query_args)
{
    // IOCTL_BATTERY_QUERY_INFORMATION,
    // enumerate the batteries and ask each one for information.

    int result = BATTERY_SUCCEESS;

    HDEVINFO hdev =
        SetupDiGetClassDevs(&GUID_DEVCLASS_BATTERY,
            0,
            0,
            DIGCF_PRESENT | DIGCF_DEVICEINTERFACE);
    if (INVALID_HANDLE_VALUE != hdev)
    {
        SP_DEVICE_INTERFACE_DATA did = { 0 };
        did.cbSize = sizeof(did);

        if (SetupDiEnumDeviceInterfaces(hdev,
            0,
            &GUID_DEVCLASS_BATTERY,
            batteryIndex,
            &did))
        {
            DWORD cbRequired = 0;

            SetupDiGetDeviceInterfaceDetail(hdev,
                &did,
                0,
                0,
                &cbRequired,
                0);
            if (ERROR_INSUFFICIENT_BUFFER == GetLastError())
            {
                PSP_DEVICE_INTERFACE_DETAIL_DATA pdidd =
                    (PSP_DEVICE_INTERFACE_DETAIL_DATA)LocalAlloc(LPTR,
                        cbRequired);
                if (pdidd)
                {
                    pdidd->cbSize = sizeof(*pdidd);
                    if (SetupDiGetDeviceInterfaceDetail(hdev,
                        &did,
                        pdidd,
                        cbRequired,
                        &cbRequired,
                        0))
                    {
                        // Enumerated a battery.  Ask it for information.
                        HANDLE hBattery =
                            CreateFile(pdidd->DevicePath,
                                GENERIC_READ | GENERIC_WRITE,
                                FILE_SHARE_READ | FILE_SHARE_WRITE,
                                NULL,
                                OPEN_EXISTING,
                                FILE_ATTRIBUTE_NORMAL,
                                NULL);

                        if (INVALID_HANDLE_VALUE != hBattery)
                        {
                            // Ask the battery for its tag.

                            ULONG batteryTag = 0;

                            DWORD dwWait = 0;
                            DWORD dwOut;

                            if (DeviceIoControl(hBattery,
                                IOCTL_BATTERY_QUERY_TAG,
                                &dwWait,
                                sizeof(dwWait),
                                &batteryTag,
                                sizeof(batteryTag),
                                &dwOut,
                                NULL)
                                && batteryTag)
                            {
                                result = (*query_func)(hBattery, batteryTag, query_args );
                                //// With the tag, you can query the battery info.
                                //BATTERY_QUERY_INFORMATION bqi = { 0 };
                                //bqi.BatteryTag = batteryTag;
                                //bqi.InformationLevel = bqil;
                                //bqi.AtRate = 0;

                                //if (DeviceIoControl(hBattery,
                                //    IOCTL_BATTERY_QUERY_INFORMATION,
                                //    &bqi,
                                //    sizeof(bqi),
                                //    struct_addr,
                                //    size_of_struct,
                                //    &dwOut,
                                //    NULL) ==0 )
                                //{
                                //    if (ERROR_INVALID_FUNCTION == GetLastError())
                                //    {
                                //        result = BATTERY_ERROR_UNSUPPORTED_FUNCTION;
                                //    }
                                //    else
                                //    {
                                //        result = BATTERY_ERROR_QUERY_INFORMATION;
                                //    }
                                //}
                            }
                            else
                            {
                                result = BATTERY_ERROR_QUERY_TAG;
                            }
                            CloseHandle(hBattery);
                        }
                        else
                        {
                            result = BATTERY_ERROR_ACCESS_DEVICE;
                        }
                    }
                    else
                    {
                        result = BATTERY_ERROR_DEVICE_INTERFACE_DETAIL;
                    }

                    LocalFree(pdidd);
                }
                else
                {
                    result = BATTERY_ERROR_MEMORY_ERROR;
                }
            }
            else
            {
                result = BATTERY_ERROR_DEVICE_INTERFACE_SIZE;
            }
        }
        else  if (ERROR_NO_MORE_ITEMS == GetLastError())
        {
            result = BATTERY_ERROR_DOES_NOT_EXIST;
        }

        SetupDiDestroyDeviceInfoList(hdev);
    }
    else
    {
        result = BATTERY_ERROR_UNABLE_TO_OPEN_HANDLE;
    }

    return result;
}

static int __cdecl queryBatteryInfo(int batteryIndex, char* struct_addr, int size_of_struct, BATTERY_QUERY_INFORMATION_LEVEL bqil, battery_func_ptr query_func )
{
    BATTERY_INFO_STRUCT bis = { 0, };
    bis.bqil = bqil;
    bis.size_of_struct = size_of_struct;
    bis.struct_addr = struct_addr;

    return queryBattery(batteryIndex, query_func,  &bis);
}

static int battery_query_information(HANDLE hBattery, ULONG batteryTag, void* query_args)
{
    BATTERY_INFO_STRUCT* bis = (BATTERY_INFO_STRUCT*)query_args;

    // With the tag, you can query the battery info.
    BATTERY_QUERY_INFORMATION bqi = { 0 };
    bqi.BatteryTag = batteryTag;
    bqi.InformationLevel = bis->bqil;
    bqi.AtRate = 0;

    DWORD dwOut = 0;
    int result = BATTERY_SUCCEESS;

    if (DeviceIoControl(hBattery,
        IOCTL_BATTERY_QUERY_INFORMATION,
        &bqi,
        sizeof(bqi),
        bis->struct_addr,
        bis->size_of_struct,
        &dwOut,
        NULL) == 0)
    {
        if (ERROR_INVALID_FUNCTION == GetLastError())
        {
            result = BATTERY_ERROR_UNSUPPORTED_FUNCTION;
        }
        else
        {
            result = BATTERY_ERROR_QUERY_INFORMATION;
        }
    }

    return result;
}

static int battery_status(HANDLE hBattery, ULONG batteryTag, void* query_args)
{
    BATTERY_STATUS* status = (BATTERY_STATUS*)query_args;

    BATTERY_WAIT_STATUS bws = { 0 };
    bws.BatteryTag = batteryTag;
    bws.Timeout = 0;

    DWORD dwOut = 0;
    int result = BATTERY_SUCCEESS;

    if (DeviceIoControl(hBattery,
        IOCTL_BATTERY_QUERY_STATUS,
        &bws,
        sizeof(bws),
        status,
        sizeof(BATTERY_STATUS),
        &dwOut,
        NULL) == 0)
    {
        if (ERROR_INVALID_FUNCTION == GetLastError())
        {
            result = BATTERY_ERROR_UNSUPPORTED_FUNCTION;
        }
        else
        {
            result = BATTERY_ERROR_QUERY_INFORMATION;
        }
    }

    return result;
}

int GetBatteryInformation( int batteryIndex, BATTERY_INFORMATION& batteryInformation)
{
    return queryBatteryInfo(batteryIndex, (char*)&batteryInformation, sizeof(BATTERY_INFORMATION), BatteryInformation, battery_query_information);
}

int GetManufactureDate(int batteryIndex, BATTERY_MANUFACTURE_DATE& batteryManufactureDate)
{
    return queryBatteryInfo(batteryIndex, (char*)&batteryManufactureDate, sizeof(BATTERY_MANUFACTURE_DATE), BatteryManufactureDate, battery_query_information);
}

int GetManufactureName(int batteryIndex, char* name, int nameLength)
{
    return queryBatteryInfo(batteryIndex, name, nameLength, BatteryManufactureName, battery_query_information);
}

int GetSerialNumber(int batteryIndex, char* serialNumber, int serialNumberLength)
{
    return queryBatteryInfo(batteryIndex, serialNumber, serialNumberLength, BatterySerialNumber, battery_query_information);
}

int GetUniqueId(int batteryIndex, char* uniqueId, int uniqueIdLength)
{
    return queryBatteryInfo(batteryIndex, uniqueId, uniqueIdLength, BatteryUniqueID, battery_query_information);
}

int GetDeviceName(int batteryIndex, char* deviceName, int deviceNameLength)
{
    return queryBatteryInfo(batteryIndex, deviceName, deviceNameLength, BatteryDeviceName, battery_query_information);
}

int GetEstimatedTime(int batteryIndex)
{
    ULONG timeVal = 0;
    int result = queryBatteryInfo(batteryIndex, (char*)&timeVal, sizeof(ULONG), BatteryEstimatedTime, battery_query_information);
    if (result == BATTERY_UNKNOWN_TIME || timeVal == BATTERY_UNKNOWN_TIME)
    {
        return BATTERY_UNKNOWN_ESTIMATED_TIME;
    }
    if (result >= 0)
    {
        return (long)timeVal;
    }
    return result;
}

long GetTemperature(int batteryIndex)
{
    ULONG tempVal = 0;
    int result = queryBatteryInfo(batteryIndex, (char*)&tempVal, sizeof(ULONG), BatteryTemperature, battery_query_information);
    if (result >= 0)
    {
        return (long)tempVal;
    }
    return result;
}

int GetStatus(int batteryIndex, BATTERY_STATUS& status)
{
    return queryBattery(batteryIndex, battery_status, &status);
}

int GetSystemPowerState(SYSTEM_POWER_STATUS& state)
{
    return GetSystemPowerStatus(&state);
}
