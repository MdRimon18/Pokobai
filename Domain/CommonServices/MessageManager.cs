using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CommonServices
{
    public static class MessageManager
    {
        public static string SaveSuccess { get; set; } = "Save Successfull!";
        public static string UpdateSuccess { get; set; } = "Update Successfull!";   
        public static string SaveFaild { get; set; } = "Save Faild!";
        public static string UpdateFaild { get; set; } = "Update Faild!";
        public static string DeleteFaild { get; set; } = "Failed to delete with ID";
        public static string Exist { get; set; } = "is Already Exist!";
        public static string NotFound { get; set; } = "Not Found!";
    }
}
