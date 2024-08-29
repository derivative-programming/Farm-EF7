using System;
using System.Data;
using System.Configuration;
using FS.Farm.Managers;
using Newtonsoft.Json.Linq;

/// <summary>
/// Summary description for DynaFlowType
/// </summary>
namespace FS.Farm.Objects
{
    /// <summary>
    /// Dyna Flow Type Dyna Flow Type
    /// </summary>
    public partial class DynaFlowType : FS.Base.Objects.BaseObject
    {

        public DynaFlowType()
        {
            IsLoaded = false;
        }

        #region private vars
        int _DynaFlowTypeID;
        private String _Description = String.Empty;
        private Int32 _DisplayOrder = 0;
        private Boolean _IsActive = false;
        private String _LookupEnumName = String.Empty;
        private String _Name = String.Empty;
        private Int32 _PacID = 0;
        private Int32 _PriorityLevel = 0;
        private System.Guid _PacCodePeek;

        Guid _LastChangeCode;
        System.Guid _Code;
        #endregion

        #region Public Props

        /// <summary>
        /// primary DynaFlowType db id
        /// DB Data type: Integer
        /// </summary>
        public int DynaFlowTypeID
        {
            get { return _DynaFlowTypeID; }
            set
            {
                this.IsDirty = true; _DynaFlowTypeID = value;
            }
        }
        /// <summary>
        /// DB Data Type: nvarchar, size: 500, Dyna Flow Type Description
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
        /// DB Data Type: int, size: , Dyna Flow Type Display Order
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
        /// DB Data Type: bit, size: , Dyna Flow Type Is Active
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
        /// DB Data Type: nvarchar, size: 50, Dyna Flow Type Lookup Enum Name
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
        /// DB Data Type: nvarchar, size: 100, Dyna Flow Type Name
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
        /// DB Data Type: int, size: , Dyna Flow Type Pac ID
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
        /// DB Data Type: int, size: , Dyna Flow Type Priority Level
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

        public void Copy(DynaFlowType obj)
        {
            //  base.Copy(obj);
            this.IsLoaded = false;
            this.DynaFlowTypeID = obj.DynaFlowTypeID;
            this.IsObjectInvalidated = obj.IsObjectInvalidated;
             this.Description = obj.Description;
             this.DisplayOrder = obj.DisplayOrder;
             this.IsActive = obj.IsActive;
             this.LookupEnumName = obj.LookupEnumName;
             this.Name = obj.Name;
             this.PacID = obj.PacID;
             this.PriorityLevel = obj.PriorityLevel;
            this.PacCodePeek = obj.PacCodePeek;

            this.Code = obj.Code;
            this.LastChangeCode = obj.LastChangeCode;
            this.SetIsLoaded();
            this.IsDirty = obj.IsDirtyObject();
        }
    }
}