using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroXBase.Devices.Features
{
    public enum MicroXDeviceFunctionEnum : byte
    {
        NONE = 0x00,
        TURN_OFF,
        UPDATE_FIRMWARE,
        CANCEL_FIRMWARE_UPDATE,
        RUN,
        STOP,
        DOWNLOAD,
        CANCEL_DOWNLOAD,
        CALIBRATE,
        DEFAULT_CALIBRATION_SAVE,
        DEFAULT_CALIBRATION_LOAD,
        SETUP,
        TEST,
        FAST_CLOCK,
        MARK_TIMESTAMP,
    }
}
