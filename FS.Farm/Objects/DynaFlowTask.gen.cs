using System;
using System.Data;
using System.Configuration;
using FS.Farm.Managers;
using Newtonsoft.Json.Linq;

/// <summary>
/// Summary description for DynaFlowTask
/// </summary>
namespace FS.Farm.Objects
{
    /// <summary>
    /// Dyna Flow Task Dyna Flow Task
    /// </summary>
    public partial class DynaFlowTask : FS.Base.Objects.BaseObject
    {

        public DynaFlowTask()
        {
            IsLoaded = false;
        }

        #region private vars
        int _DynaFlowTaskID;
        private DateTime _CompletedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        private Int32 _DependencyDynaFlowTaskID = 0;
        private String _Description = String.Empty;
        private Int32 _DynaFlowID = 0;
        private Guid _DynaFlowSubjectCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
        private Int32 _DynaFlowTaskTypeID = 0;
        private Boolean _IsCanceled = false;
        private Boolean _IsCancelRequested = false;
        private Boolean _IsCompleted = false;
        private Boolean _IsParallelRunAllowed = false;
        private Boolean _IsRunTaskDebugRequired = false;
        private Boolean _IsStarted = false;
        private Boolean _IsSuccessful = false;
        private Int32 _MaxRetryCount = 0;
        private DateTime _MinStartUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        private String _Param1 = String.Empty;
        private String _Param2 = String.Empty;
        private String _ProcessorIdentifier = String.Empty;
        private DateTime _RequestedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        private String _ResultValue = String.Empty;
        private Int32 _RetryCount = 0;
        private DateTime _StartedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        private System.Guid _DynaFlowCodePeek;
        private System.Guid _DynaFlowTaskTypeCodePeek;

        Guid _LastChangeCode;
        System.Guid _Code;
        #endregion

        #region Public Props

