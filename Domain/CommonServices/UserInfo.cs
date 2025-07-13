using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CommonServices
{
    public static class UserInfo
    {
        public static long UserId { get; set; } = 1;
    }
    public static class CompanyInfo
    {
        public static long BranchId { get; set; } = 1;
        public static long CompanyId { get; set; } = 1;
        public static int LanguageId { get; set; } = 1;
        public static int CurrencyId { get; set; } = 1;
        public static long CountryId { get; set; } = 20012;
    }
    public static class SelectedUserRole
    {
        public const long SupperAdminRoleId = 1; // Changed to const
        public const long BusinessOwnerRoleId = 2; // Changed to const
        public const long CustomerRoleId = 10003; // Changed to const
        public const long SupplierRoleId = 10002; // Changed to const
        public const long EmployeeRoleId = 10005; // Changed to const
        public const long DriverRoleId = 10004; // Changed to const
    }
    public static class RoleHelper
    {
        public static string GetRoleName(long roleId)
        {
            return roleId switch
            {
                SelectedUserRole.CustomerRoleId => "Customer",
                SelectedUserRole.SupplierRoleId => "Supplier",
                SelectedUserRole.EmployeeRoleId => "Employee",
                SelectedUserRole.DriverRoleId => "Driver",
                _ => ""
            };
        }
        public static string GetRoleWiseListPageName(long roleId)
        {
            return roleId switch
            {
                SelectedUserRole.CustomerRoleId => "Customers List",
                SelectedUserRole.SupplierRoleId => "Suppliers List",
                SelectedUserRole.EmployeeRoleId => "Employees List",
                SelectedUserRole.DriverRoleId => "Driver List",
                _ => ""
            };
        }
    }
    public static class GlobalPageConfig
    {
        public static int PageNumber { get; set; } = 1;
        public static int PageSize { get; set; } = 1000;
    }
}
