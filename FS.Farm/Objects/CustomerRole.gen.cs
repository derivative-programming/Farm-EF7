using System;
using System.Data;
using System.Configuration;
using FS.Farm.Managers;
using Newtonsoft.Json.Linq;
/// <summary>
/// Summary description for CustomerRole
/// </summary>
namespace FS.Farm.Objects
{
    /// <summary>
    /// Customer Role Customer Role
    /// </summary>
    public partial class CustomerRole : FS.Base.Objects.BaseObject
    {
        public CustomerRole()
        {
            IsLoaded = false;
        }
        #region private vars
        int _CustomerRoleID;
        private Int32 _CustomerID = 0;
        private Boolean _IsPlaceholder = false;
        private Boolean _Placeholder = false;
        private Int32 _RoleID = 0;
        private System.Guid _CustomerCodePeek;
        private System.Guid _RoleCodePeek;
        Guid _LastChangeCode;
        System.Guid _Code;
        #endregion
        #region Public Props
        /// <summary>
        /// primary CustomerRole db id
        /// DB Data type: Integer
        /// </summary>
        public int CustomerRoleID
        {
            get { return _CustomerRoleID; }
            set
            {
                this.IsDirty = true; _CustomerRoleID = value;
            }
        }
        /// <summary>
        /// DB Data Type: int, size: , Customer Role Customer ID
        /// </summary>
        public Int32 CustomerID
        {
            get { return _CustomerID; }
            set
            {
                if (_CustomerID != value)
                {
                    this.IsDirty = true;
                    _CustomerID = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: bit, size: , Customer Role Is Placeholder
        /// </summary>
        public Boolean IsPlaceholder
        {
            get { return _IsPlaceholder; }
            set
            {
                if (_IsPlaceholder != value)
                {
                    this.IsDirty = true;
                    _IsPlaceholder = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: bit, size: , Customer Role Placeholder
        /// </summary>
        public Boolean Placeholder
        {
            get { return _Placeholder; }
            set
            {
                if (_Placeholder != value)
                {
                    this.IsDirty = true;
                    _Placeholder = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: int, size: , Customer Role Role ID
        /// </summary>
        public Int32 RoleID
        {
            get { return _RoleID; }
            set
            {
                if (_RoleID != value)
                {
                    this.IsDirty = true;
                    _RoleID = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: uniqueidentifier
        /// </summary>
        public System.Guid CustomerCodePeek
        {
            get { return _CustomerCodePeek; }
            set
            {
                if (_CustomerCodePeek != value)
                {
                    this.IsDirty = true;
                    _CustomerCodePeek = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: uniqueidentifier
        /// </summary>
        public System.Guid RoleCodePeek
        {
            get { return _RoleCodePeek; }
            set
            {
                if (_RoleCodePeek != value)
                {
                    this.IsDirty = true;
                    _RoleCodePeek = value;
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
        public void Copy(CustomerRole obj)
        {
            //  base.Copy(obj);
            this.IsLoaded = false;
            this.CustomerRoleID = obj.CustomerRoleID;
            this.IsObjectInvalidated = obj.IsObjectInvalidated;
             this.CustomerID = obj.CustomerID;
             this.IsPlaceholder = obj.IsPlaceholder;
             this.Placeholder = obj.Placeholder;
             this.RoleID = obj.RoleID;
            this.CustomerCodePeek = obj.CustomerCodePeek;
            this.RoleCodePeek = obj.RoleCodePeek;
            this.Code = obj.Code;
            this.LastChangeCode = obj.LastChangeCode;
            this.SetIsLoaded();
            this.IsDirty = obj.IsDirtyObject();
        }
    }
}