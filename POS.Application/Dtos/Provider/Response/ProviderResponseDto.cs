namespace POS.Application.Dtos.Provider.Response
{
    public class ProviderResponseDto
    {
        public int ProviderId { get; set; }
        public string? Name { get; set; }
        public string? Email {  get; set; }
        public string? DocumentType { get; set; }
        public string DocumentNumber { get; set; } = null!;
        public string? Address { get; set; }
        public string Phone { get; set; } = null!;
        public DateTime? AuditCreateDate { get; set; }
        public int State { get; set; }
        public string? StateProvider {  get; set; }
    }
}
