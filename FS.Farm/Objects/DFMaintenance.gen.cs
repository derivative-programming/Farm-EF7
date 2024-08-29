using System;
using System.Data;
using System.Configuration;
using FS.Farm.Managers;
using Newtonsoft.Json.Linq;

/// <summary>
/// Summary description for DFMaintenance
/// </summary>
namespace FS.Farm.Objects
{
    /// <summary>
    /// DF Maintenance DF Maintenance
    /// </summary>
    public partial class DFMaintenance : FS.Base.Objects.BaseObject
    {

        public DFMaintenance()
        {
            IsLoaded = false;
        }

        #region private vars
        int _DFMaintenanceID;
        private Boolean _IsPaused = false;
        private Boolean _IsScheduledDFProcessRequestCompleted = false;
        private Boolean _IsScheduledDFProcessRequestStarted = false;
        private DateTime _LastScheduledDFProcessRequestUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        private DateTime _NextScheduledDFProcessRequestUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        private Int32 _PacID = 0;
        private String _PausedByUsername = String.Empty;
        private DateTime _PausedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        private String _ScheduledDFProcessRequestProcessorIdentifier = String.Empty;
        private System.Guid _PacCodePeek;

        Guid _LastChangeCode;
        System.Guid _Code;
        #endregion

        #region Public Props

        /// <summary>
        /// primary DFMaintenance db id
        /// DB Data type: Integer
        /// </summary>
        public int DFMaintenanceID
        {
            get { return _DFMaintenanceID; }
            set
            {
                this.IsDirty = true; _DFMaintenanceID = value;
            }
        }
        /// <summary>
        /// DB Data Type: bit, size: , DF Maintenance Is Paused
        /// </summary>
        public Boolean IsPaused
        {
            get { return _IsPaused; }
            set
            {
                if (_IsPaused != value)
                {
                    this.IsDirty = true;
                    _IsPaused = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: bit, size: , DF Maintenance Is Scheduled DF Process Request Completed
        /// </summary>
        public Boolean IsScheduledDFProcessRequestCompleted
        {
            get { return _IsScheduledDFProcessRequestCompleted; }
            set
            {
                if (_IsScheduledDFProcessRequestCompleted != value)
                {
                    this.IsDirty = true;
                    _IsScheduledDFProcessRequestCompleted = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: bit, size: , DF Maintenance Is Scheduled DF Process Request Started
        /// </summary>
        public Boolean IsScheduledDFProcessRequestStarted
        {
            get { return _IsScheduledDFProcessRequestStarted; }
            set
            {
                if (_IsScheduledDFProcessRequestStarted != value)
                {
                    this.IsDirty = true;
                    _IsScheduledDFProcessRequestStarted = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: datetime, size: , DF Maintenance Last Scheduled DF Process Request UTC Date Time
        /// </summary>
        public DateTime LastScheduledDFProcessRequestUTCDateTime
        {
            get { return _LastScheduledDFProcessRequestUTCDateTime; }
            set
            {
                if (_LastScheduledDFProcessRequestUTCDateTime.ToString("yyyyMMddHHmmss") != value.ToString("yyyyMMddHHmmss"))
                {
                    this.IsDirty = true;
                    _LastScheduledDFProcessRequestUTCDateTime = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: datetime, size: , DF Maintenance Next Scheduled DF Process Request UTC Date Time
        /// </summary>
        public DateTime NextScheduledDFProcessRequestUTCDateTime
        {
            get { return _NextScheduledDFProcessRequestUTCDateTime; }
            set
            {
                if (_NextScheduledDFProcessRequestUTCDateTime.ToString("yyyyMMddHHmmss") != value.ToString("yyyyMMddHHmmss"))
                {
                    this.IsDirty = true;
                    _NextScheduledDFProcessRequestUTCDateTime = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: int, size: , DF Maintenance Pac ID
        /// </summary>
        public Int32 PacID
        {
            get { return _PacID; }
            set
            {
                if (_PacID != value)
                {
                    this.IsDirty = true;
                    _PacID = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: nvarchar, size: 100, DF Maintenance Paused By Username
        /// </summary>
        public String PausedByUsername
        {
            get { return _PausedByUsername; }
            set
            {
                if (_PausedByUsername != value)
                {
                    this.IsDirty = true;
                    _PausedByUsername = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: datetime, size: , DF Maintenance Paused UTC Date Time
        /// </summary>
        public DateTime PausedUTCDateTime
        {
            get { return _PausedUTCDateTime; }
            set
            {
                if (_PausedUTCDateTime.ToString("yyyyMMddHHmmss") != value.ToString("yyyyMMddHHmmss"))
                {
                    this.IsDirty = true;
                    _PausedUTCDateTime = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: nvarchar, size: 10, DF Maintenance Scheduled DF Process Request Processor Identifier
        /// </summary>
        public String ScheduledDFProcessRequestProcessorIdentifier
        {
            get { return _ScheduledDFProcessRequestProcessorIdentifier; }
            set
            {
                if (_ScheduledDFProcessRequestProcessorIdentifier != value)
                {
                    this.IsDirty = true;
                    _ScheduledDFProcessRequestProcessorIdentifier = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: uniqueidentifier
        /// </summary>
        public System.Guid PacCodePeek
        {
            get { return _PacCodePeek; }
            set
            {
                if (_PacCodePeek != value)
                {
                    this.IsDirty = true;
                    _PacCodePeek = value;
                }
            }
        }

        /// <summary>
        /// non lookup table: Guid
        /// lookup table: Human readable string unique to table
        /// DB Data Type: VARCHAR(50)
        /// </summary>
        public System.Guid Code
        {
            get { return _Code; }
            set { this.IsDirty = true; _Code = value; }
        }

         public Guid LastChangeCode
         {
             get { return _LastChangeCode; }
             set { this.IsDirty = true; _LastChangeCode = value; }
         }

        #endregion

        private static object _lockObject = new object();

        public void Copy(DFMaintenance obj)
        {
            //  base.Copy(obj);
            this.IsLoaded = false;
            this.DFMaintenanceID = obj.DFMaintenanceID;
            this.IsObjectInvalidated = obj.IsObjectInvalidated;
             this.IsPaused = obj.IsPaused;
             this.IsScheduledDFProcessRequestCompleted = obj.IsScheduledDFProcessRequestCompleted;
             this.IsScheduledDFProcessRequestStarted = obj.IsScheduledDFProcessRequestStarted;
             this.LastScheduledDFProcessRequestUTCDateTime = obj.LastScheduledDFProcessRequestUTCDateTime;
             this.NextScheduledDFProcessRequestUTCDateTime = obj.NextScheduledDFProcessRequestUTCDateTime;
             this.PacID = obj.PacID;
             this.PausedByUsername = obj.PausedByUsername;
             this.PausedUTCDateTime = obj.PausedUTCDateTime;
             this.ScheduledDFProcessRequestProcessorIdentifier = obj.ScheduledDFProcessRequestProcessorIdentifier;
            this.PacCodePeek = obj.PacCodePeek;

            this.Code = obj.Code;
            this.LastChangeCode = obj.LastChangeCode;
            this.SetIsLoaded();
            this.IsDirty = obj.IsDirtyObject();
        }
    }
}