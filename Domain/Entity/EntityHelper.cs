using Domain.CommonServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public static class EntityHelper
    {
        public static void SetCreateAuditFields(BaseEntity entity)
        {
            entity.EntryDateTime = DateTime.UtcNow;
            entity.EntryBy = UserInfo.UserId;
            entity.Status = "Active";
        }

        public static void SetUpdateAuditFields(BaseEntity entity)
        {
            entity.LastModifyDate = DateTime.UtcNow;
            entity.LastModifyBy = UserInfo.UserId;
        }

        public static void SetDeleteAuditFields(BaseEntity entity)
        {
            entity.DeletedDate = DateTime.UtcNow;
            entity.DeletedBy = UserInfo.UserId;
            entity.Status = "Deleted";
        }
      
    }

}
