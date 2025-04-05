using IntegrationService.Services;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationService.Controllers
{
    [ApiController]
    [Route("api/v1.0/import")]
    public class ImportController : ControllerBase
    {
        private readonly IImportService _importService;

        public ImportController(IImportService importService)
        {
            _importService = importService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ImportTransactions([FromForm] IFormFile file, [FromForm] long userId, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Файл не выбран или пуст");

            var importResult = await _importService.ImportTransactionsAsync(file, userId, cancellationToken);
            return Ok(importResult);
        }
    }
}
