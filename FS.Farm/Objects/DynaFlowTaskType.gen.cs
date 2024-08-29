using System;
using System.Data;
using System.Configuration;
using FS.Farm.Managers;
using Newtonsoft.Json.Linq;

/// <summary>
/// Summary description for DynaFlowTaskType
/// </summary>
namespace FS.Farm.Objects
{
    /// <summary>
    /// Dyna Flow Task Type Dyna Flow Task Type
    /// </summary>
    public partial class DynaFlowTaskType : FS.Base.Objects.BaseObject
    {

        public DynaFlowTaskType()
        {
            IsLoaded = false;
        }

        #region private vars
        int _DynaFlowTaskTypeID;
        private String _Description = String.Empty;
        private Int32 _DisplayOrder = 0;
        private Boolean _IsActive = false;
        private String _LookupEnumName = String.Empty;
        private Int32 _MaxRetryCount = 0;
        private String _Name = String.Empty;
        private Int32 _PacID = 0;
        private System.Guid _PacCodePeek;

        Guid _LastChangeCode;
        System.Guid _Code;
        #endregion

        #region Public Props

        /// <summary>
        /// primary DynaFlowTaskType db id
        /// DB Data type: Integer
        /// </summary>
        public int DynaFlowTaskTypeID
        {
            get { return _DynaFlowTaskTypeID; }
            set
            {
                this.IsDirty = true; _DynaFlowTaskTypeID = value;
            }
        }
        /// <summary>
        /// DB Data Type: nvarchar, size: 500, Dyna Flow Task Type Description
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
        /// DB Data Type: int, size: , Dyna Flow Task Type Display Order
        /// </summary>
        public Int32 DisplayOrder
        {
            get { return _DisplayOrder; }
            set
            {
                if (_DisplayOrder != value)
                {
                    this.IsDirty = true;
                    _DisplayOrder = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: bit, size: , Dyna Flow Task Type Is Active
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
        /// DB Data Type: nvarchar, size: 50, Dyna Flow Task Type Lookup Enum Name
        /// </summary>
        public String LookupEnumName
        {
            get { return _LookupEnumName; }
            set
            {
                if (_LookupEnumName != value)
                {
                    this.IsDirty = true;
                    _LookupEnumName = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: int, size: , Dyna Flow Task Type Max Retry Count
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
        /// DB Data Type: nvarchar, size: 100, Dyna Flow Task Type Name
        /// </summary>
        public String Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    this.IsDirty = true;
                    _Name = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: int, size: , Dyna Flow Task Type Pac ID
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

        public void Copy(DynaFlowTaskType obj)
        {
            //  base.Copy(obj);
            this.IsLoaded = false;
            this.DynaFlowTaskTypeID = obj.DynaFlowTaskTypeID;
            this.IsObjectInvalidated = obj.IsObjectInvalidated;
             this.Description = obj.Description;
             this.DisplayOrder = obj.DisplayOrder;
             this.IsActive = obj.IsActive;
             this.LookupEnumName = obj.LookupEnumName;
             this.MaxRetryCount = obj.MaxRetryCount;
             this.Name = obj.Name;
             this.PacID = obj.PacID;
            this.PacCodePeek = obj.PacCodePeek;

            this.Code = obj.Code;
            this.LastChangeCode = obj.LastChangeCode;
            this.SetIsLoaded();
            this.IsDirty = obj.IsDirtyObject();
        }
    }
}