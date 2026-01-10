namespace Application.Documents.Analysis.Dtos
{
    public record ParsedDocumentResultDto
    {
        public Guid? CompanyId { get; init; }
        public string? CompanyName { get; init; }
        public DateTime? Validity { get; init; } = DateTime.MinValue;
        public string? EmployeeFullName { get; init; }
        public string? StandardCode { get; init; }
        public string? CertificateNumber { get; init; }
    }

}
