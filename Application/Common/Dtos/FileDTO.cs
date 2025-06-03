using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Dtos
{
    public class FileDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string Path { get; set; } = string.Empty;
    }
}
