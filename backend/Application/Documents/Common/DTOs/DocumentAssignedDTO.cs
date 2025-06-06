using Application.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Documents.Common.DTOs
{
    public class DocumentAssignedDTO : PaginateDTO
    {
        public Guid? AssignedTo { get; set; }
    }
}
