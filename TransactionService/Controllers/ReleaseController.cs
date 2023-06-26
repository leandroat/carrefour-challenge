using TransactionService.Models;
using TransactionService.Services;
using Microsoft.AspNetCore.Mvc;

namespace TransactionService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReleaseController : ControllerBase
    {
        private readonly ILogger<ReleaseController> _logger;
        private readonly IReleaseProvider _releaseService;

        public ReleaseController(ILogger<ReleaseController> logger, IReleaseProvider releaseService)
        {
            _logger = logger;
            _releaseService = releaseService;
        }

        [HttpGet]
        [Route("{id}")]
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
        [Route("{id}")]
        public async Task<IActionResult> DeleteTransaction([FromRoute] string transactionId)
        {
            if (string.IsNullOrEmpty(transactionId))
                return BadRequest();

            try
            {
                bool response = await _releaseService.RemoveTransaction(transactionId);

                if(response)
                    return Ok();

            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to remove transaction: " + ex.Message);
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetBalance()
        {
            try
            {
                Balance response = await _releaseService.BalanceProcess(DateTime.Now);

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