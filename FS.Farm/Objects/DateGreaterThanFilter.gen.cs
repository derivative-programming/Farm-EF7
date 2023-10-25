using System;
using System.Data;
using System.Configuration;
using FS.Farm.Managers;
using Newtonsoft.Json.Linq;
/// <summary>
/// Summary description for DateGreaterThanFilter
/// </summary>
namespace FS.Farm.Objects
{
    /// <summary>
    /// Date Greater Than Filter Date Greater Than Filter
    /// </summary>
    public partial class DateGreaterThanFilter : FS.Base.Objects.BaseObject
    {
        public DateGreaterThanFilter()
        {
            IsLoaded = false;
        }
        #region private vars
        int _DateGreaterThanFilterID;
        private Int32 _DayCount = 0;
        private String _Description = String.Empty;
        private Int32 _DisplayOrder = 0;
        private Boolean _IsActive = false;
        private String _LookupEnumName = String.Empty;
        private String _Name = String.Empty;
        private Int32 _PacID = 0;
        private System.Guid _PacCodePeek;
        Guid _LastChangeCode;
        System.Guid _Code;
        #endregion
        #region Public Props
        /// <summary>
        /// primary DateGreaterThanFilter db id
        /// DB Data type: Integer
        /// </summary>
        public int DateGreaterThanFilterID
        {
            get { return _DateGreaterThanFilterID; }
            set
            {
                this.IsDirty = true; _DateGreaterThanFilterID = value;
            }
        }
        /// <summary>
        /// DB Data Type: int, size: , Date Greater Than Filter Day Count
        /// </summary>
        public Int32 DayCount
        {
            get { return _DayCount; }
            set
            {
                if (_DayCount != value)
                {
                    this.IsDirty = true;
                    _DayCount = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: nvarchar, size: 500, Date Greater Than Filter Description
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
        /// DB Data Type: int, size: , Date Greater Than Filter Display Order
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
        /// DB Data Type: bit, size: , Date Greater Than Filter Is Active
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
        /// DB Data Type: nvarchar, size: 50, Date Greater Than Filter Lookup Enum Name
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
        /// DB Data Type: nvarchar, size: 100, Date Greater Than Filter Name
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
        /// DB Data Type: int, size: , Date Greater Than Filter Pac ID
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
        public void Copy(DateGreaterThanFilter obj)
        {
            //  base.Copy(obj);
            this.IsLoaded = false;
            this.DateGreaterThanFilterID = obj.DateGreaterThanFilterID;
            this.IsObjectInvalidated = obj.IsObjectInvalidated;
             this.DayCount = obj.DayCount;
             this.Description = obj.Description;
             this.DisplayOrder = obj.DisplayOrder;
             this.IsActive = obj.IsActive;
             this.LookupEnumName = obj.LookupEnumName;
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