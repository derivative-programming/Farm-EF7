using System;
using System.Data;
using System.Configuration; 
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
    }
}