using TransactionService.Models;
using TransactionService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace TransactionService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly IReleaseProvider _releaseService;

        public TransactionController(ILogger<TransactionController> logger, IReleaseProvider releaseService)
        {
            _logger = logger;
            _releaseService = releaseService;
        }

        [HttpGet]
        [Route("{transactionId}")]
        public async Task<IActionResult> GetTransaction([FromRoute] string transactionId)
        {
            if (string.IsNullOrEmpty(transactionId))
                return BadRequest();

            try
            {
                Transaction response = await _releaseService.GetTransaction(transactionId);

                if (string.IsNullOrEmpty(response.Id))
                    return NoContent();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get transaction info: " + ex.Message);
            }

            return BadRequest();
        }


        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] Transaction request)
        {
            if (request == null)
                return BadRequest();

            try
            {
                Transaction response = await _releaseService.CreateTransaction(request);

                if(response != null && !string.IsNullOrEmpty(response.Id))
                    return Ok(response);

            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to create transaction: " + ex.Message);
            }

            return BadRequest();
        }

        [HttpDelete]
        [Route("{transactionId}")]
        public async Task<IActionResult> DeleteTransaction([FromRoute] string transactionId)
        {
            if (string.IsNullOrEmpty(transactionId))
                return BadRequest();

            try
            {
                bool response = await _releaseService.RemoveTransaction(transactionId);

                if (!response)
                    return NoContent();

                return Ok();

            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to remove transaction: " + ex.Message);
            }

            return BadRequest();
        }

        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance([FromQuery(Name = "date")] string date)
        {
            Balance response = new Balance();

            date = date.Trim();

            try
            {
                var dateTimeObject = DateTime.ParseExact(date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                response = await _releaseService.BalanceProcess(dateTimeObject);

                if (response.Date == null)
                    return NoContent();

                return Ok(response);

            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to process balance: " + ex.Message);
            }

            return BadRequest();
        }
    }
}