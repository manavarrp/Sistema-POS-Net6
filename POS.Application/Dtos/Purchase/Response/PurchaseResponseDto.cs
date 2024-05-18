namespace POS.Application.Dtos.Purchase.Response
{
    public class PurchaseResponseDto
    {
        public int PurchaseId {  get; set; }
        public string? Provider { get; set; }
        public string? Warehouse { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime DateOfPurchase { get; set; }
    }
}
