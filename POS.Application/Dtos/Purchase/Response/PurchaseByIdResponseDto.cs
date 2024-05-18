namespace POS.Application.Dtos.Purchase.Response
{
    public class PurchaseByIdResponseDto
    {
        public int PurchaseId { get; set; } 
        public string? Observable { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Igv { get; set; }
        public decimal TotalAmount { get; set; }
        public int WarehouseId { get; set; }
        public int ProviderId { get; set; }

        public virtual ICollection<PurcharseDetailByIdResponseDto> PurcharseDetails { get; set; } = null!;
    }
}
