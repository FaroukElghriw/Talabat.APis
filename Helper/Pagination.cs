using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Talabat.APis.Helper
{
    public class Pagination<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }

        public IReadOnlyList<T> Data { get; set; }
        public Pagination(int pageindex, int pagesize,IReadOnlyList<T> data,int count)
        {
            PageSize = pagesize;
            PageIndex = pageindex;
            Data = data;
            Count = count;
        }
    }
}
