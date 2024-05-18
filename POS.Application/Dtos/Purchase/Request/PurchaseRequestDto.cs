using POS.Domain.Entities;

namespace POS.Application.Dtos.Purchase.Request
{
    public class PurchaseRequestDto
    {
       
        public string? Observable { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Igv { get; set; }
        public decimal TotalAmount { get; set; }
        public int WarehouseId { get; set; }
        public int ProviderId { get; set; }
        public ICollection<PurcharseDetailRequestDto> PurcharseDetails { get; set; } = null!;
    }
}
