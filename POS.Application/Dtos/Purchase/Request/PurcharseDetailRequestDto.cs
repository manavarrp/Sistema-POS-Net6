namespace POS.Application.Dtos.Purchase.Request
{
    public class PurcharseDetailRequestDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPurchasePrice { get; set; }
        public decimal Total { get; set; }

    }
}
