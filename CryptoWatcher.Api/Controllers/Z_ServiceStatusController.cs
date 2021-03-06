﻿using System.Threading.Tasks;
using CryptoWatcher.Api.ResponseExamples;
using CryptoWatcher.Application.Responses;
using CryptoWatcher.Application.Services;
using CryptoWatcher.Application.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace CryptoWatcher.Api.Controllers
{
    // ReSharper disable once InconsistentNaming
    [AllowAnonymous]
    public class Z_ServiceStatusController : Controller
    {
        private readonly StatusService _statusService;

        public Z_ServiceStatusController(StatusService statusService)
        {
            _statusService = statusService;
        }

        [HttpGet]
        [ActionName("ResourceNotFound")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ResourceNotFound()
        {
            var errorResponse = new ErrorResponse(nameof(Message.NotFound), 404, Message.NotFound);
            return NotFound(errorResponse);
        }

        /// <summary>
        /// Get version
        /// </summary>
        [HttpGet]
        [Route("version")]
        [SwaggerResponse(200, Type = typeof(VersionResponse))]       
        [SwaggerResponse(500, Type = typeof(ErrorResponse))]
        [SwaggerResponseExample(200, typeof(VersionResponseExample))]
        [SwaggerResponseExample(500, typeof(InternalServerErrorExample))]
        [SwaggerOperation(Tags = new[] { "Service status" }, OperationId = "ServiceStatus_GetVersion")]
        public async Task<IActionResult> GetVersion()
        {
            // Reponse
            var response = await _statusService.GetVersion();
                
            // Return
            return Ok(response);
        }

        /// <summary>
        /// Get health
        /// </summary>
        [HttpGet]
        [Route("health")]
        [SwaggerResponse(200, Type = typeof(HealthResponse))]
        [SwaggerResponse(500, Type = typeof(ErrorResponse))]
        [SwaggerResponseExample(200, typeof(HealthResponseExample))]
        [SwaggerResponseExample(500, typeof(InternalServerErrorExample))]
        [SwaggerOperation(Tags = new[] { "Service status" }, OperationId = "ServiceStatus_GetHealth")]
        public async Task<IActionResult> GetHealth()
        {
            // Reponse
            var response = await _statusService.GetHealth();

            // Return
            return Ok(response);
        }
    }
}

