using System;
using System.Data;
using System.Configuration;
using FS.Farm.Managers;
using Newtonsoft.Json.Linq;
/// <summary>
/// Summary description for Organization
/// </summary>
namespace FS.Farm.Objects
{
    /// <summary>
    /// Organization Organization
    /// </summary>
    public partial class Organization : FS.Base.Objects.BaseObject
    {
        public Organization()
        {
            IsLoaded = false;
        }
        #region private vars
        int _OrganizationID;
        private String _Name = String.Empty;
        private Int32 _TacID = 0;
        private System.Guid _TacCodePeek;
        Guid _LastChangeCode;
        System.Guid _Code;
        #endregion
        #region Public Props
        /// <summary>
        /// primary Organization db id
        /// DB Data type: Integer
        /// </summary>
        public int OrganizationID
        {
            get { return _OrganizationID; }
            set
            {
                this.IsDirty = true; _OrganizationID = value;
            }
        }
        /// <summary>
        /// DB Data Type: nvarchar, size: 100, Organization Name
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
        /// DB Data Type: int, size: , Organization Tac ID
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
        public void Copy(Organization obj)
        {
            //  base.Copy(obj);
            this.IsLoaded = false;
            this.OrganizationID = obj.OrganizationID;
            this.IsObjectInvalidated = obj.IsObjectInvalidated;
             this.Name = obj.Name;
             this.TacID = obj.TacID;
            this.TacCodePeek = obj.TacCodePeek;
            this.Code = obj.Code;
            this.LastChangeCode = obj.LastChangeCode;
            this.SetIsLoaded();
            this.IsDirty = obj.IsDirtyObject();
        }
    }
}