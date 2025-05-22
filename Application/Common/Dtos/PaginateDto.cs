using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Dtos
{
    public class PaginateDTO
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? Filter { get; set; }
    }
}
