using System;
using System.Data;
using System.Configuration;
using FS.Farm.Managers;
using Newtonsoft.Json.Linq;

/// <summary>
/// Summary description for DynaFlow
/// </summary>
namespace FS.Farm.Objects
{
    /// <summary>
    /// Dyna Flow Dyna Flow
    /// </summary>
    public partial class DynaFlow : FS.Base.Objects.BaseObject
    {

        public DynaFlow()
        {
            IsLoaded = false;
        }

        #region private vars
        int _DynaFlowID;
        private DateTime _CompletedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        private Int32 _DependencyDynaFlowID = 0;
        private String _Description = String.Empty;
        private Int32 _DynaFlowTypeID = 0;
        private Boolean _IsBuildTaskDebugRequired = false;
        private Boolean _IsCanceled = false;
        private Boolean _IsCancelRequested = false;
        private Boolean _IsCompleted = false;
        private Boolean _IsPaused = false;
        private Boolean _IsResubmitted = false;
        private Boolean _IsRunTaskDebugRequired = false;
        private Boolean _IsStarted = false;
        private Boolean _IsSuccessful = false;
        private Boolean _IsTaskCreationStarted = false;
        private Boolean _IsTasksCreated = false;
        private DateTime _MinStartUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        private Int32 _PacID = 0;
        private String _Param1 = String.Empty;
        private Int32 _ParentDynaFlowID = 0;
        private Int32 _PriorityLevel = 0;
        private DateTime _RequestedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        private String _ResultValue = String.Empty;
        private Int32 _RootDynaFlowID = 0;
        private DateTime _StartedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        private Guid _SubjectCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
        private String _TaskCreationProcessorIdentifier = String.Empty;
        private System.Guid _DynaFlowTypeCodePeek;
        private System.Guid _PacCodePeek;

        Guid _LastChangeCode;
        System.Guid _Code;
        #endregion

        #region Public Props

