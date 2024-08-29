using System;
using System.Data;
using System.Configuration;
using FS.Farm.Managers;
using Newtonsoft.Json.Linq;

/// <summary>
/// Summary description for DynaFlowTypeSchedule
/// </summary>
namespace FS.Farm.Objects
{
    /// <summary>
    /// Dyna Flow Type Schedule Dyna Flow Type Schedule
    /// </summary>
    public partial class DynaFlowTypeSchedule : FS.Base.Objects.BaseObject
    {

        public DynaFlowTypeSchedule()
        {
            IsLoaded = false;
        }

        #region private vars
        int _DynaFlowTypeScheduleID;
        private Int32 _DynaFlowTypeID = 0;
        private Int32 _FrequencyInHours = 0;
        private Boolean _IsActive = false;
        private DateTime _LastUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        private DateTime _NextUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        private Int32 _PacID = 0;
        private System.Guid _DynaFlowTypeCodePeek;
        private System.Guid _PacCodePeek;

        Guid _LastChangeCode;
        System.Guid _Code;
        #endregion

        #region Public Props

        /// <summary>
        /// primary DynaFlowTypeSchedule db id
        /// DB Data type: Integer
        /// </summary>
        public int DynaFlowTypeScheduleID
        {
            get { return _DynaFlowTypeScheduleID; }
            set
            {
                this.IsDirty = true; _DynaFlowTypeScheduleID = value;
            }
        }
        /// <summary>
        /// DB Data Type: int, size: , Dyna Flow Type Schedule Dyna Flow Type ID
        /// </summary>
        public Int32 DynaFlowTypeID
        {
            get { return _DynaFlowTypeID; }
            set
            {
                if (_DynaFlowTypeID != value)
                {
                    this.IsDirty = true;
                    _DynaFlowTypeID = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: int, size: , Dyna Flow Type Schedule Frequency In Hours
        /// </summary>
        public Int32 FrequencyInHours
        {
            get { return _FrequencyInHours; }
            set
            {
                if (_FrequencyInHours != value)
                {
                    this.IsDirty = true;
                    _FrequencyInHours = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: bit, size: , Dyna Flow Type Schedule Is Active
        /// </summary>
        public Boolean IsActive
        {
            get { return _IsActive; }
            set
            {
                if (_IsActive != value)
                {
                    this.IsDirty = true;
                    _IsActive = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: datetime, size: , Dyna Flow Type Schedule Last UTC Date Time
        /// </summary>
        public DateTime LastUTCDateTime
        {
            get { return _LastUTCDateTime; }
            set
            {
                if (_LastUTCDateTime.ToString("yyyyMMddHHmmss") != value.ToString("yyyyMMddHHmmss"))
                {
                    this.IsDirty = true;
                    _LastUTCDateTime = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: datetime, size: , Dyna Flow Type Schedule Next UTC Date Time
        /// </summary>
        public DateTime NextUTCDateTime
        {
            get { return _NextUTCDateTime; }
            set
            {
                if (_NextUTCDateTime.ToString("yyyyMMddHHmmss") != value.ToString("yyyyMMddHHmmss"))
                {
                    this.IsDirty = true;
                    _NextUTCDateTime = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: int, size: , Dyna Flow Type Schedule Pac ID
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
        /// DB Data Type: uniqueidentifier
        /// </summary>
        public System.Guid DynaFlowTypeCodePeek
        {
            get { return _DynaFlowTypeCodePeek; }
            set
            {
                if (_DynaFlowTypeCodePeek != value)
                {
                    this.IsDirty = true;
                    _DynaFlowTypeCodePeek = value;
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

        public void Copy(DynaFlowTypeSchedule obj)
        {
            //  base.Copy(obj);
            this.IsLoaded = false;
            this.DynaFlowTypeScheduleID = obj.DynaFlowTypeScheduleID;
            this.IsObjectInvalidated = obj.IsObjectInvalidated;
             this.DynaFlowTypeID = obj.DynaFlowTypeID;
             this.FrequencyInHours = obj.FrequencyInHours;
             this.IsActive = obj.IsActive;
             this.LastUTCDateTime = obj.LastUTCDateTime;
             this.NextUTCDateTime = obj.NextUTCDateTime;
             this.PacID = obj.PacID;
            this.DynaFlowTypeCodePeek = obj.DynaFlowTypeCodePeek;
            this.PacCodePeek = obj.PacCodePeek;

            this.Code = obj.Code;
            this.LastChangeCode = obj.LastChangeCode;
            this.SetIsLoaded();
            this.IsDirty = obj.IsDirtyObject();
        }
    }
}