using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Commons.Bases.Request;
using POS.Application.Dtos.Client.Request;
using POS.Application.Interfaces;
using POS.Utilities.Static;

namespace POS.Api.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientApplication _clientApplication;
        private readonly IGenerateExcelApplication _generateExcelApplication;

        public ClientController(IClientApplication clientApplication, IGenerateExcelApplication generateExcelApplication)
        {
            _clientApplication = clientApplication;
            _generateExcelApplication = generateExcelApplication;
        }

        [HttpGet]
        public async Task<IActionResult> ListClients([FromQuery] BaseFiltersRequest filters)
        {
            var response = await _clientApplication.ListClient(filters);

            if ((bool)filters.Download!)
            {
                var columnsNames = ExcelColumnsName.GetColumnsClients();
                var fileBytes = _generateExcelApplication.GenerateToExcel(response.Data!, columnsNames);
                return File(fileBytes, ContentType.ContentTypeExcel);
            }
            return Ok(response);
        }

        [HttpGet("Select")]
        public async Task<IActionResult> SelectClients()
        {
            var response = await _clientApplication.ListSelectClients();
            return Ok(response);
        }

        [HttpGet("{clientId:int}")]
        public async Task<IActionResult> GetClientById(int clientId)
        {
            var response = await _clientApplication.GetClientById(clientId);
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterClient([FromBody] ClientRequestDto clientRequestDto)
        {
            var response = await _clientApplication.RegisterClient(clientRequestDto);
            return Ok(response);
        }

        [HttpPut("Edit/{clientId:int}")]
        public async Task<IActionResult> EditClient(int clientId, [FromBody] ClientRequestDto clientRequestDto)
        {
            var response = await _clientApplication.EditClient(clientId, clientRequestDto);
            return Ok(response);
        }

        [HttpPut("Remove/{clientId:int}")]
        public async Task<IActionResult> RemoveClient(int clientId)
        {
            var response = await _clientApplication.RemoveClient(clientId);
            return Ok(response);
        }
    }
}
