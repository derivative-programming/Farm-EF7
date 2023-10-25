using System;
using System.Data;
using System.Configuration;
using FS.Farm.Managers;
using Newtonsoft.Json.Linq;
/// <summary>
/// Summary description for OrgApiKey
/// </summary>
namespace FS.Farm.Objects
{
    /// <summary>
    /// Org Api Key Org Api Key
    /// </summary>
    public partial class OrgApiKey : FS.Base.Objects.BaseObject
    {
        public OrgApiKey()
        {
            IsLoaded = false;
        }
        #region private vars
        int _OrgApiKeyID;
        private String _ApiKeyValue = String.Empty;
        private String _CreatedBy = String.Empty;
        private DateTime _CreatedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        private DateTime _ExpirationUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        private Boolean _IsActive = false;
        private Boolean _IsTempUserKey = false;
        private String _Name = String.Empty;
        private Int32 _OrganizationID = 0;
        private Int32 _OrgCustomerID = 0;
        private System.Guid _OrganizationCodePeek;
        private System.Guid _OrgCustomerCodePeek;
        Guid _LastChangeCode;
        System.Guid _Code;
        #endregion
        #region Public Props
        /// <summary>
        /// primary OrgApiKey db id
        /// DB Data type: Integer
        /// </summary>
        public int OrgApiKeyID
        {
            get { return _OrgApiKeyID; }
            set
            {
                this.IsDirty = true; _OrgApiKeyID = value;
            }
        }
        /// <summary>
        /// DB Data Type: nvarchar, size: 4000, Org Api Key Api Key Value
        /// </summary>
        public String ApiKeyValue
        {
            get { return _ApiKeyValue; }
            set
            {
                if (_ApiKeyValue != value)
                {
                    this.IsDirty = true;
                    _ApiKeyValue = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: nvarchar, size: 100, Org Api Key Created By
        /// </summary>
        public String CreatedBy
        {
            get { return _CreatedBy; }
            set
            {
                if (_CreatedBy != value)
                {
                    this.IsDirty = true;
                    _CreatedBy = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: datetime, size: , Org Api Key Created UTC Date Time
        /// </summary>
        public DateTime CreatedUTCDateTime
        {
            get { return _CreatedUTCDateTime; }
            set
            {
                if (_CreatedUTCDateTime.ToString("yyyyMMddHHmmss") != value.ToString("yyyyMMddHHmmss"))
                {
                    this.IsDirty = true;
                    _CreatedUTCDateTime = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: datetime, size: , Org Api Key Expiration UTC Date Time
        /// </summary>
        public DateTime ExpirationUTCDateTime
        {
            get { return _ExpirationUTCDateTime; }
            set
            {
                if (_ExpirationUTCDateTime.ToString("yyyyMMddHHmmss") != value.ToString("yyyyMMddHHmmss"))
                {
                    this.IsDirty = true;
                    _ExpirationUTCDateTime = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: bit, size: , Org Api Key Is Active
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
        /// DB Data Type: bit, size: , Org Api Key Is Temp User Key
        /// </summary>
        public Boolean IsTempUserKey
        {
            get { return _IsTempUserKey; }
            set
            {
                if (_IsTempUserKey != value)
                {
                    this.IsDirty = true;
                    _IsTempUserKey = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: nvarchar, size: 100, Org Api Key Name
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
        /// DB Data Type: int, size: , Org Api Key Organization ID
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
        /// DB Data Type: int, size: , Org Api Key Org Customer ID
        /// </summary>
        public Int32 OrgCustomerID
        {
            get { return _OrgCustomerID; }
            set
            {
                if (_OrgCustomerID != value)
                {
                    this.IsDirty = true;
                    _OrgCustomerID = value;
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
        /// DB Data Type: uniqueidentifier
        /// </summary>
        public System.Guid OrgCustomerCodePeek
        {
            get { return _OrgCustomerCodePeek; }
            set
            {
                if (_OrgCustomerCodePeek != value)
                {
                    this.IsDirty = true;
                    _OrgCustomerCodePeek = value;
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
        public void Copy(OrgApiKey obj)
        {
            //  base.Copy(obj);
            this.IsLoaded = false;
            this.OrgApiKeyID = obj.OrgApiKeyID;
            this.IsObjectInvalidated = obj.IsObjectInvalidated;
             this.ApiKeyValue = obj.ApiKeyValue;
             this.CreatedBy = obj.CreatedBy;
             this.CreatedUTCDateTime = obj.CreatedUTCDateTime;
             this.ExpirationUTCDateTime = obj.ExpirationUTCDateTime;
             this.IsActive = obj.IsActive;
             this.IsTempUserKey = obj.IsTempUserKey;
             this.Name = obj.Name;
             this.OrganizationID = obj.OrganizationID;
             this.OrgCustomerID = obj.OrgCustomerID;
            this.OrganizationCodePeek = obj.OrganizationCodePeek;
            this.OrgCustomerCodePeek = obj.OrgCustomerCodePeek;
            this.Code = obj.Code;
            this.LastChangeCode = obj.LastChangeCode;
            this.SetIsLoaded();
            this.IsDirty = obj.IsDirtyObject();
        }
    }
}