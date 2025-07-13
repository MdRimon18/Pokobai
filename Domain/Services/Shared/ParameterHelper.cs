using Dapper;
using Domain.Entity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Shared
{
    public static class ParameterHelper
    {
        public static void AddAuditParameters(BaseEntity entity, DynamicParameters parameters)
        {

                parameters.Add("@EntryDateTime", entity.EntryDateTime);
                parameters.Add("@EntryBy", entity.EntryBy);
                parameters.Add("@lastModifyDate", entity.LastModifyDate);
                parameters.Add("@lastModifyBy", entity.LastModifyBy);
                parameters.Add("@deletedDate", entity.DeletedDate);
                parameters.Add("@DeletedBy", entity.DeletedBy);
                parameters.Add("@Status", entity.Status);
        }
    }
}   
