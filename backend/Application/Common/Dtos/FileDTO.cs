using Microsoft.AspNetCore.Http;

namespace Application.Common.Dtos
{
    public class FileDTO
    {
        public string Name { get; set; } = string.Empty;
        public Guid UploadedById { get; set; }
        public DateTime UploadDate { get; set; }
        public IFormFile File { get; set; } = default!;
        public string Path { get; set; } = string.Empty;
    }
}
