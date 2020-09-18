using Core.Enums;
using Core.Models;
using Core.Services;
using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyConverter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ILogger<CurrencyController> _logger;
        private readonly CurrencyService _service;
        private readonly CurrencyConversionDBContext _context;

        public CurrencyController(
            ILogger<CurrencyController> logger, 
            CurrencyService service,
            CurrencyConversionDBContext context)
        {
            _logger = logger;
            _service = service;
            _context = context;
        }

        /// <summary>
        /// Get the available conversion rates
        /// </summary>
        /// <returns></returns>
        [HttpGet("{currency}")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(CurrencyName currency)
        {
            var moo = await _service.GetConversionRates(currency);
            return Ok(moo);
        }

        /// <summary>
        /// Get available audit entries
        /// </summary>
        /// <param name="from">Date from</param>
        /// <param name="to">Date to</param>
        /// <returns><see cref="CurrencyConversion"/></returns>
        [HttpGet("audit/{from}/{to}")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(DateTime from, DateTime to)
        {
            var records = _context.CurrencyConversion.Where(x => x.DateTime >= from && x.DateTime <= to).ToList().OrderByDescending(x => x.ID);
            return Ok(records);
        }


        [HttpPost("audit/add")]
        public async Task<IActionResult> AddToAudit(
            [FromBody]CurrencyConversion @currencyConversion)
        {
            await AddToAudit(@currencyConversion, 0);

            return Ok();
        }

        private async Task AddToAudit(CurrencyConversion @currencyConversion, int tryCount = 0)
        {
            var newID = _context.CurrencyConversion.Select(x => x.ID).Max() + 1 + tryCount;

            try
            {
                @currencyConversion.ID = newID;

                @currencyConversion.ID = newID;
                _context.CurrencyConversion.Add(@currencyConversion);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add to audit for {@currencyConversion.ID}");
                if (tryCount >= 3)
                    throw ex;
                await AddToAudit(@currencyConversion, tryCount + 1);
            }

        }
    }
}
