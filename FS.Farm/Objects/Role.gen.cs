using System;
using System.Data;
using System.Configuration;
using FS.Farm.Managers;
using Newtonsoft.Json.Linq;
/// <summary>
/// Summary description for Role
/// </summary>
namespace FS.Farm.Objects
{
    /// <summary>
    /// Role Role
    /// </summary>
    public partial class Role : FS.Base.Objects.BaseObject
    {
        public Role()
        {
            IsLoaded = false;
        }
        #region private vars
        int _RoleID;
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
        /// primary Role db id
        /// DB Data type: Integer
        /// </summary>
        public int RoleID
        {
            get { return _RoleID; }
            set
            {
                this.IsDirty = true; _RoleID = value;
            }
        }
        /// <summary>
        /// DB Data Type: nvarchar, size: 200, Role Description
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
        /// DB Data Type: int, size: , Role Display Order
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
        /// DB Data Type: bit, size: , Role Is Active
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
        /// DB Data Type: nvarchar, size: 50, Role Lookup Enum Name
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
        /// DB Data Type: nvarchar, size: 50, Role Name
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
        /// DB Data Type: int, size: , Role Pac ID
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
        public void Copy(Role obj)
        {
            //  base.Copy(obj);
            this.IsLoaded = false;
            this.RoleID = obj.RoleID;
            this.IsObjectInvalidated = obj.IsObjectInvalidated;
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