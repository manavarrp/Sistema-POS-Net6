namespace POS.Application.Dtos.ProductStock.Response
{
    public class ProductStockByWarehouseResponseDto
    {
        public string? Warehouse { get; set; }
        public int CurrentStock { get; set; }
        public decimal PurchasePrice { get; set; }

    }
}
