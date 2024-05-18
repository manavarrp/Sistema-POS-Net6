using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Commons.Bases.Request;
using POS.Application.Dtos.Product.Request;
using POS.Application.Interfaces;
using POS.Application.Services;
using POS.Utilities.Static;

namespace POS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductApplication _productApplication;
        private readonly IGenerateExcelApplication _generateExcelApplication;
        private readonly IProductStockApplication _productStockApplication;

        public ProductController(IProductApplication productApplication, IGenerateExcelApplication generateExcelApplication, IProductStockApplication productStockApplication)
        {
            _productApplication = productApplication;
            _generateExcelApplication = generateExcelApplication;
            _productStockApplication = productStockApplication;
        }

        [HttpGet]
        public async Task<IActionResult> ListProduct([FromQuery] BaseFiltersRequest filters)
        {
            var response = await _productApplication.ListProducts(filters);


            if ((bool)filters.Download!)
            {
                var columnsName = ExcelColumnsName.GetCColumnsProducts();
                var fileBytes = _generateExcelApplication.GenerateToExcel(response.Data!, columnsName);
                return File(fileBytes, ContentType.ContentTypeExcel);
            }
            return Ok(response);
        }

        [HttpGet("{productId:int}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            var response = await _productApplication.GetProductById(productId);
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterProduct([FromForm] ProductRequestDto requestDto)
        {
            var response = await _productApplication.RegisterProduct(requestDto);
            return Ok(response);
        }

        [HttpPut("Edit/{productId:int}")]
        public async Task<IActionResult> EditProduct(int productId, [FromForm] ProductRequestDto requestDto)
        {
            var response = await _productApplication.EditProduct(productId, requestDto);
            return Ok(response);
        }

        [HttpGet("ProductStockByWarehouse/{productId:int}")]
        public async Task<IActionResult> ProductStockByWarehouse(int productId)
        {
            var response = await _productStockApplication.GetProductStockByWarehouse(productId);
            return Ok(response);
        }

        [HttpPut("Remove/{productId:int}")]
        public async Task<IActionResult> RemoveProvider(int productId)
        {
            var response = await _productApplication.RemoveProduct(productId);
            return Ok(response);

        }

    }
}
