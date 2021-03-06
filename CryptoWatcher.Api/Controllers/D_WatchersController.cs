﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoWatcher.Api.RequestExamples;
using CryptoWatcher.Application.Requests;
using CryptoWatcher.Api.ResponseExamples;
using CryptoWatcher.Application.Responses;
using CryptoWatcher.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace CryptoWatcher.Api.Controllers
{
    // ReSharper disable once InconsistentNaming
    public class D_WatchersController : Controller
    {
        private readonly WatcherService _watcherService;

        public D_WatchersController(WatcherService watcherService)
        {
            _watcherService = watcherService;
        }

        /// <summary>
        /// Get all watchers
        /// </summary>
        [HttpGet]
        [Route("users/{userId}/watchers")]
        [SwaggerResponse(200, Type = typeof(List<WatcherResponse>))]       
        [SwaggerResponse(500, Type = typeof(ErrorResponse))]
        [SwaggerResponseExample(200, typeof(WatcherListResponseExample))]
        [SwaggerResponseExample(500, typeof(InternalServerErrorExample))]
        [SwaggerOperation(Tags = new[] { "Watchers" }, OperationId = "Watchers_GetAllWatchers")]
        public async Task<IActionResult> GetAllWatchers(string userId, string currencyId = null, string indicatorId = null)
        {
            // Reponse
            var response = await _watcherService.GetAllWatchers(userId, currencyId, indicatorId);

            // Return
            return Ok(response);
        }

        /// <summary>
        /// Get watcher
        /// </summary>
        [HttpGet]
        [Route("watchers/{watcherId}", Name = "Watchers_GetWatcher")]
        [SwaggerResponse(200, Type = typeof(WatcherResponse))]
        [SwaggerResponse(404, Type = typeof(ErrorResponse))]
        [SwaggerResponse(500, Type = typeof(ErrorResponse))]
        [SwaggerResponseExample(200, typeof(WatcherResponseExample))]
        [SwaggerResponseExample(404, typeof(NotFoundExample))]
        [SwaggerResponseExample(500, typeof(InternalServerErrorExample))]
        [SwaggerOperation(Tags = new[] { "Watchers" }, OperationId = "Watchers_GetWatcher")]
        public async Task<IActionResult> GetWatcher(string watcherId)
        {
            // Reponse
            var response = await _watcherService.GetWatcher(watcherId);

            // Return
            return Ok(response);
        }

        /// <summary>
        /// Add watcher
        /// </summary>
        [HttpPost]
        [Route("watchers")]
        [SwaggerResponse(201, Type = typeof(WatcherResponse))]
        [SwaggerResponse(400, Type = typeof(ErrorResponse))]
        [SwaggerResponse(404, Type = typeof(ErrorResponse))]
        [SwaggerResponse(409, Type = typeof(ErrorResponse))]
        [SwaggerResponse(422, Type = typeof(ValidationResponse))]
        [SwaggerResponse(500, Type = typeof(ErrorResponse))]
        [SwaggerResponseExample(201, typeof(WatcherResponseExample))]
        [SwaggerResponseExample(400, typeof(BadRequestExample))]
        [SwaggerResponseExample(404, typeof(NotFoundExample))]
        [SwaggerResponseExample(409, typeof(ConflictExample))]
        [SwaggerResponseExample(422, typeof(ValidationFailedExample))]
        [SwaggerResponseExample(500, typeof(InternalServerErrorExample))]
        [SwaggerRequestExample(typeof(AddWatcherRequest), typeof(AddWatcherRequestExample))]
        [SwaggerOperation(Tags = new[] { "Watchers" }, OperationId = "Watchers_AddWatcher")]
        public async Task<IActionResult> AddWatcher([FromBody]AddWatcherRequest request)
        {
            // Reponse
            var response = await _watcherService.AddWatcher(request);

            // Return
            return CreatedAtRoute("Watchers_GetWatcher", new { response.WatcherId }, response);
        }

        /// <summary>
        /// Update watcher
        /// </summary>
        [HttpPut]
        [Route("watchers/{watcherId}")]
        [SwaggerResponse(200, Type = typeof(WatcherResponse))]
        [SwaggerResponse(400, Type = typeof(ErrorResponse))]
        [SwaggerResponse(409, Type = typeof(ErrorResponse))]
        [SwaggerResponse(422, Type = typeof(ValidationResponse))]
        [SwaggerResponse(500, Type = typeof(ErrorResponse))]
        [SwaggerResponseExample(200, typeof(WatcherResponseExample))]
        [SwaggerResponseExample(400, typeof(BadRequestExample))]
        [SwaggerResponseExample(409, typeof(ConflictExample))]
        [SwaggerResponseExample(422, typeof(ValidationFailedExample))]
        [SwaggerResponseExample(500, typeof(InternalServerErrorExample))]
        [SwaggerRequestExample(typeof(UpdateWatcherRequest), typeof(UpdateWatcherRequestExample))]
        [SwaggerOperation(Tags = new[] { "Watchers" }, OperationId = "Watchers_UpdateWatcher")]
        public async Task<IActionResult> UpdateWatcher(string watcherId, [FromBody]UpdateWatcherRequest request)
        {
            // Reponse
            request.WatcherId = watcherId;
            var response = await _watcherService.UpdateWatcher(request);

            // Return
            return Ok(response);
        }
    }
}

