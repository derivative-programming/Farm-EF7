using System;
using System.Data;
using System.Configuration;
using FS.Farm.Managers;
using Newtonsoft.Json.Linq;
/// <summary>
/// Summary description for ErrorLog
/// </summary>
namespace FS.Farm.Objects
{
    /// <summary>
    /// Error Log Error Log
    /// </summary>
    public partial class ErrorLog : FS.Base.Objects.BaseObject
    {
        public ErrorLog()
        {
            IsLoaded = false;
        }
        #region private vars
        int _ErrorLogID;
        private Guid _BrowserCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
        private Guid _ContextCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
        private DateTime _CreatedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        private String _Description = String.Empty;
        private Boolean _IsClientSideError = false;
        private Boolean _IsResolved = false;
        private Int32 _PacID = 0;
        private String _Url = String.Empty;
        private System.Guid _PacCodePeek;
        Guid _LastChangeCode;
        System.Guid _Code;
        #endregion
        #region Public Props
        /// <summary>
        /// primary ErrorLog db id
        /// DB Data type: Integer
        /// </summary>
        public int ErrorLogID
        {
            get { return _ErrorLogID; }
            set
            {
                this.IsDirty = true; _ErrorLogID = value;
            }
        }
        /// <summary>
        /// DB Data Type: uniqueidentifier, size: , Error Log Browser Code
        /// </summary>
        public Guid BrowserCode
        {
            get { return _BrowserCode; }
            set
            {
                if (_BrowserCode != value)
                {
                    this.IsDirty = true;
                    _BrowserCode = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: uniqueidentifier, size: , Error Log Context Code
        /// </summary>
        public Guid ContextCode
        {
            get { return _ContextCode; }
            set
            {
                if (_ContextCode != value)
                {
                    this.IsDirty = true;
                    _ContextCode = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: datetime, size: , Error Log Created UTC Date Time
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
        /// DB Data Type: nvarchar, size: Max, Error Log Description
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
        /// DB Data Type: bit, size: , Error Log Is Client Side Error
        /// </summary>
        public Boolean IsClientSideError
        {
            get { return _IsClientSideError; }
            set
            {
                if (_IsClientSideError != value)
                {
                    this.IsDirty = true;
                    _IsClientSideError = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: bit, size: , Error Log Is Resolved
        /// </summary>
        public Boolean IsResolved
        {
            get { return _IsResolved; }
            set
            {
                if (_IsResolved != value)
                {
                    this.IsDirty = true;
                    _IsResolved = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: int, size: , Error Log Pac ID
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
        /// DB Data Type: nvarchar, size: 500, Error Log Url
        /// </summary>
        public String Url
        {
            get { return _Url; }
            set
            {
                if (_Url != value)
                {
                    this.IsDirty = true;
                    _Url = value;
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
        public void Copy(ErrorLog obj)
        {
            //  base.Copy(obj);
            this.IsLoaded = false;
            this.ErrorLogID = obj.ErrorLogID;
            this.IsObjectInvalidated = obj.IsObjectInvalidated;
             this.BrowserCode = obj.BrowserCode;
             this.ContextCode = obj.ContextCode;
             this.CreatedUTCDateTime = obj.CreatedUTCDateTime;
             this.Description = obj.Description;
             this.IsClientSideError = obj.IsClientSideError;
             this.IsResolved = obj.IsResolved;
             this.PacID = obj.PacID;
             this.Url = obj.Url;
            this.PacCodePeek = obj.PacCodePeek;
            this.Code = obj.Code;
            this.LastChangeCode = obj.LastChangeCode;
            this.SetIsLoaded();
            this.IsDirty = obj.IsDirtyObject();
        }
    }
}