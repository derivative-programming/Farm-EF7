using System;
using System.Data;
using System.Configuration;
using FS.Farm.Managers;
using Newtonsoft.Json.Linq;
/// <summary>
/// Summary description for Plant
/// </summary>
namespace FS.Farm.Objects
{
    /// <summary>
    /// Plant Plant
    /// </summary>
    public partial class Plant : FS.Base.Objects.BaseObject
    {
        public Plant()
        {
            IsLoaded = false;
        }
        #region private vars
        int _PlantID;
        private Int32 _FlvrForeignKeyID = 0;
        private Boolean _IsDeleteAllowed = true;
        private Boolean _IsEditAllowed = true;
        private Int32 _LandID = 0;
        private String _OtherFlavor = String.Empty;
        private Int64 _SomeBigIntVal = 0;
        private Boolean _SomeBitVal = false;
        private DateTime _SomeDateVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        private Decimal _SomeDecimalVal = 0;
        private String _SomeEmailAddress = String.Empty;
        private Double _SomeFloatVal = 0;
        private Int32 _SomeIntVal = 0;
        private Decimal _SomeMoneyVal = 0;
        private String _SomeNVarCharVal = String.Empty;
        private String _SomePhoneNumber = String.Empty;
        private String _SomeTextVal = String.Empty;
        private Guid _SomeUniqueidentifierVal = Guid.Parse("00000000-0000-0000-0000-000000000000");
        private DateTime _SomeUTCDateTimeVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        private String _SomeVarCharVal = String.Empty;
        private String _someImageUrlVal = String.Empty;
        private Boolean _isImageUrlAvailable = false;
        private System.Guid _FlvrForeignKeyCodePeek;
        private System.Guid _LandCodePeek;
        Guid _LastChangeCode;
        System.Guid _Code;
        #endregion
        #region Public Props
        /// <summary>
        /// primary Plant db id
        /// DB Data type: Integer
        /// </summary>
        public int PlantID
        {
            get { return _PlantID; }
            set
            {
                this.IsDirty = true; _PlantID = value;
            }
        }
        /// <summary>
        /// DB Data Type: int, size: , Plant Flavor ID
        /// </summary>
        public Int32 FlvrForeignKeyID
        {
            get { return _FlvrForeignKeyID; }
            set
            {
                if (_FlvrForeignKeyID != value)
                {
                    this.IsDirty = true;
                    _FlvrForeignKeyID = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: bit, size: , Plant Is Delete Allowed
        /// </summary>
        public Boolean IsDeleteAllowed
        {
            get { return _IsDeleteAllowed; }
            set
            {
                if (_IsDeleteAllowed != value)
                {
                    this.IsDirty = true;
                    _IsDeleteAllowed = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: bit, size: , Plant Is Edit Allowed
        /// </summary>
        public Boolean IsEditAllowed
        {
            get { return _IsEditAllowed; }
            set
            {
                if (_IsEditAllowed != value)
                {
                    this.IsDirty = true;
                    _IsEditAllowed = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: int, size: , Plant Land ID
        /// </summary>
        public Int32 LandID
        {
            get { return _LandID; }
            set
            {
                if (_LandID != value)
                {
                    this.IsDirty = true;
                    _LandID = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: nvarchar, size: 50, Plant Other Flavor
        /// </summary>
        public String OtherFlavor
        {
            get { return _OtherFlavor; }
            set
            {
                if (_OtherFlavor != value)
                {
                    this.IsDirty = true;
                    _OtherFlavor = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: bigint, size: , Plant Some Big Int Val
        /// </summary>
        public Int64 SomeBigIntVal
        {
            get { return _SomeBigIntVal; }
            set
            {
                if (_SomeBigIntVal != value)
                {
                    this.IsDirty = true;
                    _SomeBigIntVal = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: bit, size: , Plant Some Bit Val
        /// </summary>
        public Boolean SomeBitVal
        {
            get { return _SomeBitVal; }
            set
            {
                if (_SomeBitVal != value)
                {
                    this.IsDirty = true;
                    _SomeBitVal = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: date, size: , Plant Some Date Val
        /// </summary>
        public DateTime SomeDateVal
        {
            get { return _SomeDateVal; }
            set
            {
                if (_SomeDateVal.ToString("yyyyMMddHHmmss") != value.ToString("yyyyMMddHHmmss"))
                {
                    this.IsDirty = true;
                    _SomeDateVal = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: decimal, size: , Plant Some Decimal Val
        /// </summary>
        public Decimal SomeDecimalVal
        {
            get { return _SomeDecimalVal; }
            set
            {
                if (_SomeDecimalVal != value)
                {
                    this.IsDirty = true;
                    _SomeDecimalVal = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: varchar, size: 50, Plant Some Email Address
        /// </summary>
        public String SomeEmailAddress
        {
            get { return _SomeEmailAddress; }
            set
            {
                if (_SomeEmailAddress != value)
                {
                    this.IsDirty = true;
                    _SomeEmailAddress = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: float, size: , Plant Some Float Val
        /// </summary>
        public Double SomeFloatVal
        {
            get { return _SomeFloatVal; }
            set
            {
                if (_SomeFloatVal != value)
                {
                    this.IsDirty = true;
                    _SomeFloatVal = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: int, size: , Plant Some Int Val
        /// </summary>
        public Int32 SomeIntVal
        {
            get { return _SomeIntVal; }
            set
            {
                if (_SomeIntVal != value)
                {
                    this.IsDirty = true;
                    _SomeIntVal = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: money, size: , Plant Some Money Val
        /// </summary>
        public Decimal SomeMoneyVal
        {
            get { return _SomeMoneyVal; }
            set
            {
                if (_SomeMoneyVal != value)
                {
                    this.IsDirty = true;
                    _SomeMoneyVal = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: nvarchar, size: 50, Plant Some N Var Char Val
        /// </summary>
        public String SomeNVarCharVal
        {
            get { return _SomeNVarCharVal; }
            set
            {
                if (_SomeNVarCharVal != value)
                {
                    this.IsDirty = true;
                    _SomeNVarCharVal = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: varchar, size: 50, Plant Some Phone Number
        /// </summary>
        public String SomePhoneNumber
        {
            get { return _SomePhoneNumber; }
            set
            {
                if (_SomePhoneNumber != value)
                {
                    this.IsDirty = true;
                    _SomePhoneNumber = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: text, size: , Plant Some Text Val
        /// </summary>
        public String SomeTextVal
        {
            get { return _SomeTextVal; }
            set
            {
                if (_SomeTextVal != value)
                {
                    this.IsDirty = true;
                    _SomeTextVal = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: uniqueidentifier, size: , Plant Some Uniqueidentifier Val
        /// </summary>
        public Guid SomeUniqueidentifierVal
        {
            get { return _SomeUniqueidentifierVal; }
            set
            {
                if (_SomeUniqueidentifierVal != value)
                {
                    this.IsDirty = true;
                    _SomeUniqueidentifierVal = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: datetime, size: , Plant Some UTC Date Time Val
        /// </summary>
        public DateTime SomeUTCDateTimeVal
        {
            get { return _SomeUTCDateTimeVal; }
            set
            {
                if (_SomeUTCDateTimeVal.ToString("yyyyMMddHHmmss") != value.ToString("yyyyMMddHHmmss"))
                {
                    this.IsDirty = true;
                    _SomeUTCDateTimeVal = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: varchar, size: 50, Plant Some Var Char Val
        /// </summary>
        public String SomeVarCharVal
        {
            get { return _SomeVarCharVal; }
            set
            {
                if (_SomeVarCharVal != value)
                {
                    this.IsDirty = true;
                    _SomeVarCharVal = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: varchar, size: 50, Plant Image Url Val
        /// </summary>
        public String SomeImageUrlVal
        {
            get { return _someImageUrlVal; }
            set
            {
                if (_someImageUrlVal != value)
                {
                    this.IsDirty = true;
                    _someImageUrlVal = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: bit, size: , Plant Is Delete Allowed
        /// </summary>
        public Boolean IsImageUrlAvailable
        {
            get { return _isImageUrlAvailable; }
            set
            {
                if (_isImageUrlAvailable != value)
                {
                    this.IsDirty = true;
                    _isImageUrlAvailable = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: uniqueidentifier
        /// </summary>
        public System.Guid FlvrForeignKeyCodePeek
        {
            get { return _FlvrForeignKeyCodePeek; }
            set
            {
                if (_FlvrForeignKeyCodePeek != value)
                {
                    this.IsDirty = true;
                    _FlvrForeignKeyCodePeek = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: uniqueidentifier
        /// </summary>
        public System.Guid LandCodePeek
        {
            get { return _LandCodePeek; }
            set
            {
                if (_LandCodePeek != value)
                {
                    this.IsDirty = true;
                    _LandCodePeek = value;
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
        public void Copy(Plant obj)
        {
            //  base.Copy(obj);
            this.IsLoaded = false;
            this.PlantID = obj.PlantID;
            this.IsObjectInvalidated = obj.IsObjectInvalidated;
             this.FlvrForeignKeyID = obj.FlvrForeignKeyID;
             this.IsDeleteAllowed = obj.IsDeleteAllowed;
             this.IsEditAllowed = obj.IsEditAllowed;
             this.LandID = obj.LandID;
             this.OtherFlavor = obj.OtherFlavor;
             this.SomeBigIntVal = obj.SomeBigIntVal;
             this.SomeBitVal = obj.SomeBitVal;
             this.SomeDateVal = obj.SomeDateVal;
             this.SomeDecimalVal = obj.SomeDecimalVal;
             this.SomeEmailAddress = obj.SomeEmailAddress;
             this.SomeFloatVal = obj.SomeFloatVal;
             this.SomeIntVal = obj.SomeIntVal;
             this.SomeMoneyVal = obj.SomeMoneyVal;
             this.SomeNVarCharVal = obj.SomeNVarCharVal;
             this.SomePhoneNumber = obj.SomePhoneNumber;
             this.SomeTextVal = obj.SomeTextVal;
             this.SomeUniqueidentifierVal = obj.SomeUniqueidentifierVal;
             this.SomeUTCDateTimeVal = obj.SomeUTCDateTimeVal;
             this.SomeVarCharVal = obj.SomeVarCharVal;
            this.FlvrForeignKeyCodePeek = obj.FlvrForeignKeyCodePeek;
            this.LandCodePeek = obj.LandCodePeek;
            this.Code = obj.Code;
            this.LastChangeCode = obj.LastChangeCode;
            this.SetIsLoaded();
            this.IsDirty = obj.IsDirtyObject();
        }
    }
}