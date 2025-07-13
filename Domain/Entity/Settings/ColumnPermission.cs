
using System.Data;

namespace Domain.Entity.Settings
{

    public class ColumnPermission<T> where T : class
    {
        public string ColumnName { get; set; }
        public string ColumnProprtyName { get; set; }
        public bool IsShow { get; set; } = false;
        public Func<T, object> ValueAccessor { get; set; }
    }

}