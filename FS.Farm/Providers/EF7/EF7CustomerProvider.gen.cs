using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using FS.Common.Objects;
using System.Threading.Tasks;
using FS.Farm.EF;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;
using FS.Farm.EF.Models;
using NetTopologySuite.Index.HPRtree;
using FS.Farm.Objects;
namespace FS.Farm.Providers.EF7
{
    partial class EF7CustomerProvider : FS.Farm.Providers.CustomerProvider
    {
        string _connectionString = "";
        #region Provider specific behaviors
        public override void Initialize(string name, NameValueCollection configValue)
        {
            _connectionString = configValue["connectionString"].ToString();
        }
        public override string Name
        {
            get
            {
                return null;
            }
        }
        #endregion
        #region Customer Methods
        public override int CustomerGetCount(
            SessionContext context )
        {
            string procedureName = "CustomerGetCount";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                iOut = customerManager.GetTotalCount();
            }
            catch (Exception x)
            {
                HandleError( x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return iOut;
        }
        public override async Task<int> CustomerGetCountAsync(
            SessionContext context )
        {
            string procedureName = "CustomerGetCountAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                iOut = await customerManager.GetTotalCountAsync();
            }
            catch (Exception x)
            {
                await HandleErrorAsync(context,  x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return iOut;
        }
        public override int CustomerGetMaxID(
            SessionContext context)
        {
            string procedureName = "CustomerGetMaxID";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                iOut = customerManager.GetMaxId().Value;
            }
            catch (Exception x)
            {
                HandleError( x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return iOut;
        }
        public override async Task<int> CustomerGetMaxIDAsync(
            SessionContext context)
        {
            string procedureName = "CustomerGetMaxIDAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                var maxId = await customerManager.GetMaxIdAsync();
                iOut = maxId.Value;
            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return iOut;
        }
        public override int CustomerInsert(
            SessionContext context,
            Int32 activeOrganizationID,
            String email,
            DateTime emailConfirmedUTCDateTime,
            String firstName,
            DateTime forgotPasswordKeyExpirationUTCDateTime,
            String forgotPasswordKeyValue,
            Guid fSUserCodeValue,
            Boolean isActive,
            Boolean isEmailAllowed,
            Boolean isEmailConfirmed,
            Boolean isEmailMarketingAllowed,
            Boolean isLocked,
            Boolean isMultipleOrganizationsAllowed,
            Boolean isVerboseLoggingForced,
            DateTime lastLoginUTCDateTime,
            String lastName,
            String password,
            String phone,
            String province,
            DateTime registrationUTCDateTime,
            Int32 tacID,
            Int32 uTCOffsetInMinutes,
            String zip,
            System.Guid code)
        {
            string procedureName = "CustomerInsert";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            //Int32 activeOrganizationID,
            //String email,
            if (System.Convert.ToDateTime(emailConfirmedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 emailConfirmedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //String firstName,
            if (System.Convert.ToDateTime(forgotPasswordKeyExpirationUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 forgotPasswordKeyExpirationUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //String forgotPasswordKeyValue,
            //Guid fSUserCodeValue,
            //Boolean isActive,
            //Boolean isEmailAllowed,
            //Boolean isEmailConfirmed,
            //Boolean isEmailMarketingAllowed,
            //Boolean isLocked,
            //Boolean isMultipleOrganizationsAllowed,
            //Boolean isVerboseLoggingForced,
            if (System.Convert.ToDateTime(lastLoginUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 lastLoginUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //String lastName,
            //String password,
            //String phone,
            //String province,
            if (System.Convert.ToDateTime(registrationUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 registrationUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Int32 tacID,
            //Int32 uTCOffsetInMinutes,
            //String zip,
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                EF.Models.Customer customer = new EF.Models.Customer();
                customer.Code = code;
                customer.LastChangeCode = Guid.NewGuid();
                customer.ActiveOrganizationID = activeOrganizationID;
                customer.Email = email;
                customer.EmailConfirmedUTCDateTime = emailConfirmedUTCDateTime;
                customer.FirstName = firstName;
                customer.ForgotPasswordKeyExpirationUTCDateTime = forgotPasswordKeyExpirationUTCDateTime;
                customer.ForgotPasswordKeyValue = forgotPasswordKeyValue;
                customer.FSUserCodeValue = fSUserCodeValue;
                customer.IsActive = isActive;
                customer.IsEmailAllowed = isEmailAllowed;
                customer.IsEmailConfirmed = isEmailConfirmed;
                customer.IsEmailMarketingAllowed = isEmailMarketingAllowed;
                customer.IsLocked = isLocked;
                customer.IsMultipleOrganizationsAllowed = isMultipleOrganizationsAllowed;
                customer.IsVerboseLoggingForced = isVerboseLoggingForced;
                customer.LastLoginUTCDateTime = lastLoginUTCDateTime;
                customer.LastName = lastName;
                customer.Password = password;
                customer.Phone = phone;
                customer.Province = province;
                customer.RegistrationUTCDateTime = registrationUTCDateTime;
                customer.TacID = tacID;
                customer.UTCOffsetInMinutes = uTCOffsetInMinutes;
                customer.Zip = zip;
                customer = customerManager.Add(customer);
                iOut = customer.CustomerID;
            }
            catch (Exception x)
            {
                HandleError(x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return iOut;
        }
        public override async Task<int> CustomerInsertAsync(
            SessionContext context,
            Int32 activeOrganizationID,
            String email,
            DateTime emailConfirmedUTCDateTime,
            String firstName,
            DateTime forgotPasswordKeyExpirationUTCDateTime,
            String forgotPasswordKeyValue,
            Guid fSUserCodeValue,
            Boolean isActive,
            Boolean isEmailAllowed,
            Boolean isEmailConfirmed,
            Boolean isEmailMarketingAllowed,
            Boolean isLocked,
            Boolean isMultipleOrganizationsAllowed,
            Boolean isVerboseLoggingForced,
            DateTime lastLoginUTCDateTime,
            String lastName,
            String password,
            String phone,
            String province,
            DateTime registrationUTCDateTime,
            Int32 tacID,
            Int32 uTCOffsetInMinutes,
            String zip,
            System.Guid code)
        {
            string procedureName = "CustomerInsertAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            //Int32 activeOrganizationID,
            //String email,
            if (System.Convert.ToDateTime(emailConfirmedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 emailConfirmedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //String firstName,
            if (System.Convert.ToDateTime(forgotPasswordKeyExpirationUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 forgotPasswordKeyExpirationUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //String forgotPasswordKeyValue,
            //Guid fSUserCodeValue,
            //Boolean isActive,
            //Boolean isEmailAllowed,
            //Boolean isEmailConfirmed,
            //Boolean isEmailMarketingAllowed,
            //Boolean isLocked,
            //Boolean isMultipleOrganizationsAllowed,
            //Boolean isVerboseLoggingForced,
            if (System.Convert.ToDateTime(lastLoginUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 lastLoginUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //String lastName,
            //String password,
            //String phone,
            //String province,
            if (System.Convert.ToDateTime(registrationUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 registrationUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Int32 tacID,
            //Int32 uTCOffsetInMinutes,
            //String zip,
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                EF.Models.Customer customer = new EF.Models.Customer();
                customer.Code = code;
                customer.LastChangeCode = Guid.NewGuid();
                customer.ActiveOrganizationID = activeOrganizationID;
                customer.Email = email;
                customer.EmailConfirmedUTCDateTime = emailConfirmedUTCDateTime;
                customer.FirstName = firstName;
                customer.ForgotPasswordKeyExpirationUTCDateTime = forgotPasswordKeyExpirationUTCDateTime;
                customer.ForgotPasswordKeyValue = forgotPasswordKeyValue;
                customer.FSUserCodeValue = fSUserCodeValue;
                customer.IsActive = isActive;
                customer.IsEmailAllowed = isEmailAllowed;
                customer.IsEmailConfirmed = isEmailConfirmed;
                customer.IsEmailMarketingAllowed = isEmailMarketingAllowed;
                customer.IsLocked = isLocked;
                customer.IsMultipleOrganizationsAllowed = isMultipleOrganizationsAllowed;
                customer.IsVerboseLoggingForced = isVerboseLoggingForced;
                customer.LastLoginUTCDateTime = lastLoginUTCDateTime;
                customer.LastName = lastName;
                customer.Password = password;
                customer.Phone = phone;
                customer.Province = province;
                customer.RegistrationUTCDateTime = registrationUTCDateTime;
                customer.TacID = tacID;
                customer.UTCOffsetInMinutes = uTCOffsetInMinutes;
                customer.Zip = zip;
                customer = await customerManager.AddAsync(customer);
                iOut = customer.CustomerID;
            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return iOut;
        }
        public override void CustomerUpdate(
            SessionContext context,
            int customerID,
            Int32 activeOrganizationID,
            String email,
            DateTime emailConfirmedUTCDateTime,
            String firstName,
            DateTime forgotPasswordKeyExpirationUTCDateTime,
            String forgotPasswordKeyValue,
            Guid fSUserCodeValue,
            Boolean isActive,
            Boolean isEmailAllowed,
            Boolean isEmailConfirmed,
            Boolean isEmailMarketingAllowed,
            Boolean isLocked,
            Boolean isMultipleOrganizationsAllowed,
            Boolean isVerboseLoggingForced,
            DateTime lastLoginUTCDateTime,
            String lastName,
            String password,
            String phone,
            String province,
            DateTime registrationUTCDateTime,
            Int32 tacID,
            Int32 uTCOffsetInMinutes,
            String zip,
             Guid lastChangeCode,
             System.Guid code)
        {
            string procedureName = "CustomerUpdate";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            //Int32 activeOrganizationID,
            //String email,
            if (System.Convert.ToDateTime(emailConfirmedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 emailConfirmedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //String firstName,
            if (System.Convert.ToDateTime(forgotPasswordKeyExpirationUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 forgotPasswordKeyExpirationUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //String forgotPasswordKeyValue,
            //Guid fSUserCodeValue,
            //Boolean isActive,
            //Boolean isEmailAllowed,
            //Boolean isEmailConfirmed,
            //Boolean isEmailMarketingAllowed,
            //Boolean isLocked,
            //Boolean isMultipleOrganizationsAllowed,
            //Boolean isVerboseLoggingForced,
            if (System.Convert.ToDateTime(lastLoginUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 lastLoginUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //String lastName,
            //String password,
            //String phone,
            //String province,
            if (System.Convert.ToDateTime(registrationUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 registrationUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Int32 tacID,
            //Int32 uTCOffsetInMinutes,
            //String zip,
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                EF.Models.Customer customer = new EF.Models.Customer();
                customer.Code = code;
                customer.ActiveOrganizationID = activeOrganizationID;
                customer.Email = email;
                customer.EmailConfirmedUTCDateTime = emailConfirmedUTCDateTime;
                customer.FirstName = firstName;
                customer.ForgotPasswordKeyExpirationUTCDateTime = forgotPasswordKeyExpirationUTCDateTime;
                customer.ForgotPasswordKeyValue = forgotPasswordKeyValue;
                customer.FSUserCodeValue = fSUserCodeValue;
                customer.IsActive = isActive;
                customer.IsEmailAllowed = isEmailAllowed;
                customer.IsEmailConfirmed = isEmailConfirmed;
                customer.IsEmailMarketingAllowed = isEmailMarketingAllowed;
                customer.IsLocked = isLocked;
                customer.IsMultipleOrganizationsAllowed = isMultipleOrganizationsAllowed;
                customer.IsVerboseLoggingForced = isVerboseLoggingForced;
                customer.LastLoginUTCDateTime = lastLoginUTCDateTime;
                customer.LastName = lastName;
                customer.Password = password;
                customer.Phone = phone;
                customer.Province = province;
                customer.RegistrationUTCDateTime = registrationUTCDateTime;
                customer.TacID = tacID;
                customer.UTCOffsetInMinutes = uTCOffsetInMinutes;
                customer.Zip = zip;
                bool success = customerManager.Update(customer);
                if (!success)
                {
                    throw new System.Exception("Your changes will overwrite changes made by another user.");
                }
            }
            catch (Exception x)
            {
                HandleError(x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override async Task CustomerUpdateAsync(
            SessionContext context,
            int customerID,
            Int32 activeOrganizationID,
            String email,
            DateTime emailConfirmedUTCDateTime,
            String firstName,
            DateTime forgotPasswordKeyExpirationUTCDateTime,
            String forgotPasswordKeyValue,
            Guid fSUserCodeValue,
            Boolean isActive,
            Boolean isEmailAllowed,
            Boolean isEmailConfirmed,
            Boolean isEmailMarketingAllowed,
            Boolean isLocked,
            Boolean isMultipleOrganizationsAllowed,
            Boolean isVerboseLoggingForced,
            DateTime lastLoginUTCDateTime,
            String lastName,
            String password,
            String phone,
            String province,
            DateTime registrationUTCDateTime,
            Int32 tacID,
            Int32 uTCOffsetInMinutes,
            String zip,
            Guid lastChangeCode,
            System.Guid code)
        {
            string procedureName = "CustomerUpdateAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            //Int32 activeOrganizationID,
            //String email,
            if (System.Convert.ToDateTime(emailConfirmedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 emailConfirmedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //String firstName,
            if (System.Convert.ToDateTime(forgotPasswordKeyExpirationUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 forgotPasswordKeyExpirationUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //String forgotPasswordKeyValue,
            //Guid fSUserCodeValue,
            //Boolean isActive,
            //Boolean isEmailAllowed,
            //Boolean isEmailConfirmed,
            //Boolean isEmailMarketingAllowed,
            //Boolean isLocked,
            //Boolean isMultipleOrganizationsAllowed,
            //Boolean isVerboseLoggingForced,
            if (System.Convert.ToDateTime(lastLoginUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 lastLoginUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //String lastName,
            //String password,
            //String phone,
            //String province,
            if (System.Convert.ToDateTime(registrationUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 registrationUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Int32 tacID,
            //Int32 uTCOffsetInMinutes,
            //String zip,
            //Define the parameters
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                EF.Models.Customer customer = new EF.Models.Customer();
                customer.Code = code;
                customer.ActiveOrganizationID = activeOrganizationID;
                customer.Email = email;
                customer.EmailConfirmedUTCDateTime = emailConfirmedUTCDateTime;
                customer.FirstName = firstName;
                customer.ForgotPasswordKeyExpirationUTCDateTime = forgotPasswordKeyExpirationUTCDateTime;
                customer.ForgotPasswordKeyValue = forgotPasswordKeyValue;
                customer.FSUserCodeValue = fSUserCodeValue;
                customer.IsActive = isActive;
                customer.IsEmailAllowed = isEmailAllowed;
                customer.IsEmailConfirmed = isEmailConfirmed;
                customer.IsEmailMarketingAllowed = isEmailMarketingAllowed;
                customer.IsLocked = isLocked;
                customer.IsMultipleOrganizationsAllowed = isMultipleOrganizationsAllowed;
                customer.IsVerboseLoggingForced = isVerboseLoggingForced;
                customer.LastLoginUTCDateTime = lastLoginUTCDateTime;
                customer.LastName = lastName;
                customer.Password = password;
                customer.Phone = phone;
                customer.Province = province;
                customer.RegistrationUTCDateTime = registrationUTCDateTime;
                customer.TacID = tacID;
                customer.UTCOffsetInMinutes = uTCOffsetInMinutes;
                customer.Zip = zip;
                bool success = await customerManager.UpdateAsync(customer);
                if(!success)
                {
                    throw new System.Exception("Your changes will overwrite changes made by another user.");
                }
            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
        }
        public override IDataReader SearchCustomers(
            SessionContext context,
            bool searchByCustomerID, int customerID,
            bool searchByActiveOrganizationID, Int32 activeOrganizationID,
            bool searchByEmail, String email,
            bool searchByEmailConfirmedUTCDateTime, DateTime emailConfirmedUTCDateTime,
            bool searchByFirstName, String firstName,
            bool searchByForgotPasswordKeyExpirationUTCDateTime, DateTime forgotPasswordKeyExpirationUTCDateTime,
            bool searchByForgotPasswordKeyValue, String forgotPasswordKeyValue,
            bool searchByFSUserCodeValue, Guid fSUserCodeValue,
            bool searchByIsActive, Boolean isActive,
            bool searchByIsEmailAllowed, Boolean isEmailAllowed,
            bool searchByIsEmailConfirmed, Boolean isEmailConfirmed,
            bool searchByIsEmailMarketingAllowed, Boolean isEmailMarketingAllowed,
            bool searchByIsLocked, Boolean isLocked,
            bool searchByIsMultipleOrganizationsAllowed, Boolean isMultipleOrganizationsAllowed,
            bool searchByIsVerboseLoggingForced, Boolean isVerboseLoggingForced,
            bool searchByLastLoginUTCDateTime, DateTime lastLoginUTCDateTime,
            bool searchByLastName, String lastName,
            bool searchByPassword, String password,
            bool searchByPhone, String phone,
            bool searchByProvince, String province,
            bool searchByRegistrationUTCDateTime, DateTime registrationUTCDateTime,
            bool searchByTacID, Int32 tacID,
            bool searchByUTCOffsetInMinutes, Int32 uTCOffsetInMinutes,
            bool searchByZip, String zip,
            bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchCustomers";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                throw new System.Exception("Not implemented");
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Customer_Search: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public override async Task<IDataReader> SearchCustomersAsync(
                    SessionContext context,
                    bool searchByCustomerID, int customerID,
                    bool searchByActiveOrganizationID, Int32 activeOrganizationID,
                    bool searchByEmail, String email,
                    bool searchByEmailConfirmedUTCDateTime, DateTime emailConfirmedUTCDateTime,
                    bool searchByFirstName, String firstName,
                    bool searchByForgotPasswordKeyExpirationUTCDateTime, DateTime forgotPasswordKeyExpirationUTCDateTime,
                    bool searchByForgotPasswordKeyValue, String forgotPasswordKeyValue,
                    bool searchByFSUserCodeValue, Guid fSUserCodeValue,
                    bool searchByIsActive, Boolean isActive,
                    bool searchByIsEmailAllowed, Boolean isEmailAllowed,
                    bool searchByIsEmailConfirmed, Boolean isEmailConfirmed,
                    bool searchByIsEmailMarketingAllowed, Boolean isEmailMarketingAllowed,
                    bool searchByIsLocked, Boolean isLocked,
                    bool searchByIsMultipleOrganizationsAllowed, Boolean isMultipleOrganizationsAllowed,
                    bool searchByIsVerboseLoggingForced, Boolean isVerboseLoggingForced,
                    bool searchByLastLoginUTCDateTime, DateTime lastLoginUTCDateTime,
                    bool searchByLastName, String lastName,
                    bool searchByPassword, String password,
                    bool searchByPhone, String phone,
                    bool searchByProvince, String province,
                    bool searchByRegistrationUTCDateTime, DateTime registrationUTCDateTime,
                    bool searchByTacID, Int32 tacID,
                    bool searchByUTCOffsetInMinutes, Int32 uTCOffsetInMinutes,
                    bool searchByZip, String zip,
                    bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchCustomersAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                throw new System.Exception("Not implemented");
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Customer_Search: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public override IDataReader GetCustomerList(
            SessionContext context)
        {
            string procedureName = "GetCustomerList";
            Log(procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                rdr = BuildDataReader(customerManager.GetAll());
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Customer_GetList: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public override async Task<IDataReader> GetCustomerListAsync(
            SessionContext context)
        {
            string procedureName = "GetCustomerListAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                rdr = BuildDataReader(await customerManager.GetAllAsync());
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Customer_GetList: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public override Guid GetCustomerCode(
            SessionContext context,
            int customerID)
        {
            string procedureName = "GetCustomerCode";
            Log(procedureName + "::Start");
            Log(procedureName + "::customerID::" + customerID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "Customer::" + customerID.ToString() + "::code";
            if (FS.Common.Caches.StringCache.Exists(cacheKey))
            {
                string codeStr = FS.Common.Caches.StringCache.GetData(cacheKey);
                if (Guid.TryParse(codeStr, out result))
                {
                    Log(procedureName + "::Get From Cache");
                    Log(procedureName + "::End");
                    return result;
                }
            }
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                var customer = customerManager.GetById(customerID);
                result = customer.Code.Value;
                FS.Common.Caches.StringCache.SetData(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Customer_GetCode: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return result;
        }
        public override async Task<Guid> GetCustomerCodeAsync(
            SessionContext context,
            int customerID)
        {
            string procedureName = "GetCustomerCodeAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::customerID::" + customerID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "Customer::" + customerID.ToString() + "::code";
            if (FS.Common.Caches.StringCache.Exists(cacheKey))
            {
                string codeStr = FS.Common.Caches.StringCache.GetData(cacheKey);
                if (Guid.TryParse(codeStr, out result))
                {
                    await LogAsync(context, procedureName + "::Get From Cache");
                    await LogAsync(context, procedureName + "::End");
                    return result;
                }
            }
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                var customer = await customerManager.GetByIdAsync(customerID);
                result = customer.Code.Value;
                await FS.Common.Caches.StringCache.SetDataAsync(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Customer_GetCode: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return result;
        }
        public override IDataReader GetCustomer(
            SessionContext context,
            int customerID)
        {
            string procedureName = "GetCustomer";
            Log(procedureName + "::Start");
            Log(procedureName + "::customerID::" + customerID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                List<EF.Models.Customer> customers = new List<EF.Models.Customer>();
                var customer = customerManager.GetById(customerID);
                customers.Add(customer);
                rdr = BuildDataReader(customers);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Customer_Get: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public override async Task<IDataReader> GetCustomerAsync(
            SessionContext context,
            int customerID)
        {
            string procedureName = "GetCustomerAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::customerID::" + customerID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                List<EF.Models.Customer> customers = new List<EF.Models.Customer>();
                var customer = await customerManager.GetByIdAsync(customerID);
                customers.Add(customer);
                rdr = BuildDataReader(customers);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Customer_Get: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public override IDataReader GetDirtyCustomer(
            SessionContext context,
            int customerID)
        {
            string procedureName = "GetDirtyCustomer";
            Log(procedureName + "::Start");
            Log(procedureName + "::customerID::" + customerID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                List<EF.Models.Customer> customers = new List<EF.Models.Customer>();
                var customer = customerManager.DirtyGetById(customerID);
                customers.Add(customer);
                rdr = BuildDataReader(customers);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Customer_DirtyGet: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public override async Task<IDataReader> GetDirtyCustomerAsync(
            SessionContext context,
            int customerID)
        {
            string procedureName = "GetDirtyCustomerAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::customerID::" + customerID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                List<EF.Models.Customer> customers = new List<EF.Models.Customer>();
                var customer = await customerManager.DirtyGetByIdAsync(customerID);
                customers.Add(customer);
                rdr = BuildDataReader(customers);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Customer_DirtyGet: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public override IDataReader GetCustomer(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetCustomer";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                List<EF.Models.Customer> customers = new List<EF.Models.Customer>();
                var customer = customerManager.GetByCode(code);
                customers.Add(customer);
                rdr = BuildDataReader(customers);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Customer_GetByCode: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public override async Task<IDataReader> GetCustomerAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetCustomerAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                List<EF.Models.Customer> customers = new List<EF.Models.Customer>();
                var customer = await customerManager.GetByCodeAsync(code);
                customers.Add(customer);
                rdr = BuildDataReader(customers);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Customer_GetByCode: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public override IDataReader GetDirtyCustomer(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDirtyCustomer";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                List<EF.Models.Customer> customers = new List<EF.Models.Customer>();
                var customer = customerManager.DirtyGetByCode(code);
                customers.Add(customer);
                rdr = BuildDataReader(customers);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Customer_DirtyGetByCode: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public override async Task<IDataReader> GetDirtyCustomerAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDirtyCustomerAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                List<EF.Models.Customer> customers = new List<EF.Models.Customer>();
                var customer = await customerManager.DirtyGetByCodeAsync(code);
                customers.Add(customer);
                rdr = BuildDataReader(customers);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Customer_DirtyGetByCode: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public override int GetCustomerID(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetCustomerID";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            int result = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                var customer = customerManager.GetByCode(code);
                result = customer.CustomerID;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Customer_GetID: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return result;
        }
        public override async Task<int> GetCustomerIDAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetCustomerIDAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            int result = 0;
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                var customer = await customerManager.GetByCodeAsync(code);
                result = customer.CustomerID;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Customer_GetID: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return result;
        }
        public override int CustomerBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.Customer> dataList)
        {
            string procedureName = "CustomerBulkInsertList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (dataList.Count == 0)
                return bulkCount;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                List<EF.Models.Customer> customers = new List<EF.Models.Customer>();
                for(int i = 0;i < dataList.Count;i++)
                {
                    if (dataList[i].CustomerID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.Customer item = dataList[i];
                    EF.Models.Customer customer = new EF.Models.Customer();
                    customer.Code = item.Code;
                    customer.LastChangeCode = Guid.NewGuid();
                    customer.ActiveOrganizationID = item.ActiveOrganizationID;
                    customer.Email = item.Email;
                    customer.EmailConfirmedUTCDateTime = item.EmailConfirmedUTCDateTime;
                    customer.FirstName = item.FirstName;
                    customer.ForgotPasswordKeyExpirationUTCDateTime = item.ForgotPasswordKeyExpirationUTCDateTime;
                    customer.ForgotPasswordKeyValue = item.ForgotPasswordKeyValue;
                    customer.FSUserCodeValue = item.FSUserCodeValue;
                    customer.IsActive = item.IsActive;
                    customer.IsEmailAllowed = item.IsEmailAllowed;
                    customer.IsEmailConfirmed = item.IsEmailConfirmed;
                    customer.IsEmailMarketingAllowed = item.IsEmailMarketingAllowed;
                    customer.IsLocked = item.IsLocked;
                    customer.IsMultipleOrganizationsAllowed = item.IsMultipleOrganizationsAllowed;
                    customer.IsVerboseLoggingForced = item.IsVerboseLoggingForced;
                    customer.LastLoginUTCDateTime = item.LastLoginUTCDateTime;
                    customer.LastName = item.LastName;
                    customer.Password = item.Password;
                    customer.Phone = item.Phone;
                    customer.Province = item.Province;
                    customer.RegistrationUTCDateTime = item.RegistrationUTCDateTime;
                    customer.TacID = item.TacID;
                    customer.UTCOffsetInMinutes = item.UTCOffsetInMinutes;
                    customer.Zip = item.Zip;
                    customers.Add(customer);
                }
                customerManager.BulkInsert(customers);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_Customer_BulkInsert_v19: " + x.Message + " \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return bulkCount;
        }
        public override async Task<int> CustomerBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Customer> dataList)
        {
            string procedureName = "CustomerBulkInsertListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (dataList.Count == 0)
                return bulkCount;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                List<EF.Models.Customer> customers = new List<EF.Models.Customer>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].CustomerID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.Customer item = dataList[i];
                    EF.Models.Customer customer = new EF.Models.Customer();
                    customer.Code = item.Code;
                    customer.LastChangeCode = Guid.NewGuid();
                    customer.ActiveOrganizationID = item.ActiveOrganizationID;
                    customer.Email = item.Email;
                    customer.EmailConfirmedUTCDateTime = item.EmailConfirmedUTCDateTime;
                    customer.FirstName = item.FirstName;
                    customer.ForgotPasswordKeyExpirationUTCDateTime = item.ForgotPasswordKeyExpirationUTCDateTime;
                    customer.ForgotPasswordKeyValue = item.ForgotPasswordKeyValue;
                    customer.FSUserCodeValue = item.FSUserCodeValue;
                    customer.IsActive = item.IsActive;
                    customer.IsEmailAllowed = item.IsEmailAllowed;
                    customer.IsEmailConfirmed = item.IsEmailConfirmed;
                    customer.IsEmailMarketingAllowed = item.IsEmailMarketingAllowed;
                    customer.IsLocked = item.IsLocked;
                    customer.IsMultipleOrganizationsAllowed = item.IsMultipleOrganizationsAllowed;
                    customer.IsVerboseLoggingForced = item.IsVerboseLoggingForced;
                    customer.LastLoginUTCDateTime = item.LastLoginUTCDateTime;
                    customer.LastName = item.LastName;
                    customer.Password = item.Password;
                    customer.Phone = item.Phone;
                    customer.Province = item.Province;
                    customer.RegistrationUTCDateTime = item.RegistrationUTCDateTime;
                    customer.TacID = item.TacID;
                    customer.UTCOffsetInMinutes = item.UTCOffsetInMinutes;
                    customer.Zip = item.Zip;
                    customers.Add(customer);
                }
                await customerManager.BulkInsertAsync(customers);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_Customer_BulkInsert_v19: " + x.Message + " \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return bulkCount;
        }
        public override int CustomerBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.Customer> dataList)
        {
            string procedureName = "CustomerBulkUpdateList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (dataList.Count == 0)
                return bulkCount;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                List<EF.Models.Customer> customers = new List<EF.Models.Customer>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].CustomerID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.Customer item = dataList[i];
                    EF.Models.Customer customer = new EF.Models.Customer();
                    customer.CustomerID = item.CustomerID;
                    customer.Code = item.Code;
                    customer.ActiveOrganizationID = item.ActiveOrganizationID;
                    customer.Email = item.Email;
                    customer.EmailConfirmedUTCDateTime = item.EmailConfirmedUTCDateTime;
                    customer.FirstName = item.FirstName;
                    customer.ForgotPasswordKeyExpirationUTCDateTime = item.ForgotPasswordKeyExpirationUTCDateTime;
                    customer.ForgotPasswordKeyValue = item.ForgotPasswordKeyValue;
                    customer.FSUserCodeValue = item.FSUserCodeValue;
                    customer.IsActive = item.IsActive;
                    customer.IsEmailAllowed = item.IsEmailAllowed;
                    customer.IsEmailConfirmed = item.IsEmailConfirmed;
                    customer.IsEmailMarketingAllowed = item.IsEmailMarketingAllowed;
                    customer.IsLocked = item.IsLocked;
                    customer.IsMultipleOrganizationsAllowed = item.IsMultipleOrganizationsAllowed;
                    customer.IsVerboseLoggingForced = item.IsVerboseLoggingForced;
                    customer.LastLoginUTCDateTime = item.LastLoginUTCDateTime;
                    customer.LastName = item.LastName;
                    customer.Password = item.Password;
                    customer.Phone = item.Phone;
                    customer.Province = item.Province;
                    customer.RegistrationUTCDateTime = item.RegistrationUTCDateTime;
                    customer.TacID = item.TacID;
                    customer.UTCOffsetInMinutes = item.UTCOffsetInMinutes;
                    customer.Zip = item.Zip;
                    customer.LastChangeCode = item.LastChangeCode;
                    customers.Add(customer);
                }
                customerManager.BulkUpdate(customers);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_Customer_BulkUpdate_v19: " + x.Message + " \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return bulkCount;
        }
        public override async Task<int> CustomerBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Customer> dataList)
        {
            string procedureName = "CustomerBulkUpdateListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (dataList.Count == 0)
                return bulkCount;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                List<EF.Models.Customer> customers = new List<EF.Models.Customer>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].CustomerID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.Customer item = dataList[i];
                    EF.Models.Customer customer = new EF.Models.Customer();
                    customer.CustomerID = item.CustomerID;
                    customer.Code = item.Code;
                    customer.ActiveOrganizationID = item.ActiveOrganizationID;
                    customer.Email = item.Email;
                    customer.EmailConfirmedUTCDateTime = item.EmailConfirmedUTCDateTime;
                    customer.FirstName = item.FirstName;
                    customer.ForgotPasswordKeyExpirationUTCDateTime = item.ForgotPasswordKeyExpirationUTCDateTime;
                    customer.ForgotPasswordKeyValue = item.ForgotPasswordKeyValue;
                    customer.FSUserCodeValue = item.FSUserCodeValue;
                    customer.IsActive = item.IsActive;
                    customer.IsEmailAllowed = item.IsEmailAllowed;
                    customer.IsEmailConfirmed = item.IsEmailConfirmed;
                    customer.IsEmailMarketingAllowed = item.IsEmailMarketingAllowed;
                    customer.IsLocked = item.IsLocked;
                    customer.IsMultipleOrganizationsAllowed = item.IsMultipleOrganizationsAllowed;
                    customer.IsVerboseLoggingForced = item.IsVerboseLoggingForced;
                    customer.LastLoginUTCDateTime = item.LastLoginUTCDateTime;
                    customer.LastName = item.LastName;
                    customer.Password = item.Password;
                    customer.Phone = item.Phone;
                    customer.Province = item.Province;
                    customer.RegistrationUTCDateTime = item.RegistrationUTCDateTime;
                    customer.TacID = item.TacID;
                    customer.UTCOffsetInMinutes = item.UTCOffsetInMinutes;
                    customer.Zip = item.Zip;
                    customer.LastChangeCode = item.LastChangeCode;
                    customers.Add(customer);
                }
                customerManager.BulkUpdate(customers);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_Customer_BulkUpdate_v19: " + x.Message + " \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return bulkCount;
        }
        public override int CustomerBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.Customer> dataList)
        {
            string procedureName = "CustomerBulkDeleteList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                List<EF.Models.Customer> customers = new List<EF.Models.Customer>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].CustomerID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.Customer item = dataList[i];
                    EF.Models.Customer customer = new EF.Models.Customer();
                    customer.CustomerID = item.CustomerID;
                    customer.Code = item.Code;
                    customer.ActiveOrganizationID = item.ActiveOrganizationID;
                    customer.Email = item.Email;
                    customer.EmailConfirmedUTCDateTime = item.EmailConfirmedUTCDateTime;
                    customer.FirstName = item.FirstName;
                    customer.ForgotPasswordKeyExpirationUTCDateTime = item.ForgotPasswordKeyExpirationUTCDateTime;
                    customer.ForgotPasswordKeyValue = item.ForgotPasswordKeyValue;
                    customer.FSUserCodeValue = item.FSUserCodeValue;
                    customer.IsActive = item.IsActive;
                    customer.IsEmailAllowed = item.IsEmailAllowed;
                    customer.IsEmailConfirmed = item.IsEmailConfirmed;
                    customer.IsEmailMarketingAllowed = item.IsEmailMarketingAllowed;
                    customer.IsLocked = item.IsLocked;
                    customer.IsMultipleOrganizationsAllowed = item.IsMultipleOrganizationsAllowed;
                    customer.IsVerboseLoggingForced = item.IsVerboseLoggingForced;
                    customer.LastLoginUTCDateTime = item.LastLoginUTCDateTime;
                    customer.LastName = item.LastName;
                    customer.Password = item.Password;
                    customer.Phone = item.Phone;
                    customer.Province = item.Province;
                    customer.RegistrationUTCDateTime = item.RegistrationUTCDateTime;
                    customer.TacID = item.TacID;
                    customer.UTCOffsetInMinutes = item.UTCOffsetInMinutes;
                    customer.Zip = item.Zip;
                    customer.LastChangeCode = item.LastChangeCode;
                    customers.Add(customer);
                }
                customerManager.BulkDelete(customers);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_Customer_BulkDelete_v19: " + x.Message + " \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return bulkCount;
        }
        public override async Task<int> CustomerBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Customer> dataList)
        {
            string procedureName = "CustomerBulkDeleteListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                List<EF.Models.Customer> customers = new List<EF.Models.Customer>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].CustomerID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.Customer item = dataList[i];
                    EF.Models.Customer customer = new EF.Models.Customer();
                    customer.CustomerID = item.CustomerID;
                    customer.Code = item.Code;
                    customer.ActiveOrganizationID = item.ActiveOrganizationID;
                    customer.Email = item.Email;
                    customer.EmailConfirmedUTCDateTime = item.EmailConfirmedUTCDateTime;
                    customer.FirstName = item.FirstName;
                    customer.ForgotPasswordKeyExpirationUTCDateTime = item.ForgotPasswordKeyExpirationUTCDateTime;
                    customer.ForgotPasswordKeyValue = item.ForgotPasswordKeyValue;
                    customer.FSUserCodeValue = item.FSUserCodeValue;
                    customer.IsActive = item.IsActive;
                    customer.IsEmailAllowed = item.IsEmailAllowed;
                    customer.IsEmailConfirmed = item.IsEmailConfirmed;
                    customer.IsEmailMarketingAllowed = item.IsEmailMarketingAllowed;
                    customer.IsLocked = item.IsLocked;
                    customer.IsMultipleOrganizationsAllowed = item.IsMultipleOrganizationsAllowed;
                    customer.IsVerboseLoggingForced = item.IsVerboseLoggingForced;
                    customer.LastLoginUTCDateTime = item.LastLoginUTCDateTime;
                    customer.LastName = item.LastName;
                    customer.Password = item.Password;
                    customer.Phone = item.Phone;
                    customer.Province = item.Province;
                    customer.RegistrationUTCDateTime = item.RegistrationUTCDateTime;
                    customer.TacID = item.TacID;
                    customer.UTCOffsetInMinutes = item.UTCOffsetInMinutes;
                    customer.Zip = item.Zip;
                    customer.LastChangeCode = item.LastChangeCode;
                    customers.Add(customer);
                }
                await customerManager.BulkDeleteAsync(customers);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_Customer_BulkDelete_v19: " + x.Message + " \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return bulkCount;
        }
        public override void CustomerDelete(
            SessionContext context,
            int customerID)
        {
            string procedureName = "CustomerDelete";
            Log(procedureName + "::Start");
            Log(procedureName + "::customerID::" + customerID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                customerManager.Delete(customerID);
            }
            catch (Exception x)
            {
                HandleError( x, "FS_Farm_Customer_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override async Task CustomerDeleteAsync(
           SessionContext context,
           int customerID)
        {
            string procedureName = "CustomerDeleteAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::customerID::" + customerID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                await customerManager.DeleteAsync(customerID);
            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, "FS_Farm_Customer_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
        }
        public override void CustomerCleanupTesting(
            SessionContext context )
        {
            string procedureName = "CustomerCleanupTesting";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                EF.CurrentRuntime.ClearTestObjects(dbContext);
            }
            catch (Exception x)
            {
                HandleError(  x, "FS_Farm_TestObjectCleanup");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override void CustomerCleanupChildObjectTesting(
            SessionContext context)
        {
            string procedureName = "CustomerCleanupChildObjectTesting";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                EF.CurrentRuntime.ClearTestChildObjects(dbContext);
            }
            catch (Exception x)
            {
                HandleError(  x, "FS_Farm_TestChildObjectCleanup");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override bool SupportsTransactions()
        {
            return true;
        }
        #endregion
        #region Error Handling
        void HandleError( Exception x, string sprocName)
        {
            Log(x);
            string sException = "Error Executing " + sprocName + ": " + x.Message + " \r\n";
            throw new Exception(sException, x);
        }
        async Task HandleErrorAsync(SessionContext context, Exception x, string sprocName)
        {
            await LogAsync(context, x);
            string sException = "Error Executing " + sprocName + ": " + x.Message + " \r\n";
            throw new Exception(sException, x);
        }
        #endregion
        public override IDataReader GetCustomerList_FetchByTacID(
            int tacID,
           SessionContext context
            )
        {
            string procedureName = "GetCustomerList_FetchByTacID";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            IDataReader rdr = null;
            try
            {
                dbContext = BuildDbContext(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                rdr = BuildDataReader(customerManager.GetByTacID(tacID));
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Customer_FetchByTacID: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public override async Task<IDataReader> GetCustomerList_FetchByTacIDAsync(
            int tacID,
           SessionContext context
            )
        {
            string procedureName = "GetCustomerList_FetchByTacIDAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var customerManager = new EF.Managers.CustomerManager(dbContext);
                rdr = BuildDataReader(await customerManager.GetByTacIDAsync(tacID));
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Customer_FetchByTacID: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        private async Task<EF.FarmDbContext> BuildDbContextAsync(SessionContext context)
        {
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            if (context.UseTransactions)
            {
                if (!context.SqlConnectionExists(_connectionString))
                {
                    if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
                    connection = new SqlConnection(_connectionString);
                    await connection.OpenAsync();
                    context.AddConnection(_connectionString, connection, connection.BeginTransaction());
                }
                else
                {
                    connection = context.GetSqlConnection(_connectionString);
                }
                dbContext = EF.FarmDbContextFactory.Create(connection);
                await dbContext.Database.UseTransactionAsync(context.GetSqlTransaction(_connectionString));
            }
            else
            {
                dbContext = EF.FarmDbContextFactory.Create(_connectionString);
            }
            return dbContext;
        }
        private EF.FarmDbContext BuildDbContext(SessionContext context)
        {
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            if (context.UseTransactions)
            {
                if (!context.SqlConnectionExists(_connectionString))
                {
                    if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
                    connection = new SqlConnection(_connectionString);
                    connection.Open();
                    context.AddConnection(_connectionString, connection, connection.BeginTransaction());
                }
                else
                {
                    connection = context.GetSqlConnection(_connectionString);
                }
                dbContext = EF.FarmDbContextFactory.Create(connection);
                dbContext.Database.UseTransaction(context.GetSqlTransaction(_connectionString));
            }
            else
            {
                dbContext = EF.FarmDbContextFactory.Create(_connectionString);
            }
            return dbContext;
        }
        private IDataReader BuildDataReader(List<EF.Models.Customer> data)
        {
            var dataTable = new DataTable();
            // Using reflection to create columns based on obj properties
            foreach (var prop in typeof(EF.Models.Customer).GetProperties())
            {
                dataTable.Columns.Add(prop.Name, prop.PropertyType);
            }
            // Populating the DataTable
            foreach (var item in data)
            {
                var row = dataTable.NewRow();
                foreach (var prop in typeof(EF.Models.Customer).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }
            return dataTable.CreateDataReader();
        }
    }
}
