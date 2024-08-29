using System;
using System.Data;
using System.Configuration;
using FS.Farm.Managers;
using Newtonsoft.Json.Linq;

/// <summary>
/// Summary description for DFTDependency
/// </summary>
namespace FS.Farm.Objects
{
    /// <summary>
    /// DFT Dependency DFT Dependency
    /// </summary>
    public partial class DFTDependency : FS.Base.Objects.BaseObject
    {

        public DFTDependency()
        {
            IsLoaded = false;
        }

        #region private vars
        int _DFTDependencyID;
        private Int32 _DependencyDFTaskID = 0;
        private Int32 _DynaFlowTaskID = 0;
        private Boolean _IsPlaceholder = false;
        private System.Guid _DynaFlowTaskCodePeek;

        Guid _LastChangeCode;
        System.Guid _Code;
        #endregion

        #region Public Props

        /// <summary>
        /// primary DFTDependency db id
        /// DB Data type: Integer
        /// </summary>
        public int DFTDependencyID
        {
            get { return _DFTDependencyID; }
            set
            {
                this.IsDirty = true; _DFTDependencyID = value;
            }
        }
        /// <summary>
        /// DB Data Type: int, size: , DFT Dependency Dependency DF Task ID
        /// </summary>
        public Int32 DependencyDFTaskID
        {
            get { return _DependencyDFTaskID; }
            set
            {
                if (_DependencyDFTaskID != value)
                {
                    this.IsDirty = true;
                    _DependencyDFTaskID = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: int, size: , DFT Dependency Dyna Flow Task ID
        /// </summary>
        public Int32 DynaFlowTaskID
        {
            get { return _DynaFlowTaskID; }
            set
            {
                if (_DynaFlowTaskID != value)
                {
                    this.IsDirty = true;
                    _DynaFlowTaskID = value;
                }
            }
        }
        /// <summary>
        /// DB Data Type: bit, size: , DFT Dependency Is Placeholder
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
        /// DB Data Type: uniqueidentifier
        /// </summary>
        public System.Guid DynaFlowTaskCodePeek
        {
            get { return _DynaFlowTaskCodePeek; }
            set
            {
                if (_DynaFlowTaskCodePeek != value)
                {
                    this.IsDirty = true;
                    _DynaFlowTaskCodePeek = value;
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

        public void Copy(DFTDependency obj)
        {
            //  base.Copy(obj);
            this.IsLoaded = false;
            this.DFTDependencyID = obj.DFTDependencyID;
            this.IsObjectInvalidated = obj.IsObjectInvalidated;
             this.DependencyDFTaskID = obj.DependencyDFTaskID;
             this.DynaFlowTaskID = obj.DynaFlowTaskID;
             this.IsPlaceholder = obj.IsPlaceholder;
            this.DynaFlowTaskCodePeek = obj.DynaFlowTaskCodePeek;

            this.Code = obj.Code;
            this.LastChangeCode = obj.LastChangeCode;
            this.SetIsLoaded();
            this.IsDirty = obj.IsDirtyObject();
        }
    }
}