        /// <summary>
        /// primary DynaFlow db id
        /// DB Data type: Integer
        /// </summary>
        public int DynaFlowID
        {
            get { return _DynaFlowID; }
            set
            {
                this.IsDirty = true; _DynaFlowID = value;
            }
        }
        /// <summary>
        /// DB Data Type: datetime, size: , Dyna Flow Completed UTC Date Time
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
        /// DB Data Type: int, size: , Dyna Flow Dependency Dyna Flow ID
        /// </summary>
        public Int32 DependencyDynaFlowID
        {
            get { return _DependencyDynaFlowID; }
            set
            {
                if (_DependencyDynaFlowID != value)
                {
                    this.IsDirty = true;
                    _DependencyDynaFlowID = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: nvarchar, size: 500, Dyna Flow Description
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
        /// DB Data Type: int, size: , Dyna Flow Dyna Flow Type ID
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
        /// DB Data Type: bit, size: , Dyna Flow Is Build Task Debug Required
        /// </summary>
        public Boolean IsBuildTaskDebugRequired
        {
            get { return _IsBuildTaskDebugRequired; }
            set
            {
                if (_IsBuildTaskDebugRequired != value)
                {
                    this.IsDirty = true;
                    _IsBuildTaskDebugRequired = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: bit, size: , Dyna Flow Is Canceled
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
        /// DB Data Type: bit, size: , Dyna Flow Is Cancel Requested
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
        /// DB Data Type: bit, size: , Dyna Flow Is Completed
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
        /// DB Data Type: bit, size: , Dyna Flow Is Paused
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
        /// DB Data Type: bit, size: , Dyna Flow Is Resubmitted
        /// </summary>
        public Boolean IsResubmitted
        {
            get { return _IsResubmitted; }
            set
            {
                if (_IsResubmitted != value)
                {
                    this.IsDirty = true;
                    _IsResubmitted = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: bit, size: , Dyna Flow Is Run Task Debug Required
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
        /// DB Data Type: bit, size: , Dyna Flow Is Started
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
        /// DB Data Type: bit, size: , Dyna Flow Is Successful
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
        /// DB Data Type: bit, size: , Dyna Flow Is Task Creation Started
        /// </summary>
        public Boolean IsTaskCreationStarted
        {
            get { return _IsTaskCreationStarted; }
            set
            {
                if (_IsTaskCreationStarted != value)
                {
                    this.IsDirty = true;
                    _IsTaskCreationStarted = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: bit, size: , Dyna Flow Is Tasks Created
        /// </summary>
        public Boolean IsTasksCreated
        {
            get { return _IsTasksCreated; }
            set
            {
                if (_IsTasksCreated != value)
                {
                    this.IsDirty = true;
                    _IsTasksCreated = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: datetime, size: , Dyna Flow Min Start UTC Date Time
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
        /// DB Data Type: int, size: , Dyna Flow Pac ID
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
        /// DB Data Type: nvarchar, size: 100, Dyna Flow Param 1
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
        /// DB Data Type: int, size: , Dyna Flow Parent Dyna Flow ID
        /// </summary>
        public Int32 ParentDynaFlowID
        {
            get { return _ParentDynaFlowID; }
            set
            {
                if (_ParentDynaFlowID != value)
                {
                    this.IsDirty = true;
                    _ParentDynaFlowID = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: int, size: , Dyna Flow Priority Level
        /// </summary>
        public Int32 PriorityLevel
        {
            get { return _PriorityLevel; }
            set
            {
                if (_PriorityLevel != value)
                {
                    this.IsDirty = true;
                    _PriorityLevel = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: datetime, size: , Dyna Flow Requested UTC Date Time
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
        /// DB Data Type: nvarchar, size: 100, Dyna Flow Result
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
        /// DB Data Type: int, size: , Dyna Flow Root Dyna Flow ID
        /// </summary>
        public Int32 RootDynaFlowID
        {
            get { return _RootDynaFlowID; }
            set
            {
                if (_RootDynaFlowID != value)
                {
                    this.IsDirty = true;
                    _RootDynaFlowID = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: datetime, size: , Dyna Flow Started UTC Date Time
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
        /// DB Data Type: uniqueidentifier, size: , Dyna Flow Subject Code
        /// </summary>
        public Guid SubjectCode
        {
            get { return _SubjectCode; }
            set
            {
                if (_SubjectCode != value)
                {
                    this.IsDirty = true;
                    _SubjectCode = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: nvarchar, size: 10, Dyna Flow Task Creation Processor Identifier
        /// </summary>
        public String TaskCreationProcessorIdentifier
        {
            get { return _TaskCreationProcessorIdentifier; }
            set
            {
                if (_TaskCreationProcessorIdentifier != value)
                {
                    this.IsDirty = true;
                    _TaskCreationProcessorIdentifier = value;
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

        public void Copy(DynaFlow obj)
        {
            //  base.Copy(obj);
            this.IsLoaded = false;
            this.DynaFlowID = obj.DynaFlowID;
            this.IsObjectInvalidated = obj.IsObjectInvalidated;
             this.CompletedUTCDateTime = obj.CompletedUTCDateTime;
             this.DependencyDynaFlowID = obj.DependencyDynaFlowID;
             this.Description = obj.Description;
             this.DynaFlowTypeID = obj.DynaFlowTypeID;
             this.IsBuildTaskDebugRequired = obj.IsBuildTaskDebugRequired;
             this.IsCanceled = obj.IsCanceled;
             this.IsCancelRequested = obj.IsCancelRequested;
             this.IsCompleted = obj.IsCompleted;
             this.IsPaused = obj.IsPaused;
             this.IsResubmitted = obj.IsResubmitted;
             this.IsRunTaskDebugRequired = obj.IsRunTaskDebugRequired;
             this.IsStarted = obj.IsStarted;
             this.IsSuccessful = obj.IsSuccessful;
             this.IsTaskCreationStarted = obj.IsTaskCreationStarted;
             this.IsTasksCreated = obj.IsTasksCreated;
             this.MinStartUTCDateTime = obj.MinStartUTCDateTime;
             this.PacID = obj.PacID;
             this.Param1 = obj.Param1;
             this.ParentDynaFlowID = obj.ParentDynaFlowID;
             this.PriorityLevel = obj.PriorityLevel;
             this.RequestedUTCDateTime = obj.RequestedUTCDateTime;
             this.ResultValue = obj.ResultValue;
             this.RootDynaFlowID = obj.RootDynaFlowID;
             this.StartedUTCDateTime = obj.StartedUTCDateTime;
             this.SubjectCode = obj.SubjectCode;
             this.TaskCreationProcessorIdentifier = obj.TaskCreationProcessorIdentifier;
            this.DynaFlowTypeCodePeek = obj.DynaFlowTypeCodePeek;
            this.PacCodePeek = obj.PacCodePeek;

            this.Code = obj.Code;
            this.LastChangeCode = obj.LastChangeCode;
            this.SetIsLoaded();
            this.IsDirty = obj.IsDirtyObject();
        }
    }
}