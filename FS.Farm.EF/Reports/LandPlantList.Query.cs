using FS.Common.Configuration;
using FS.Common.Objects;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Farm.EF.Reports
{
    public partial class GENVALPascalReportName
    {
         
        private IQueryable<QueryDTO> BuildQuery()
        { 
            return from GENVALLowerUnderscoredObjectName in _dbContext.GENVALPascalObjectNameSet.AsNoTracking() 

				   
				//GENIF[calculatedIsTargetChildObjectAvailable=true]Start 
				//GENIF[calculatedIsTrueParentChild=true]Start
				from GENVALLowerUnderscoredcalculatedTargetChildObject in _dbContext.GENVALPascalcalculatedTargetChildObjectSet.AsNoTracking().Where(x => x.GENVALPascalObjectNameID == GENVALLowerUnderscoredObjectName.GENVALPascalObjectNameID)  //child obj 
				//GENLOOPchildObjLookupsSTART
				from GENVALLowerUnderscoredcalculatedTargetChildObjectGENVALLowerUnderscoredlookupName in _dbContext.GENVALPascallookupNameSet.AsNoTracking().Where(x => x.GENVALPascallookupNameID == GENVALLowerUnderscoredcalculatedTargetChildObject.GENVALPascalimplementationPropName).DefaultIfEmpty() //child obj lookup
                //GENLOOPchildObjLookupsEnd 
				//GENIF[calculatedIsTrueParentChild=true]End 
				//GENIF[calculatedIsTargetChildObjectAvailable=true]End
			
				//GENLOOPPropSTART
				//GENIF[isFK=true]Start
				//GENIF[calculatedisFKObjectParentOFOwnerObject=false]Start
				//GENIF[isFKLookup=false]Start
				from GENVALLowerUnderscoredPROPcalculatedFKObjectName in _dbContext.GENVALPascalPROPcalculatedFKObjectNameSet.AsNoTracking().Where(x => x.GENVALPascalPROPcalculatedFKObjectPropertyName == GENVALLowerUnderscoredObjectName.GENVALPascalPropName).DefaultIfEmpty() // fk prop
                //GENIF[isFKLookup=false]End
				//GENIF[isFKLookup=true]Start
				from GENVALLowerUnderscoredObjectNameGENVALLowerUnderscoredPROPcalculatedFKObjectName in _dbContext.GENVALPascalPROPcalculatedFKObjectNameSet.AsNoTracking().Where(x => x.GENVALPascalPROPcalculatedFKObjectPropertyName == GENVALLowerUnderscoredObjectName.GENVALPascalPropName).DefaultIfEmpty()   // fk lookup prop
                //GENIF[isFKLookup=true]End
				//GENIF[calculatedisFKObjectParentOFOwnerObject=false]End  
				//GENIF[isFK=true]End 
				//GENLOOPPropEnd  
			
				//GENLOOPobjTreePathSTART   
				from GENVALLowerUnderscoredparentObjName  in _dbContext.GENVALPascalparentObjNameSet.AsNoTracking().Where(x => x.GENVALPascalparentObjNameID == GENVALLowerUnderscoredchildObjName.GENVALPascalchildPropName).DefaultIfEmpty() // up obj tree
                //GENLOOPparentObjLookupsSTART   
				from GENVALLowerUnderscoredparentObjNameGENVALLowerUnderscoredlookupName in _dbContext.GENVALPascallookupNameSet.AsNoTracking().Where(x => x.GENVALPascallookupNameID == GENVALLowerUnderscoredparentObjName.GENVALPascalimplementationPropName).DefaultIfEmpty() // tree parent obj lookup prop
				//GENLOOPparentObjLookupsEnd 
				//GENLOOPobjTreePathEnd 
			 

				//GENLOOPobjJoinTreeSTART   
				from GENVALLowerUnderscoredchildObjName in _dbContext.GENVALPascalchildObjNameSet.AsNoTracking().Where(x => x.GENVALPascalchildPropName == GENVALLowerUnderscoredparentObjName.GENVALPascalparentObjNameID).DefaultIfEmpty() // up obj join tree
				//GENLOOPchildObjLookupsSTART   
				from GENVALLowerUnderscoredchildObjNameGENVALLowerUnderscoredlookupName in _dbContext.GENVALPascallookupNameSet.AsNoTracking().Where(x => x.GENVALPascallookupNameID == GENVALLowerUnderscoredchildObjName.GENVALPascalimplementationPropName).DefaultIfEmpty() // join tree hild obj lookup prop
				//GENLOOPchildObjLookupsEnd 
				//GENLOOPobjJoinTreeEnd 
			
				//GENIF[calculatedIsTargetChildObjAPairedIntersectionObj=true]Start
				//GENLOOPintersectionObjSTART 
				//owner obj intersection table  
				from GENVALLowerUnderscoredName in _dbContext.GENVALPascalNameSet.AsNoTracking().Where(x => x.GENVALPascalObjectNameID == GENVALLowerUnderscoredObjectName.GENVALPascalObjectNameID)
				from GENVALLowerUnderscoredpairedObj in _dbContext.GENVALPascalpairedObjSet.AsNoTracking().Where(x => x.GENVALPascalpairedObjID == GENVALLowerUnderscoredName.GENVALPascalpairedObjID)
				//GENLOOPintersectionObjEnd
				//GENIF[calculatedIsTargetChildObjAPairedIntersectionObj=true]End
			 
				//GENIF[calculatedIsTargetChildObjAPairedIntersectionObj=false]Start
				//GENLOOPintersectionObjSTART 
				//owner obj intersection table , 1st record only 
				join GENVALLowerUnderscoredName_group_item in _dbContext.GENVALPascalNameSet.AsNoTracking() on GENVALLowerUnderscoredObjectName.GENVALPascalObjectNameID equals GENVALLowerUnderscoredName_group_item.GENVALPascalObjectNameID into GENVALLowerUnderscoredName_group
				from GENVALLowerUnderscoredName in GENVALLowerUnderscoredName_group.OrderByDescending(x => x.GENVALPascalNameID).DefaultIfEmpty().Take(1)
				//GENLOOPintersectionObjEnd
				//GENIF[calculatedIsTargetChildObjAPairedIntersectionObj=false]End
			  
				//GENLOOPtargetChildObjectIntersectionObjSTART 
				//target child obj intersection table , 1st record only
				join GENVALLowerUnderscoredName_group_item in _dbContext.GENVALPascalNameSet.AsNoTracking() on GENVALLowerUnderscoredcalculatedTargetChildObject.GENVALPascalcalculatedTargetChildObjectID equals GENVALLowerUnderscoredName_group_item.GENVALPascalcalculatedTargetChildObjectID into GENVALLowerUnderscoredName_group
				from GENVALLowerUnderscoredName in GENVALLowerUnderscoredName_group.OrderByDescending(x => x.GENVALPascalNameID).DefaultIfEmpty().Take(1)
				//GENLOOPtargetChildObjectIntersectionObjEnd

				//GENIF[calculatedIsRowLevelOrgCustomerSecurityUsed=true]Start 
				from Customer_Security in _dbContext.CustomerSet.AsNoTracking().Where(x => x.CustomerID == org_customer.CustomerID)
                //GENIF[calculatedIsRowLevelOrgCustomerSecurityUsed=true]End
			
				//GENIF[calculatedIsRowLevelOrganizationSecurityUsed=true]Start
				from OrgCustomer_Security in _dbContext.OrgCustomerSet.AsNoTracking().Where(x => x.OrganizationID == organization.OrganizationID)
				from Customer_Security in _dbContext.CustomerSet.AsNoTracking().Where(x => x.CustomerID == OrgCustomer_Security.CustomerID)
                //GENIF[calculatedIsRowLevelOrganizationSecurityUsed=true]End

                select new QueryDTO
                {
                    GENVALLowerUnderscoredObjectName = GENVALCamelObjectName,
					
				//GENIF[calculatedIsTargetChildObjectAvailable=true]Start 
				//GENIF[calculatedIsTrueParentChild=true]Start
				GENVALLowerUnderscoredcalculatedTargetChildObject = GENVALLowerUnderscoredcalculatedTargetChildObject,
				//GENLOOPchildObjLookupsSTART
				GENVALLowerUnderscoredcalculatedTargetChildObjectGENVALLowerUnderscoredlookupName = GENVALLowerUnderscoredcalculatedTargetChildObjectGENVALLowerUnderscoredlookupName,
				//GENLOOPchildObjLookupsEnd 
				//GENIF[calculatedIsTrueParentChild=true]End 
				//GENIF[calculatedIsTargetChildObjectAvailable=true]End
			
				//GENLOOPPropSTART
				//GENIF[isFK=true]Start
				//GENIF[calculatedisFKObjectParentOFOwnerObject=false]Start
				//GENIF[isFKLookup=false]Start
				GENVALLowerUnderscoredPROPcalculatedFKObjectName = GENVALLowerUnderscoredPROPcalculatedFKObjectName,
				//GENIF[isFKLookup=false]End
				//GENIF[isFKLookup=true]Start
				GENVALLowerUnderscoredObjectNameGENVALLowerUnderscoredPROPcalculatedFKObjectName = GENVALLowerUnderscoredObjectNameGENVALLowerUnderscoredPROPcalculatedFKObjectName,
				//GENIF[isFKLookup=true]End
				//GENIF[calculatedisFKObjectParentOFOwnerObject=false]End  
				//GENIF[isFK=true]End 
				//GENLOOPPropEnd  
			
				//GENLOOPobjTreePathSTART   
				GENVALLowerUnderscoredparentObjName = GENVALLowerUnderscoredparentObjName,
				//GENLOOPparentObjLookupsSTART
				GENVALLowerUnderscoredparentObjNameGENVALLowerUnderscoredlookupName = GENVALLowerUnderscoredparentObjNameGENVALLowerUnderscoredlookupName,
				//GENLOOPparentObjLookupsEnd 
				//GENLOOPobjTreePathEnd 
			 

				//GENLOOPobjJoinTreeSTART   
				GENVALLowerUnderscoredchildObjName = GENVALLowerUnderscoredchildObjName,
				//GENLOOPchildObjLookupsSTART   
				GENVALLowerUnderscoredchildObjNameGENVALLowerUnderscoredlookupName = GENVALLowerUnderscoredchildObjNameGENVALLowerUnderscoredlookupName,
				//GENLOOPchildObjLookupsEnd 
				//GENLOOPobjJoinTreeEnd 
			
				//GENIF[calculatedIsTargetChildObjAPairedIntersectionObj=true]Start
				//GENLOOPintersectionObjSTART 
				//owner obj intersection table  
				GENVALLowerUnderscoredName = GENVALLowerUnderscoredName,
				GENVALLowerUnderscoredpairedObj = GENVALLowerUnderscoredpairedObj,
				//GENLOOPintersectionObjEnd
				//GENIF[calculatedIsTargetChildObjAPairedIntersectionObj=true]End
			

				//GENIF[calculatedIsTargetChildObjAPairedIntersectionObj=false]Start
				//GENLOOPintersectionObjSTART 
				//owner obj intersection table , 1st record only
				GENVALLowerUnderscoredName = GENVALLowerUnderscoredName,
				//GENLOOPintersectionObjEnd
				//GENIF[calculatedIsTargetChildObjAPairedIntersectionObj=false]End
			
			
				//GENLOOPtargetChildObjectIntersectionObjSTART 
				//target child obj intersection table , 1st record only
				GENVALLowerUnderscoredName = GENVALLowerUnderscoredName,
				//GENLOOPtargetChildObjectIntersectionObjEnd

				//GENIF[calculatedIsRowLevelOrgCustomerSecurityUsed=true]Start 
				Customer_Security = Customer_Security,
                //GENIF[calculatedIsRowLevelOrgCustomerSecurityUsed=true]End
			
				//GENIF[calculatedIsRowLevelOrganizationSecurityUsed=true]Start
				OrgCustomer_Security = OrgCustomer_Security,
				Customer_Security = Customer_Security,
                //GENIF[calculatedIsRowLevelOrganizationSecurityUsed=true]End

                };
        }

		

        private IQueryable<QueryDTO> ApplyFilters(
            IQueryable<QueryDTO> query,
           Guid? flavorCode,
           Int32? someIntVal,
           Int64? someBigIntVal,
           Double? someFloatVal,
           Boolean? someBitVal,
           Boolean? isEditAllowed,
           Boolean? isDeleteAllowed,
           Decimal? someDecimalVal,
           DateTime? someMinUTCDateTimeVal,
           DateTime? someMinDateVal,
           Decimal? someMoneyVal,
           String someNVarCharVal,
           String someVarCharVal,
           String someTextVal,
           String somePhoneNumber,
           String someEmailAddress,
           System.Guid userID,
           System.Guid contextCode
           )
        {
            if (contextCode != Guid.Empty) query = query.Where(x => x.GENVALLowerUnderscoredObjectName.Code == contextCode);
			    
        //GENLOOPReportParamStart 
		//GENIF[calculatedIsTargetObjectAvailable=true]Start
			 	//GENCASE[calculatedCSharpDataType]Start
				//GENWHEN[Date]Start 
            if (GENVALCamelReportParamName != null && GENVALCamelReportParamName != (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue) query = query.Where(x => x.GENVALLowerUnderscoredcalculatedTargetLookupObjImplementationObjNameGENVALLowerUnderscoredcalculatedTargetObjectName.GENVALPascalcalculatedTargetPropertyName >= GENVALCamelReportParamName);
            	//GENWHEN[Date]End
				//GENWHEN[DateTime]Start 
            if (GENVALCamelReportParamName != null && GENVALCamelReportParamName != (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue) query = query.Where(x => x.GENVALLowerUnderscoredcalculatedTargetLookupObjImplementationObjNameGENVALLowerUnderscoredcalculatedTargetObjectName.GENVALPascalcalculatedTargetPropertyName >= GENVALCamelReportParamName);
            	//GENWHEN[DateTime]End
				//GENWHEN[Guid]Start 
            if (GENVALCamelReportParamName != null && GENVALCamelReportParamName != Guid.Empty) query = query.Where(x => x.GENVALLowerUnderscoredcalculatedTargetLookupObjImplementationObjNameGENVALLowerUnderscoredcalculatedTargetObjectName.GENVALPascalcalculatedTargetPropertyName == GENVALCamelReportParamName);
				//GENWHEN[Guid]End
				//GENWHEN[String]Start  
            if (!string.IsNullOrEmpty(GENVALCamelReportParamName)) query = query.Where(x => x.GENVALLowerUnderscoredcalculatedTargetLookupObjImplementationObjNameGENVALLowerUnderscoredcalculatedTargetObjectName.GENVALPascalcalculatedTargetPropertyName.Contains(GENVALCamelReportParamName));
				//GENWHEN[String]End
				//GENElseStart  
				
				//GENIF[calculatedFKObjectName=TriStateFilter]Start
				//TriStateFilter GENVALName @GENVALName_TriStateFilterValue
			//TODO and (%(GENVALCamelReportParamName)s is null or %(GENVALCamelReportParamName)s = '00000000-0000-0000-0000-000000000000' or @GENVALName_TriStateFilterValue = -1 or @GENVALName_TriStateFilterValue = GENVALLowerUnderscoredcalculatedTargetLookupObjImplementationObjNameGENVALLowerUnderscoredcalculatedTargetObjectName.GENVALLowerUnderscoredcalculatedTargetPropertyName)
				//GENIF[calculatedFKObjectName=TriStateFilter]End
				//GENIF[calculatedFKObjectName=DateGreaterThanFilter]Start
				//DateGreaterThanFilter GENVALName @GENVALName_DateGreaterThanFilterUtcDateTimeValue
			//TODO and (%(GENVALCamelReportParamName)s is null or %(GENVALCamelReportParamName)s = '00000000-0000-0000-0000-000000000000' or @GENVALName_DateGreaterThanFilterUtcDateTimeValue < GENVALLowerUnderscoredcalculatedTargetLookupObjImplementationObjNameGENVALLowerUnderscoredcalculatedTargetObjectName.GENVALLowerUnderscoredcalculatedTargetPropertyName)
				//GENIF[calculatedFKObjectName=DateGreaterThanFilter]End
				
				//GENIF[calculatedFKObjectName!=TriStateFilter]Start
				//GENIF[calculatedFKObjectName!=DateGreaterThanFilter]Start
			if (GENVALCamelReportParamName != null) query = query.Where(x => x.GENVALLowerUnderscoredcalculatedTargetLookupObjImplementationObjNameGENVALLowerUnderscoredcalculatedTargetObjectName.GENVALPascalcalculatedTargetPropertyName == GENVALCamelReportParamName);
            	//GENIF[calculatedFKObjectName!=DateGreaterThanFilter]End
				//GENIF[calculatedFKObjectName!=TriStateFilter]End

				//GENElseEnd 
				//GENCASE[calculatedCSharpDataType]End
		//GENIF[calculatedIsTargetObjectAvailable=true]End
		//GENIF[calculatedIsTargetObjectAvailable=false]Start
			 	//GENCASE[calculatedCSharpDataType]Start
				//GENWHEN[Date]Start 
            if (GENVALCamelReportParamName != null && GENVALCamelReportParamName != (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue) query = query.Where(x => x.GENVALLowerUnderscoredObjectName.GENVALPascalReportParamName >= GENVALCamelReportParamName);
            	//GENWHEN[Date]End
				//GENWHEN[DateTime]Start 
            if (GENVALCamelReportParamName != null && GENVALCamelReportParamName != (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue) query = query.Where(x => x.GENVALLowerUnderscoredObjectName.GENVALPascalReportParamName >= GENVALCamelReportParamName);
            	//GENWHEN[DateTime]End
				//GENWHEN[Guid]Start 
            if (GENVALCamelReportParamName != null && GENVALCamelReportParamName != Guid.Empty) query = query.Where(x => x.GENVALLowerUnderscoredObjectName.GENVALPascalReportParamName == GENVALCamelReportParamName);
				//GENWHEN[Guid]End
				//GENWHEN[String]Start 
			if (!string.IsNullOrEmpty(GENVALCamelReportParamName)) query = query.Where(x => x.GENVALLowerUnderscoredObjectName.GENVALPascalReportParamName.Contains(GENVALCamelReportParamName));
				//GENWHEN[String]End
				//GENElseStart  
			if (GENVALCamelReportParamName != null) query = query.Where(x => x.GENVALLowerUnderscoredObjectName.GENVALPascalReportParamName == GENVALCamelReportParamName);
            	//GENElseEnd 
				//GENCASE[calculatedCSharpDataType]End
		//GENIF[calculatedIsTargetObjectAvailable=false]End
        //GENLOOPReportParamEnd   
		
			//GENIF[calculatedIsRowLevelCustomerSecurityUsed=true]Start
            query = query.Where(x => x.customer.Code == userID); 
			//GENIF[calculatedIsRowLevelCustomerSecurityUsed=true]End

			//GENIF[calculatedIsRowLevelOrgCustomerSecurityUsed=true]Start
            query = query.Where(x => x.Customer_Security.Code == userID); 
			//GENIF[calculatedIsRowLevelOrgCustomerSecurityUsed=true]End

			//GENIF[calculatedIsRowLevelOrganizationSecurityUsed=true]Start
            query = query.Where(x => x.Customer_Security.Code == userID);  
			//GENIF[calculatedIsRowLevelOrganizationSecurityUsed=true]End

            return query;
        }

		
        private static LandPlantListDTO MapLandPlantListDTO(QueryDTO data)
        {
            return new LandPlantListDTO
            { 
				
				//GENLOOPReportColumnStart
				//GENIF[calculatedIsSourceObjectAvailable=true]Start
				--GENCASE[calculatedCSharpDataType]Start
				--GENWHEN[String]Start 
				GENVALPascalReportColumnName = data.GENVALLowerUnderscoredcalculatedSourceLookupObjImplementationObjNameGENVALLowerUnderscoredcalculatedSourceObjectName.GENVALPascalcalculatedSourcePropertyName,
				--GENWHEN[String]End
				--GENElseStart  
					GENVALPascalReportColumnName = data.GENVALLowerUnderscoredcalculatedSourceLookupObjImplementationObjNameGENVALLowerUnderscoredcalculatedSourceObjectName.GENVALPascalcalculatedSourcePropertyName.Value,
				--GENElseEnd  
				--GENCASE[calculatedCSharpDataType]End
				//GENIF[calculatedIsSourceObjectAvailable=true]End
				//GENIF[calculatedIsSourceObjectAvailable=false]Start
						//GENCASE[calculatedCSharpDataType]Start
						--GENWHEN[String]Start 
						GENVALLowerUnderscoredReportColumnName = data.GENVALLowerUnderscoredObjectName.GENVALPascalReportColumnName, 
						--GENWHEN[String]End
						//GENWHEN[Guid]Start 
						GENVALPascalReportColumnName = data.GENVALLowerUnderscoredObjectName.Code, 
						//GENWHEN[Guid]End
						//GENWHEN[Boolean]Start 
						//GENIF[calculatedIsConditionalSqlLogicAvailable=false]Start 
						GENVALPascalReportColumnName = data.GENVALLowerUnderscoredObjectName.GENVALPascalReportColumnName.Value,
						//GENIF[calculatedIsConditionalSqlLogicAvailable=false]End
						//GENIF[calculatedIsConditionalSqlLogicAvailable=true]Start 
				//TODO	cast((case when GENVALconditionalSqlLogic then 1 else 0 end) as bit) as GENVALPascalReportColumnName.Value,
						//GENIF[calculatedIsConditionalSqlLogicAvailable=true]End
						//GENWHEN[Boolean]End
						//GENElseStart  
						GENVALLowerUnderscoredReportColumnName = data.GENVALLowerUnderscoredObjectName.GENVALPascalReportColumnName.Value, 
						//GENElseEnd 
						//GENCASE[calculatedCSharpDataType]End 
				//GENIF[calculatedIsSourceObjectAvailable=false]End
				//GENLOOPReportColumnEnd  
            };
        }


        private class QueryDTO
        {
            public GENVALPascalObjectName GENVALLowerUnderscoredObjectName { get; set; }

				//GENIF[calculatedIsTargetChildObjectAvailable=true]Start 
				//GENIF[calculatedIsTrueParentChild=true]Start
            public GENVALPascalcalculatedTargetChildObject GENVALLowerUnderscoredcalculatedTargetChildObject { get; set; }
				//GENLOOPchildObjLookupsSTART
            public GENVALPascallookupName GENVALLowerUnderscoredcalculatedTargetChildObjectGENVALLowerUnderscoredlookupName { get; set; }
				//GENLOOPchildObjLookupsEnd 
				//GENIF[calculatedIsTrueParentChild=true]End 
				//GENIF[calculatedIsTargetChildObjectAvailable=true]End
			
				//GENLOOPPropSTART
				//GENIF[isFK=true]Start
				//GENIF[calculatedisFKObjectParentOFOwnerObject=false]Start
				//GENIF[isFKLookup=false]Start
            public GENVALPascalPROPcalculatedFKObjectName GENVALLowerUnderscoredPROPcalculatedFKObjectName { get; set; }
				//GENIF[isFKLookup=false]End
				//GENIF[isFKLookup=true]Start
            public GENVALPascalPROPcalculatedFKObjectName GENVALLowerUnderscoredObjectNameGENVALLowerUnderscoredPROPcalculatedFKObjectName { get; set; }
				//GENIF[isFKLookup=true]End
				//GENIF[calculatedisFKObjectParentOFOwnerObject=false]End  
				//GENIF[isFK=true]End 
				//GENLOOPPropEnd  
			
				//GENLOOPobjTreePathSTART   
            public GENVALPascalparentObjName GENVALLowerUnderscoredparentObjName { get; set; }
				//GENLOOPparentObjLookupsSTART   
            public GENVALPascallookupName GENVALLowerUnderscoredparentObjNameGENVALLowerUnderscoredlookupName { get; set; }
				//GENLOOPparentObjLookupsEnd 
				//GENLOOPobjTreePathEnd 
			 

				//GENLOOPobjJoinTreeSTART   
            public GENVALPascalchildObjName GENVALLowerUnderscoredchildObjName { get; set; }
				//GENLOOPchildObjLookupsSTART   
            public GENVALPascallookupName GENVALLowerUnderscoredchildObjNameGENVALLowerUnderscoredlookupName { get; set; }
				//GENLOOPchildObjLookupsEnd 
				//GENLOOPobjJoinTreeEnd 
			
				//GENIF[calculatedIsTargetChildObjAPairedIntersectionObj=true]Start
				//GENLOOPintersectionObjSTART 
				//owner obj intersection table  
            public GENVALPascalName GENVALLowerUnderscoredName { get; set; }
            public GENVALPascalpairedObj GENVALLowerUnderscoredpairedObj { get; set; }
				//GENLOOPintersectionObjEnd
				//GENIF[calculatedIsTargetChildObjAPairedIntersectionObj=true]End
			 
				//GENIF[calculatedIsTargetChildObjAPairedIntersectionObj=false]Start
				//GENLOOPintersectionObjSTART 
				//owner obj intersection table , 1st record only
            public GENVALPascalName GENVALLowerUnderscoredName { get; set; }
				//GENLOOPintersectionObjEnd
				//GENIF[calculatedIsTargetChildObjAPairedIntersectionObj=false]End
			
			
				//GENLOOPtargetChildObjectIntersectionObjSTART 
				//target child obj intersection table , 1st record only
            public GENVALPascalName GENVALLowerUnderscoredName { get; set; }
				//GENLOOPtargetChildObjectIntersectionObjEnd

				//GENIF[calculatedIsRowLevelOrgCustomerSecurityUsed=true]Start 
            public Customer Customer_Security { get; set; }
			    //GENIF[calculatedIsRowLevelOrgCustomerSecurityUsed=true]End
			
				//GENIF[calculatedIsRowLevelOrganizationSecurityUsed=true]Start
            public OrgCustomer OrgCustomer_Security { get; set; }
            public Customer Customer_Security { get; set; }
			    //GENIF[calculatedIsRowLevelOrganizationSecurityUsed=true]End
        }
    }
}


/*
 * 
            --GENLOOPReportParamStart
		--GENIF[calculatedFKObjectName=TriStateFilter]Start
		--TriStateFilter GENVALName
		DECLARE @GENVALName_TriStateFilterValue int = -1
		select @GENVALName_TriStateFilterValue = StateIntValue from TriStateFilter where code = @i_GENVALCamelcalculatedDBStoredProcParameterPrefixGENVALReportParamName
		--GENIF[calculatedFKObjectName=TriStateFilter]End
		--GENIF[calculatedFKObjectName=DateGreaterThanFilter]Start
		--DateGreaterThanFilter GENVALName
		DECLARE @GENVALName_DateGreaterThanFilterIntValue int = -1
		select @GENVALName_DateGreaterThanFilterIntValue = DayCount from DateGreaterThanFilter where code = @i_GENVALCamelcalculatedDBStoredProcParameterPrefixGENVALReportParamName
		DECLARE @GENVALName_DateGreaterThanFilterUtcDateTimeValue datetime = getutcdate()
		select @GENVALName_DateGreaterThanFilterUtcDateTimeValue = dateadd(d,(-1 * @GENVALName_DateGreaterThanFilterIntValue),getutcdate())
		--GENIF[calculatedFKObjectName=DateGreaterThanFilter]End
        --GENLOOPReportParamEnd

   
	SELECT * FROM 
	(
		SELECT  Top  (@i_iPageNumber * @i_iItemCountPerPage)  
        --GENLOOPReportColumnStart
		--GENIF[calculatedIsSourceObjectAvailable=true]Start
			GENVALcalculatedSourceLookupObjImplementationObjNameGENVALcalculatedSourceObjectName.GENVALcalculatedSourcePropertyName as GENVALReportColumnName,
		--GENIF[calculatedIsSourceObjectAvailable=true]End
		--GENIF[calculatedIsSourceObjectAvailable=false]Start
				--GENCASE[calculatedCSharpDataType]Start
				--GENWHEN[Guid]Start 
			GENVALObjectName.Code as GENVALReportColumnName,
				--GENWHEN[Guid]End
				--GENWHEN[Boolean]Start 
				--GENIF[calculatedIsConditionalSqlLogicAvailable=false]Start 
			GENVALObjectName.GENVALReportColumnName as GENVALReportColumnName,
				--GENIF[calculatedIsConditionalSqlLogicAvailable=false]End
				--GENIF[calculatedIsConditionalSqlLogicAvailable=true]Start 
			cast((case when GENVALconditionalSqlLogic then 1 else 0 end) as bit) as GENVALReportColumnName,
				--GENIF[calculatedIsConditionalSqlLogicAvailable=true]End
				--GENWHEN[Boolean]End
				--GENElseStart  
			GENVALObjectName.GENVALReportColumnName as GENVALReportColumnName,
				--GENElseEnd 
				--GENCASE[calculatedCSharpDataType]End 
		--GENIF[calculatedIsSourceObjectAvailable=false]End
		--GENLOOPReportColumnEnd  
			ROW_NUMBER() OVER(
				ORDER BY 

				
		--GENIF[calculatedIsVisualizationDetailThreeColumn=false]Start 
		--GENIF[calculatedIsVisualizationDetailTwoColumn=false]Start 
				
        --GENLOOPReportColumnStart  
				--GENFILTER[isVisible=true]  
				--GENIF[calculatedIsSourceObjectAvailable=true]Start
				--GENCASE[calculatedSqlServerDBDataType]Start
				--GENWHEN[Uniqueidentifier]Start 
				--GENWHEN[Uniqueidentifier]End
				--GENWHEN[Text]Start 
				--GENWHEN[Text]End
				--GENElseStart  
					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'GENVALReportColumnName' THEN GENVALcalculatedSourceLookupObjImplementationObjNameGENVALcalculatedSourceObjectName.GENVALcalculatedSourcePropertyName  END ASC, 
				--GENElseEnd 
				--GENCASE[calculatedSqlServerDBDataType]End 
				--GENIF[calculatedIsSourceObjectAvailable=true]End
				--GENIF[calculatedIsSourceObjectAvailable=false]Start
				--GENCASE[calculatedSqlServerDBDataType]Start
				--GENWHEN[Uniqueidentifier]Start 
				--GENWHEN[Uniqueidentifier]End
				--GENWHEN[Text]Start 
				--GENWHEN[Text]End
				--GENElseStart  
					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'GENVALReportColumnName' THEN GENVALObjectName.GENVALReportColumnName  END ASC, 
				--GENElseEnd 
				--GENCASE[calculatedSqlServerDBDataType]End 
				--GENIF[calculatedIsSourceObjectAvailable=false]End 
        --GENLOOPReportColumnEnd   


        --GENLOOPReportColumnStart 
				--GENFILTER[isVisible=true]  
				--GENIF[calculatedIsSourceObjectAvailable=true]Start
				--GENCASE[calculatedSqlServerDBDataType]Start
				--GENWHEN[Uniqueidentifier]Start 
				--GENWHEN[Uniqueidentifier]End
				--GENWHEN[Text]Start 
				--GENWHEN[Text]End
				--GENElseStart  
					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'GENVALReportColumnName' THEN GENVALcalculatedSourceLookupObjImplementationObjNameGENVALcalculatedSourceObjectName.GENVALcalculatedSourcePropertyName  END DESC, 
				--GENElseEnd 
				--GENCASE[calculatedSqlServerDBDataType]End
				--GENIF[calculatedIsSourceObjectAvailable=true]End
				--GENIF[calculatedIsSourceObjectAvailable=false]Start
				--GENCASE[calculatedSqlServerDBDataType]Start
				--GENWHEN[Uniqueidentifier]Start 
				--GENWHEN[Uniqueidentifier]End
				--GENWHEN[Text]Start 
				--GENWHEN[Text]End
				--GENElseStart  
					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'GENVALReportColumnName' THEN GENVALObjectName.GENVALReportColumnName  END DESC, 
				--GENElseEnd 
				--GENCASE[calculatedSqlServerDBDataType]End
				--GENIF[calculatedIsSourceObjectAvailable=false]End 
        --GENLOOPReportColumnEnd
		
		--GENIF[calculatedIsVisualizationDetailTwoColumn=false]End 
		--GENIF[calculatedIsVisualizationDetailThreeColumn=false]End 

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'placeholder' THEN ''  END DESC 

				) AS ROWNUMBER 
		  -- select * 
		from 
		 	vw_GENVALRootName_GENVALNamespaceName_GENVALObjectName  GENVALObjectName  --owner obj
			
			--GENIF[calculatedIsTargetChildObjectAvailable=true]Start
			--GENIF[calculatedIsTrueParentChild=false]Start
			--left join vw_GENVALRootName_GENVALNamespaceName_GENVALcalculatedTargetChildObject GENVALcalculatedTargetChildObject on 1=1			
			--GENIF[calculatedIsTrueParentChild=false]End
			--GENIF[calculatedIsTrueParentChild=true]Start
			  join vw_GENVALRootName_GENVALNamespaceName_GENVALcalculatedTargetChildObject GENVALcalculatedTargetChildObject on GENVALObjectName.GENVALObjectNameID = GENVALcalculatedTargetChildObject.GENVALObjectNameID		 --child obj
			--GENLOOPchildObjLookupsSTART
			left join vw_GENVALRootName_GENVALNamespaceName_GENVALlookupName GENVALcalculatedTargetChildObjectGENVALlookupName on GENVALcalculatedTargetChildObject.GENVALimplementationPropName = GENVALcalculatedTargetChildObjectGENVALlookupName.GENVALlookupNameID --child obj lookup prop
			--GENLOOPchildObjLookupsEnd 
			--GENIF[calculatedIsTrueParentChild=true]End 
			--GENIF[calculatedIsTargetChildObjectAvailable=true]End
			
			--GENLOOPPropSTART
			--GENIF[isFK=true]Start
			--GENIF[calculatedisFKObjectParentOFOwnerObject=false]Start
			--GENIF[isFKLookup=false]Start
			left join vw_GENVALRootName_GENVALNamespaceName_GENVALPROPcalculatedFKObjectName GENVALPROPcalculatedFKObjectName on GENVALObjectName.GENVALPropName = GENVALPROPcalculatedFKObjectName.GENVALPROPcalculatedFKObjectPropertyName   -- fk prop
			--GENIF[isFKLookup=false]End
			--GENIF[isFKLookup=true]Start
			left join vw_GENVALRootName_GENVALNamespaceName_GENVALPROPcalculatedFKObjectName GENVALObjectNameGENVALPROPcalculatedFKObjectName on GENVALObjectName.GENVALPropName = GENVALObjectNameGENVALPROPcalculatedFKObjectName.GENVALPROPcalculatedFKObjectPropertyName   -- fk prop
			--GENIF[isFKLookup=true]End
			--GENIF[calculatedisFKObjectParentOFOwnerObject=false]End  
			--GENIF[isFK=true]End 
			--GENLOOPPropEnd  
			
			--GENLOOPobjTreePathSTART   
			left join vw_GENVALRootName_GENVALNamespaceName_GENVALparentObjName GENVALparentObjName on GENVALparentObjName.GENVALparentObjNameID = GENVALchildObjName.GENVALchildPropName  -- up obj tree
			--GENLOOPparentObjLookupsSTART   
			left join vw_GENVALRootName_GENVALNamespaceName_GENVALlookupName GENVALparentObjNameGENVALlookupName on GENVALparentObjName.GENVALimplementationPropName = GENVALparentObjNameGENVALlookupName.GENVALlookupNameID -- tree parent obj lookup prop
			--GENLOOPparentObjLookupsEnd 
			--GENLOOPobjTreePathEnd 
			 

			--GENLOOPobjJoinTreeSTART   
			left join vw_GENVALRootName_GENVALNamespaceName_GENVALchildObjName GENVALchildObjName on GENVALchildObjName.GENVALchildPropName = GENVALparentObjName.GENVALparentObjNameID  -- up obj join tree
			--GENLOOPchildObjLookupsSTART   
			left join vw_GENVALRootName_GENVALNamespaceName_GENVALlookupName GENVALchildObjNameGENVALlookupName on GENVALchildObjName.GENVALimplementationPropName = GENVALchildObjNameGENVALlookupName.GENVALlookupNameID -- join tree hild obj lookup prop
			--GENLOOPchildObjLookupsEnd 
			--GENLOOPobjJoinTreeEnd 
			
			--GENIF[calculatedIsTargetChildObjAPairedIntersectionObj=true]Start
			--GENLOOPintersectionObjSTART 
			--owner obj intersection table  
			join  vw_GENVALRootName_GENVALNamespaceName_GENVALName GENVALName on GENVALName.GENVALObjectNameID = GENVALObjectName.GENVALObjectNameID
			join vw_GENVALRootName_GENVALNamespaceName_GENVALpairedObj GENVALpairedObj on GENVALName.GENVALpairedObjID = GENVALpairedObj.GENVALpairedObjID
			--GENLOOPintersectionObjEnd
			--GENIF[calculatedIsTargetChildObjAPairedIntersectionObj=true]End
			
			
			--GENIF[calculatedIsTargetChildObjAPairedIntersectionObj=false]Start
			--GENLOOPintersectionObjSTART 
			--owner obj intersection table , 1st record only
			left join vw_GENVALRootName_GENVALNamespaceName_GENVALName GENVALName on GENVALName.GENVALObjectNameID = GENVALObjectName.GENVALObjectNameID
			left join vw_GENVALRootName_GENVALNamespaceName_GENVALName GENVALName2 on GENVALName.GENVALObjectNameID = GENVALName2.GENVALObjectNameID and GENVALName.GENVALNameID > GENVALName2.GENVALNameID
			--GENLOOPintersectionObjEnd
			--GENIF[calculatedIsTargetChildObjAPairedIntersectionObj=false]End
			
			
			--GENLOOPtargetChildObjectIntersectionObjSTART 
			--target child obj intersection table , 1st record only
			left join vw_GENVALRootName_GENVALNamespaceName_GENVALName GENVALName on GENVALName.GENVALcalculatedTargetChildObjectID = GENVALcalculatedTargetChildObject.GENVALcalculatedTargetChildObjectID
			left join vw_GENVALRootName_GENVALNamespaceName_GENVALName GENVALName2 on GENVALName.GENVALcalculatedTargetChildObjectID = GENVALName2.GENVALcalculatedTargetChildObjectID and GENVALName.GENVALNameID > GENVALName2.GENVALNameID
			--left join vw_GENVALRootName_GENVALNamespaceName_GENVALpairedObj GENVALpairedObj on GENVALName.GENVALpairedObjID = GENVALpairedObj.GENVALpairedObjID
			--GENLOOPtargetChildObjectIntersectionObjEnd
			
			--GENIF[calculatedIsRowLevelOrgCustomerSecurityUsed=true]Start 
			join vw_GENVALRootName_GENVALNamespaceName_Customer Customer_Security on orgCustomer.CustomerID = Customer_Security.CustomerID
			--GENIF[calculatedIsRowLevelOrgCustomerSecurityUsed=true]End
			
			--GENIF[calculatedIsRowLevelOrganizationSecurityUsed=true]Start
			join vw_GENVALRootName_GENVALNamespaceName_OrgCustomer orgCustomer_Security on orgCustomer_Security.OrganizationID = Organization.OrganizationID
			join vw_GENVALRootName_GENVALNamespaceName_Customer Customer_Security on orgCustomer_Security.CustomerID = Customer_Security.CustomerID
			--GENIF[calculatedIsRowLevelOrganizationSecurityUsed=true]End

		where
			 (GENVALObjectName.code = @i_uidContextCode 
			 GENVALfilteringSqlLogic  )
             --GENIF[calculatedIsRowLevelCustomerSecurityUsed=true]Start
			and (@i_uidUserID is not null and @i_uidUserID <> '00000000-0000-0000-0000-000000000000' and Customer.Code = @i_uidUserID )
			--GENIF[calculatedIsRowLevelCustomerSecurityUsed=true]End

			--GENIF[calculatedIsRowLevelOrgCustomerSecurityUsed=true]Start
			and (@i_uidUserID is not null and @i_uidUserID <> '00000000-0000-0000-0000-000000000000' and Customer_Security.Code = @i_uidUserID )
			--GENIF[calculatedIsRowLevelOrgCustomerSecurityUsed=true]End

			--GENIF[calculatedIsRowLevelOrganizationSecurityUsed=true]Start
			and (@i_uidUserID is not null and @i_uidUserID <> '00000000-0000-0000-0000-000000000000' and Customer_Security.Code = @i_uidUserID )
			--GENIF[calculatedIsRowLevelOrganizationSecurityUsed=true]End
			 
			--GENIF[calculatedIsTargetChildObjAPairedIntersectionObj=false]Start
			--GENLOOPintersectionObjSTART  
			and GENVALName2.GENVALNameID is null
			--GENLOOPintersectionObjEnd
			--GENIF[calculatedIsTargetChildObjAPairedIntersectionObj=false]End
			
			--GENLOOPtargetChildObjectIntersectionObjSTART 
			and GENVALName2.GENVALNameID is null
			--GENLOOPtargetChildObjectIntersectionObjEnd

        --GENLOOPReportParamStart 
		--GENIF[calculatedIsTargetObjectAvailable=true]Start
			 	--GENCASE[calculatedCSharpDataType]Start
				--GENWHEN[String]Start 
			and (@i_GENVALCamelcalculatedDBStoredProcParameterPrefixGENVALReportParamName is null or @i_GENVALCamelcalculatedDBStoredProcParameterPrefixGENVALReportParamName = GENVALcalculatedSqlServerSingleQuoteDefaultValue or  GENVALcalculatedTargetLookupObjImplementationObjNameGENVALcalculatedTargetObjectName.GENVALcalculatedTargetPropertyName like '%' + @i_GENVALCamelcalculatedDBStoredProcParameterPrefixGENVALReportParamName + '%')
				--GENWHEN[String]End
				--GENElseStart  
				
				--GENIF[calculatedFKObjectName=TriStateFilter]Start
				--TriStateFilter GENVALName @GENVALName_TriStateFilterValue
			and (@i_GENVALCamelcalculatedDBStoredProcParameterPrefixGENVALReportParamName is null or @i_GENVALCamelcalculatedDBStoredProcParameterPrefixGENVALReportParamName = '00000000-0000-0000-0000-000000000000' or @GENVALName_TriStateFilterValue = -1 or @GENVALName_TriStateFilterValue = GENVALcalculatedTargetLookupObjImplementationObjNameGENVALcalculatedTargetObjectName.GENVALcalculatedTargetPropertyName)
				--GENIF[calculatedFKObjectName=TriStateFilter]End
				--GENIF[calculatedFKObjectName=DateGreaterThanFilter]Start
				--DateGreaterThanFilter GENVALName @GENVALName_DateGreaterThanFilterUtcDateTimeValue
			and (@i_GENVALCamelcalculatedDBStoredProcParameterPrefixGENVALReportParamName is null or @i_GENVALCamelcalculatedDBStoredProcParameterPrefixGENVALReportParamName = '00000000-0000-0000-0000-000000000000' or @GENVALName_DateGreaterThanFilterUtcDateTimeValue < GENVALcalculatedTargetLookupObjImplementationObjNameGENVALcalculatedTargetObjectName.GENVALcalculatedTargetPropertyName)
				--GENIF[calculatedFKObjectName=DateGreaterThanFilter]End
				
				--GENIF[calculatedFKObjectName!=TriStateFilter]Start
				--GENIF[calculatedFKObjectName!=DateGreaterThanFilter]Start
			and (@i_GENVALCamelcalculatedDBStoredProcParameterPrefixGENVALReportParamName is null or @i_GENVALCamelcalculatedDBStoredProcParameterPrefixGENVALReportParamName = GENVALcalculatedSqlServerSingleQuoteDefaultValue or @i_GENVALCamelcalculatedDBStoredProcParameterPrefixGENVALReportParamName = GENVALcalculatedTargetLookupObjImplementationObjNameGENVALcalculatedTargetObjectName.GENVALcalculatedTargetPropertyName)
				--GENIF[calculatedFKObjectName!=DateGreaterThanFilter]End
				--GENIF[calculatedFKObjectName!=TriStateFilter]End

				--GENElseEnd 
				--GENCASE[calculatedCSharpDataType]End
		--GENIF[calculatedIsTargetObjectAvailable=true]End
		--GENIF[calculatedIsTargetObjectAvailable=false]Start
			 	--GENCASE[calculatedCSharpDataType]Start
				--GENWHEN[String]Start 
			and (@i_GENVALCamelcalculatedDBStoredProcParameterPrefixGENVALReportParamName is null or @i_GENVALCamelcalculatedDBStoredProcParameterPrefixGENVALReportParamName = GENVALcalculatedSqlServerSingleQuoteDefaultValue or  GENVALObjectName.GENVALReportParamName like '%' + @i_GENVALCamelcalculatedDBStoredProcParameterPrefixGENVALReportParamName + '%')
				--GENWHEN[String]End
				--GENElseStart  
			and (@i_GENVALCamelcalculatedDBStoredProcParameterPrefixGENVALReportParamName is null or @i_GENVALCamelcalculatedDBStoredProcParameterPrefixGENVALReportParamName = GENVALcalculatedSqlServerSingleQuoteDefaultValue or @i_GENVALCamelcalculatedDBStoredProcParameterPrefixGENVALReportParamName = GENVALObjectName.GENVALReportParamName)
				--GENElseEnd 
				--GENCASE[calculatedCSharpDataType]End
		--GENIF[calculatedIsTargetObjectAvailable=false]End
        --GENLOOPReportParamEnd    
   
	) AS TBL
	WHERE 
		ROWNUMBER BETWEEN ((@i_iPageNumber - 1) * @i_iItemCountPerPage + 1) AND (@i_iPageNumber * @i_iItemCountPerPage) 
		 
	
		  */