        /// <summary>
        /// primary DynaFlowTask db id
        /// DB Data type: Integer
        /// </summary>
        public int DynaFlowTaskID
        {
            get { return _DynaFlowTaskID; }
            set
            {
                this.IsDirty = true; _DynaFlowTaskID = value;
            }
        }
        /// <summary>
        /// DB Data Type: datetime, size: , Dyna Flow Task Completed UTC Date Time
        /// </summary>
        public DateTime CompletedUTCDateTime
        {
            get { return _CompletedUTCDateTime; }
            set
            {
                if (_CompletedUTCDateTime.ToString("yyyyMMddHHmmss") != value.ToString("yyyyMMddHHmmss"))
                {
                    this.IsDirty = true;
                    _CompletedUTCDateTime = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: int, size: , Dyna Flow Task Dependency Dyna Flow Task ID
        /// </summary>
        public Int32 DependencyDynaFlowTaskID
        {
            get { return _DependencyDynaFlowTaskID; }
            set
            {
                if (_DependencyDynaFlowTaskID != value)
                {
                    this.IsDirty = true;
                    _DependencyDynaFlowTaskID = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: nvarchar, size: 500, Dyna Flow Task Description
        /// </summary>
        public String Description
        {
            get { return _Description; }
            set
            {
                if (_Description != value)
                {
                    this.IsDirty = true;
                    _Description = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: int, size: , Dyna Flow Task Dyna Flow ID
        /// </summary>
        public Int32 DynaFlowID
        {
            get { return _DynaFlowID; }
            set
            {
                if (_DynaFlowID != value)
                {
                    this.IsDirty = true;
                    _DynaFlowID = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: uniqueidentifier, size: , Dyna Flow Task Dyna Flow Subject Code
        /// </summary>
        public Guid DynaFlowSubjectCode
        {
            get { return _DynaFlowSubjectCode; }
            set
            {
                if (_DynaFlowSubjectCode != value)
                {
                    this.IsDirty = true;
                    _DynaFlowSubjectCode = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: int, size: , Dyna Flow Task Dyna Flow Task Type ID
        /// </summary>
        public Int32 DynaFlowTaskTypeID
        {
            get { return _DynaFlowTaskTypeID; }
            set
            {
                if (_DynaFlowTaskTypeID != value)
                {
                    this.IsDirty = true;
                    _DynaFlowTaskTypeID = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: bit, size: , Dyna Flow Task Is Canceled
        /// </summary>
        public Boolean IsCanceled
        {
            get { return _IsCanceled; }
            set
            {
                if (_IsCanceled != value)
                {
                    this.IsDirty = true;
                    _IsCanceled = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: bit, size: , Dyna Flow Task Is Cancel Requested
        /// </summary>
        public Boolean IsCancelRequested
        {
            get { return _IsCancelRequested; }
            set
            {
                if (_IsCancelRequested != value)
                {
                    this.IsDirty = true;
                    _IsCancelRequested = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: bit, size: , Dyna Flow Task Is Completed
        /// </summary>
        public Boolean IsCompleted
        {
            get { return _IsCompleted; }
            set
            {
                if (_IsCompleted != value)
                {
                    this.IsDirty = true;
                    _IsCompleted = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: bit, size: , Dyna Flow Task Is Parallel Run Allowed
        /// </summary>
        public Boolean IsParallelRunAllowed
        {
            get { return _IsParallelRunAllowed; }
            set
            {
                if (_IsParallelRunAllowed != value)
                {
                    this.IsDirty = true;
                    _IsParallelRunAllowed = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: bit, size: , Dyna Flow Task Is Run Task Debug Required
        /// </summary>
        public Boolean IsRunTaskDebugRequired
        {
            get { return _IsRunTaskDebugRequired; }
            set
            {
                if (_IsRunTaskDebugRequired != value)
                {
                    this.IsDirty = true;
                    _IsRunTaskDebugRequired = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: bit, size: , Dyna Flow Task Is Started
        /// </summary>
        public Boolean IsStarted
        {
            get { return _IsStarted; }
            set
            {
                if (_IsStarted != value)
                {
                    this.IsDirty = true;
                    _IsStarted = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: bit, size: , Dyna Flow Task Is Successful
        /// </summary>
        public Boolean IsSuccessful
        {
            get { return _IsSuccessful; }
            set
            {
                if (_IsSuccessful != value)
                {
                    this.IsDirty = true;
                    _IsSuccessful = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: int, size: , Dyna Flow Task Max Retry Count
        /// </summary>
        public Int32 MaxRetryCount
        {
            get { return _MaxRetryCount; }
            set
            {
                if (_MaxRetryCount != value)
                {
                    this.IsDirty = true;
                    _MaxRetryCount = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: datetime, size: , Dyna Flow Task Min Start UTC Date Time
        /// </summary>
        public DateTime MinStartUTCDateTime
        {
            get { return _MinStartUTCDateTime; }
            set
            {
                if (_MinStartUTCDateTime.ToString("yyyyMMddHHmmss") != value.ToString("yyyyMMddHHmmss"))
                {
                    this.IsDirty = true;
                    _MinStartUTCDateTime = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: nvarchar, size: 200, Dyna Flow Task Param 1
        /// </summary>
        public String Param1
        {
            get { return _Param1; }
            set
            {
                if (_Param1 != value)
                {
                    this.IsDirty = true;
                    _Param1 = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: nvarchar, size: 200, Dyna Flow Task Param 2
        /// </summary>
        public String Param2
        {
            get { return _Param2; }
            set
            {
                if (_Param2 != value)
                {
                    this.IsDirty = true;
                    _Param2 = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: nvarchar, size: 10, Dyna Flow Task Processor Identifier
        /// </summary>
        public String ProcessorIdentifier
        {
            get { return _ProcessorIdentifier; }
            set
            {
                if (_ProcessorIdentifier != value)
                {
                    this.IsDirty = true;
                    _ProcessorIdentifier = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: datetime, size: , Dyna Flow Task Requested UTC Date Time
        /// </summary>
        public DateTime RequestedUTCDateTime
        {
            get { return _RequestedUTCDateTime; }
            set
            {
                if (_RequestedUTCDateTime.ToString("yyyyMMddHHmmss") != value.ToString("yyyyMMddHHmmss"))
                {
                    this.IsDirty = true;
                    _RequestedUTCDateTime = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: nvarchar, size: 100, Dyna Flow Task Result
        /// </summary>
        public String ResultValue
        {
            get { return _ResultValue; }
            set
            {
                if (_ResultValue != value)
                {
                    this.IsDirty = true;
                    _ResultValue = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: int, size: , Dyna Flow Task Retry Count
        /// </summary>
        public Int32 RetryCount
        {
            get { return _RetryCount; }
            set
            {
                if (_RetryCount != value)
                {
                    this.IsDirty = true;
                    _RetryCount = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: datetime, size: , Dyna Flow Task Started UTC Date Time
        /// </summary>
        public DateTime StartedUTCDateTime
        {
            get { return _StartedUTCDateTime; }
            set
            {
                if (_StartedUTCDateTime.ToString("yyyyMMddHHmmss") != value.ToString("yyyyMMddHHmmss"))
                {
                    this.IsDirty = true;
                    _StartedUTCDateTime = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: uniqueidentifier
        /// </summary>
        public System.Guid DynaFlowCodePeek
        {
            get { return _DynaFlowCodePeek; }
            set
            {
                if (_DynaFlowCodePeek != value)
                {
                    this.IsDirty = true;
                    _DynaFlowCodePeek = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: uniqueidentifier
        /// </summary>
        public System.Guid DynaFlowTaskTypeCodePeek
        {
            get { return _DynaFlowTaskTypeCodePeek; }
            set
            {
                if (_DynaFlowTaskTypeCodePeek != value)
                {
                    this.IsDirty = true;
                    _DynaFlowTaskTypeCodePeek = value;
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

        public void Copy(DynaFlowTask obj)
        {
            //  base.Copy(obj);
            this.IsLoaded = false;
            this.DynaFlowTaskID = obj.DynaFlowTaskID;
            this.IsObjectInvalidated = obj.IsObjectInvalidated;
             this.CompletedUTCDateTime = obj.CompletedUTCDateTime;
             this.DependencyDynaFlowTaskID = obj.DependencyDynaFlowTaskID;
             this.Description = obj.Description;
             this.DynaFlowID = obj.DynaFlowID;
             this.DynaFlowSubjectCode = obj.DynaFlowSubjectCode;
             this.DynaFlowTaskTypeID = obj.DynaFlowTaskTypeID;
             this.IsCanceled = obj.IsCanceled;
             this.IsCancelRequested = obj.IsCancelRequested;
             this.IsCompleted = obj.IsCompleted;
             this.IsParallelRunAllowed = obj.IsParallelRunAllowed;
             this.IsRunTaskDebugRequired = obj.IsRunTaskDebugRequired;
             this.IsStarted = obj.IsStarted;
             this.IsSuccessful = obj.IsSuccessful;
             this.MaxRetryCount = obj.MaxRetryCount;
             this.MinStartUTCDateTime = obj.MinStartUTCDateTime;
             this.Param1 = obj.Param1;
             this.Param2 = obj.Param2;
             this.ProcessorIdentifier = obj.ProcessorIdentifier;
             this.RequestedUTCDateTime = obj.RequestedUTCDateTime;
             this.ResultValue = obj.ResultValue;
             this.RetryCount = obj.RetryCount;
             this.StartedUTCDateTime = obj.StartedUTCDateTime;
            this.DynaFlowCodePeek = obj.DynaFlowCodePeek;
            this.DynaFlowTaskTypeCodePeek = obj.DynaFlowTaskTypeCodePeek;

            this.Code = obj.Code;
            this.LastChangeCode = obj.LastChangeCode;
            this.SetIsLoaded();
            this.IsDirty = obj.IsDirtyObject();
        }
    }
}