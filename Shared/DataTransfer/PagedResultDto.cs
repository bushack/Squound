using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransfer
{
    public class PagedResultDto<T>
    {
        public int TotalItems { get; set; } = 0;

        public int PageSize { get; set; } = 0;

        public int CurrentPage { get; set; } = 0;

        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);

        public List<T> Items { get; set; } = [];
    }
}
