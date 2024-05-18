using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Commons.Bases.Request;
using POS.Application.Dtos.Purchase.Request;
using POS.Application.Interfaces;
using POS.Utilities.Static;

namespace POS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseApplication _purchaseApplication;
        private readonly IGenerateExcelApplication _generateExcelApplication;

        public PurchaseController(IPurchaseApplication purchaseApplication, IGenerateExcelApplication generateExcelApplication)
        {
            _purchaseApplication = purchaseApplication;
            _generateExcelApplication = generateExcelApplication;
        }

        [HttpGet]
        public async Task<IActionResult> ListPurchases([FromQuery] BaseFiltersRequest filters)
        {
            var response = await _purchaseApplication.ListPurchases(filters);

            if ((bool)filters.Download!)
            {
                var columnsNames = ExcelColumnsName.GetColumnsPurchases();
                var fileBytes = _generateExcelApplication.GenerateToExcel(response.Data!, columnsNames);
                return File(fileBytes, ContentType.ContentTypeExcel);
            }
            return Ok(response);
        }

        [HttpGet("{purcharseId:int}")]
        public async Task<IActionResult> PurcharseById(int purcharseId)
        {
            var response = await _purchaseApplication.GetPurchaseById(purcharseId);
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterPurchase([FromBody] PurchaseRequestDto requestDto)
        {
            var response = await _purchaseApplication.RegisterPurchase(requestDto);
            return Ok(response);
        }

        [HttpPut("Cancel/{purcharseId:int}")]
        public async Task<IActionResult> CancelPurchase(int purcharseId)
        {
            var response = await _purchaseApplication.CancelPurcharse(purcharseId);
            return Ok(response);
        }
    }
}
