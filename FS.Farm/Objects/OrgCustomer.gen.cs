using System;
using System.Data;
using System.Configuration;
using FS.Farm.Managers;
using Newtonsoft.Json.Linq;
/// <summary>
/// Summary description for OrgCustomer
/// </summary>
namespace FS.Farm.Objects
{
    /// <summary>
    /// Org Customer Org Customer
    /// </summary>
    public partial class OrgCustomer : FS.Base.Objects.BaseObject
    {
        public OrgCustomer()
        {
            IsLoaded = false;
        }
        #region private vars
        int _OrgCustomerID;
        private Int32 _CustomerID = 0;
        private String _Email = String.Empty;
        private Int32 _OrganizationID = 0;
        private System.Guid _CustomerCodePeek;
        private System.Guid _OrganizationCodePeek;
        Guid _LastChangeCode;
        System.Guid _Code;
        #endregion
        #region Public Props
        /// <summary>
        /// primary OrgCustomer db id
        /// DB Data type: Integer
        /// </summary>
        public int OrgCustomerID
        {
            get { return _OrgCustomerID; }
            set
            {
                this.IsDirty = true; _OrgCustomerID = value;
            }
        }
        /// <summary>
        /// DB Data Type: int, size: , Org Customer Customer ID
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
        /// DB Data Type: nvarchar, size: 100, Org Customer Email
        /// </summary>
        public String Email
        {
            get { return _Email; }
            set
            {
                if (_Email != value)
                {
                    this.IsDirty = true;
                    _Email = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: int, size: , Org Customer Organization ID
        /// </summary>
        public Int32 OrganizationID
        {
            get { return _OrganizationID; }
            set
            {
                if (_OrganizationID != value)
                {
                    this.IsDirty = true;
                    _OrganizationID = value;
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
        public System.Guid OrganizationCodePeek
        {
            get { return _OrganizationCodePeek; }
            set
            {
                if (_OrganizationCodePeek != value)
                {
                    this.IsDirty = true;
                    _OrganizationCodePeek = value;
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
        public void Copy(OrgCustomer obj)
        {
            //  base.Copy(obj);
            this.IsLoaded = false;
            this.OrgCustomerID = obj.OrgCustomerID;
            this.IsObjectInvalidated = obj.IsObjectInvalidated;
             this.CustomerID = obj.CustomerID;
             this.Email = obj.Email;
             this.OrganizationID = obj.OrganizationID;
            this.CustomerCodePeek = obj.CustomerCodePeek;
            this.OrganizationCodePeek = obj.OrganizationCodePeek;
            this.Code = obj.Code;
            this.LastChangeCode = obj.LastChangeCode;
            this.SetIsLoaded();
            this.IsDirty = obj.IsDirtyObject();
        }
    }
}