using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Helpers
{
    public class QueryObject
    {
        public string CategoryName { get; set; } = string.Empty;
        public string SortBy { get; set; } = string.Empty;
        public int MonthNumber { get; set; }
        public bool IsDecending { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}