using AuxiliaryLibrary.Tools;
using Base.Devices;
using Base.Devices.Management;
using Base.OpCodes.Helpers;
using Base.OpCodes.Management;
using Maintenance;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Devices.Components
{
    public abstract class SetupFile<T> where T : OpCodeWrapper
    {
        #region Static
        public enum SetupStatusEnum { Idle, Valid, Corrupt, Missing, Incompatible, UnexpectedError };
        private static string[] SetupStatuses = new string[] { "Idle", "Valid", "Corrupt setup", "File not found", "Incompatible version/device", "Unexpected Error" };
        public static string GetSetupFileError(SetupStatusEnum status)
        {
            return SetupStatuses[(int)status];
        }
        #endregion
        #region Properties
        public T ParentOpCodeWrapper
        {
            get;
            protected set;
        }
        #endregion
        #region Constructors
        public SetupFile(T parent)
        {
            this.ParentOpCodeWrapper = parent;
        }
        #endregion
        #region Save
        public Result Save(string path)
        {
            try
            {
                using (IO FileIO = new IO(path, false))
                {
                    FileIO.EncryptAndSerialize<T>(ParentOpCodeWrapper);
                    return Result.OK;
                }
            }
            catch (System.Exception ex)
            {
                Log.Write("Error saving device configuration", ex);
                return Result.ERROR;
            }
        }
        #endregion
        #region Load
        public SetupStatusEnum Load(string path)
        {
            try
            {
                OpCodeWrapper opCodes;
                SetupStatusEnum status = LoadOpCodes(path, out opCodes);
                if (status == SetupStatusEnum.Valid)
                {
                    ParentOpCodeWrapper.Assimilate(opCodes);
                }
                return status;
            }
            catch (System.Exception ex)
            {
                Log.Write("There was an error reading configuration file.", ex);
                return SetupStatusEnum.UnexpectedError;
            }
        }

        private T LoadFile(string FileName)
        {
            try
            {
                using (IO FileIO = new IO(FileName, false))
                {
                    return FileIO.DecryptAndDeserialize<T>();
                }
            }
            catch (System.Exception ex)
            {
                Log.Write(ex);
                return default(T);
            }
        }

        private SetupStatusEnum LoadOpCodes(string path, out OpCodeWrapper opCodes)
        {
            FileInfo fileInfo = new FileInfo(path);
            SetupStatusEnum status = SetupStatusEnum.Idle;
            opCodes = null;
            try
            {
                if (fileInfo.Exists)
                {
                    opCodes = LoadFile(path) as OpCodeWrapper;
                    if (opCodes == null)
                        status = SetupStatusEnum.Corrupt;
                    else if (opCodes.CompatibleWith(typeof(T)))
                    {
                        status = SetupStatusEnum.Valid;
                    }
                    else
                        status = SetupStatusEnum.Incompatible;
                }
                else
                    status = SetupStatusEnum.Missing;
            }
            catch (System.Exception)
            {
                status = SetupStatusEnum.Corrupt;
            }

            return status;
        }
        #endregion
    }
}
