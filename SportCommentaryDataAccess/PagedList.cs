using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportCommentaryDataAccess
{
    public class PagedList<T>
    {
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages
        {
            get
            {
                return CalculateTotalPages(TotalItems, PageSize);
            }
        }
        public int PageSize { get; set; }

        public IList<T> Items { get; set; }

        public PagedList()
        {
            Items = new List<T>();
            PageSize = 10;
        }

        private int CalculateTotalPages(int count, int pageSze)
        {
            return (int)Math.Ceiling(decimal.Divide(count, pageSze));
        }
    }
}
