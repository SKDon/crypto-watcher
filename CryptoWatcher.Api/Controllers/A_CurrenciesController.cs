﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoWatcher.Api.ResponseExamples;
using CryptoWatcher.Application.Responses;
using CryptoWatcher.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace CryptoWatcher.Api.Controllers
{
    // ReSharper disable once InconsistentNaming
    public class A_CurrenciesController : Controller
    {
        private readonly CurrencyService _currencyService;

        public A_CurrenciesController(CurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        /// <summary>
        /// Get all currencies
        /// </summary>
        [HttpGet]
        [Route("currencies")]
        [SwaggerResponse(200, Type = typeof(List<CurrencyResponse>))]       
        [SwaggerResponse(500, Type = typeof(ErrorResponse))]
        [SwaggerResponseExample(200, typeof(CurrencyListResponseExample))]
        [SwaggerResponseExample(500, typeof(InternalServerErrorExample))]
        [SwaggerOperation(Tags = new[] { "Currencies" }, OperationId = "Currencies_GetAllCurrencies")]
        public async Task<IActionResult> GetAllCurrencies()
        {
            // Reponse
            var response = await _currencyService.GetAllCurrencies();

            // Return
            return Ok(response);
        }

        /// <summary>
        /// Get currency
        /// </summary>
        [HttpGet]
        [Route("currencies/{currencyId}", Name = "Currencies_GetCurrency")]
        [SwaggerResponse(200, Type = typeof(CurrencyResponse))]
        [SwaggerResponse(404, Type = typeof(ErrorResponse))]
        [SwaggerResponse(500, Type = typeof(ErrorResponse))]
        [SwaggerResponseExample(200, typeof(CurrencyResponseExample))]
        [SwaggerResponseExample(404, typeof(NotFoundExample))]
        [SwaggerResponseExample(500, typeof(InternalServerErrorExample))]
        [SwaggerOperation(Tags = new[] { "Currencies" }, OperationId = "Currencies_GetCurrency")]
        public async Task<IActionResult> GetCurrency(string currencyId)
        {
            // Reponse
            var response = await _currencyService.GetCurrency(currencyId);

            // Return
            return Ok(response);
        }
    }
}

