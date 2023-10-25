using System;
using System.Data;
using System.Configuration;
using FS.Farm.Managers;
using Newtonsoft.Json.Linq;
/// <summary>
/// Summary description for Customer
/// </summary>
namespace FS.Farm.Objects
{
    /// <summary>
    /// Customer Customer
    /// </summary>
    public partial class Customer : FS.Base.Objects.BaseObject
    {
        public Customer()
        {
            IsLoaded = false;
        }
        #region private vars
        int _CustomerID;
        private Int32 _ActiveOrganizationID = 0;
        private String _Email = String.Empty;
        private DateTime _EmailConfirmedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        private String _FirstName = String.Empty;
        private DateTime _ForgotPasswordKeyExpirationUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        private String _ForgotPasswordKeyValue = String.Empty;
        private Guid _FSUserCodeValue = Guid.Parse("00000000-0000-0000-0000-000000000000");
        private Boolean _IsActive = false;
        private Boolean _IsEmailAllowed = false;
        private Boolean _IsEmailConfirmed = false;
        private Boolean _IsEmailMarketingAllowed = false;
        private Boolean _IsLocked = false;
        private Boolean _IsMultipleOrganizationsAllowed = false;
        private Boolean _IsVerboseLoggingForced = false;
        private DateTime _LastLoginUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        private String _LastName = String.Empty;
        private String _Password = String.Empty;
        private String _Phone = String.Empty;
        private String _Province = String.Empty;
        private DateTime _RegistrationUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        private Int32 _TacID = 0;
        private Int32 _UTCOffsetInMinutes = 0;
        private String _Zip = String.Empty;
        private System.Guid _TacCodePeek;
        Guid _LastChangeCode;
        System.Guid _Code;
        #endregion
        #region Public Props
        /// <summary>
        /// primary Customer db id
        /// DB Data type: Integer
        /// </summary>
        public int CustomerID
        {
            get { return _CustomerID; }
            set
            {
                this.IsDirty = true; _CustomerID = value;
            }
        }
        /// <summary>
        /// DB Data Type: int, size: , Customer Active Organization ID
        /// </summary>
        public Int32 ActiveOrganizationID
        {
            get { return _ActiveOrganizationID; }
            set
            {
                if (_ActiveOrganizationID != value)
                {
                    this.IsDirty = true;
                    _ActiveOrganizationID = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: nvarchar, size: 100, Customer Email
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
        /// DB Data Type: datetime, size: , Customer Email Confirmed UTC Date Time
        /// </summary>
        public DateTime EmailConfirmedUTCDateTime
        {
            get { return _EmailConfirmedUTCDateTime; }
            set
            {
                if (_EmailConfirmedUTCDateTime.ToString("yyyyMMddHHmmss") != value.ToString("yyyyMMddHHmmss"))
                {
                    this.IsDirty = true;
                    _EmailConfirmedUTCDateTime = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: nvarchar, size: 200, Customer First Name
        /// </summary>
        public String FirstName
        {
            get { return _FirstName; }
            set
            {
                if (_FirstName != value)
                {
                    this.IsDirty = true;
                    _FirstName = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: datetime, size: , Customer Forgot Password Key Expiration UTC Date Time
        /// </summary>
        public DateTime ForgotPasswordKeyExpirationUTCDateTime
        {
            get { return _ForgotPasswordKeyExpirationUTCDateTime; }
            set
            {
                if (_ForgotPasswordKeyExpirationUTCDateTime.ToString("yyyyMMddHHmmss") != value.ToString("yyyyMMddHHmmss"))
                {
                    this.IsDirty = true;
                    _ForgotPasswordKeyExpirationUTCDateTime = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: nvarchar, size: 1000, Customer Forgot Password Key Value
        /// </summary>
        public String ForgotPasswordKeyValue
        {
            get { return _ForgotPasswordKeyValue; }
            set
            {
                if (_ForgotPasswordKeyValue != value)
                {
                    this.IsDirty = true;
                    _ForgotPasswordKeyValue = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: uniqueidentifier, size: , Customer FS User Code Value
        /// </summary>
        public Guid FSUserCodeValue
        {
            get { return _FSUserCodeValue; }
            set
            {
                if (_FSUserCodeValue != value)
                {
                    this.IsDirty = true;
                    _FSUserCodeValue = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: bit, size: , Customer Is Active
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
        /// DB Data Type: bit, size: , Customer Is Email Allowed
        /// </summary>
        public Boolean IsEmailAllowed
        {
            get { return _IsEmailAllowed; }
            set
            {
                if (_IsEmailAllowed != value)
                {
                    this.IsDirty = true;
                    _IsEmailAllowed = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: bit, size: , Customer Is Email Confirmed
        /// </summary>
        public Boolean IsEmailConfirmed
        {
            get { return _IsEmailConfirmed; }
            set
            {
                if (_IsEmailConfirmed != value)
                {
                    this.IsDirty = true;
                    _IsEmailConfirmed = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: bit, size: , Customer Is Email Marketing Allowed
        /// </summary>
        public Boolean IsEmailMarketingAllowed
        {
            get { return _IsEmailMarketingAllowed; }
            set
            {
                if (_IsEmailMarketingAllowed != value)
                {
                    this.IsDirty = true;
                    _IsEmailMarketingAllowed = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: bit, size: , Customer Is Locked
        /// </summary>
        public Boolean IsLocked
        {
            get { return _IsLocked; }
            set
            {
                if (_IsLocked != value)
                {
                    this.IsDirty = true;
                    _IsLocked = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: bit, size: , Customer Is Multiple Organizations Allowed
        /// </summary>
        public Boolean IsMultipleOrganizationsAllowed
        {
            get { return _IsMultipleOrganizationsAllowed; }
            set
            {
                if (_IsMultipleOrganizationsAllowed != value)
                {
                    this.IsDirty = true;
                    _IsMultipleOrganizationsAllowed = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: bit, size: , Customer Is Verbose Logging Forced
        /// </summary>
        public Boolean IsVerboseLoggingForced
        {
            get { return _IsVerboseLoggingForced; }
            set
            {
                if (_IsVerboseLoggingForced != value)
                {
                    this.IsDirty = true;
                    _IsVerboseLoggingForced = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: datetime, size: , Customer Last Login UTC Date Time
        /// </summary>
        public DateTime LastLoginUTCDateTime
        {
            get { return _LastLoginUTCDateTime; }
            set
            {
                if (_LastLoginUTCDateTime.ToString("yyyyMMddHHmmss") != value.ToString("yyyyMMddHHmmss"))
                {
                    this.IsDirty = true;
                    _LastLoginUTCDateTime = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: nvarchar, size: 200, Customer Last Name
        /// </summary>
        public String LastName
        {
            get { return _LastName; }
            set
            {
                if (_LastName != value)
                {
                    this.IsDirty = true;
                    _LastName = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: nvarchar, size: 100, Customer Password
        /// </summary>
        public String Password
        {
            get { return _Password; }
            set
            {
                if (_Password != value)
                {
                    this.IsDirty = true;
                    _Password = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: nvarchar, size: 50, Customer Phone
        /// </summary>
        public String Phone
        {
            get { return _Phone; }
            set
            {
                if (_Phone != value)
                {
                    this.IsDirty = true;
                    _Phone = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: nvarchar, size: 50, Customer Province
        /// </summary>
        public String Province
        {
            get { return _Province; }
            set
            {
                if (_Province != value)
                {
                    this.IsDirty = true;
                    _Province = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: datetime, size: , Customer Registration UTC Date Time
        /// </summary>
        public DateTime RegistrationUTCDateTime
        {
            get { return _RegistrationUTCDateTime; }
            set
            {
                if (_RegistrationUTCDateTime.ToString("yyyyMMddHHmmss") != value.ToString("yyyyMMddHHmmss"))
                {
                    this.IsDirty = true;
                    _RegistrationUTCDateTime = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: int, size: , Customer Tac ID
        /// </summary>
        public Int32 TacID
        {
            get { return _TacID; }
            set
            {
                if (_TacID != value)
                {
                    this.IsDirty = true;
                    _TacID = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: int, size: , Customer UTC Offset In Minutes
        /// </summary>
        public Int32 UTCOffsetInMinutes
        {
            get { return _UTCOffsetInMinutes; }
            set
            {
                if (_UTCOffsetInMinutes != value)
                {
                    this.IsDirty = true;
                    _UTCOffsetInMinutes = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: nvarchar, size: 200, Customer Zip
        /// </summary>
        public String Zip
        {
            get { return _Zip; }
            set
            {
                if (_Zip != value)
                {
                    this.IsDirty = true;
                    _Zip = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: uniqueidentifier
        /// </summary>
        public System.Guid TacCodePeek
        {
            get { return _TacCodePeek; }
            set
            {
                if (_TacCodePeek != value)
                {
                    this.IsDirty = true;
                    _TacCodePeek = value;
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
        public void Copy(Customer obj)
        {
            //  base.Copy(obj);
            this.IsLoaded = false;
            this.CustomerID = obj.CustomerID;
            this.IsObjectInvalidated = obj.IsObjectInvalidated;
             this.ActiveOrganizationID = obj.ActiveOrganizationID;
             this.Email = obj.Email;
             this.EmailConfirmedUTCDateTime = obj.EmailConfirmedUTCDateTime;
             this.FirstName = obj.FirstName;
             this.ForgotPasswordKeyExpirationUTCDateTime = obj.ForgotPasswordKeyExpirationUTCDateTime;
             this.ForgotPasswordKeyValue = obj.ForgotPasswordKeyValue;
             this.FSUserCodeValue = obj.FSUserCodeValue;
             this.IsActive = obj.IsActive;
             this.IsEmailAllowed = obj.IsEmailAllowed;
             this.IsEmailConfirmed = obj.IsEmailConfirmed;
             this.IsEmailMarketingAllowed = obj.IsEmailMarketingAllowed;
             this.IsLocked = obj.IsLocked;
             this.IsMultipleOrganizationsAllowed = obj.IsMultipleOrganizationsAllowed;
             this.IsVerboseLoggingForced = obj.IsVerboseLoggingForced;
             this.LastLoginUTCDateTime = obj.LastLoginUTCDateTime;
             this.LastName = obj.LastName;
             this.Password = obj.Password;
             this.Phone = obj.Phone;
             this.Province = obj.Province;
             this.RegistrationUTCDateTime = obj.RegistrationUTCDateTime;
             this.TacID = obj.TacID;
             this.UTCOffsetInMinutes = obj.UTCOffsetInMinutes;
             this.Zip = obj.Zip;
            this.TacCodePeek = obj.TacCodePeek;
            this.Code = obj.Code;
            this.LastChangeCode = obj.LastChangeCode;
            this.SetIsLoaded();
            this.IsDirty = obj.IsDirtyObject();
        }
    }
}