using AutoMapper;
using CardFile.BLL.DTO;
using CardFile.BLL.Interfaces;
using CardFile.Contracts.Requests.Queries;
using CardFile.Contracts.Response;
using CardFile.Contracts.Responses;
using CardFile.WebAPI.Contracts.Request;
using CardFile.WebAPI.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CardFile.WebAPI.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CardController : ControllerBase
    {
        private readonly ICardFileService _cardFileService;
        private readonly IMapper _mapper;
        private readonly ILogger<CardController> _logger;
        private readonly IUriService _uriService;

        public CardController(
            ICardFileService cardFileService, 
            IMapper mapper, 
            ILogger<CardController> logger, 
            IUriService uriService)
        {
            _cardFileService = cardFileService;
            _mapper = mapper;
            _logger = logger;
            _uriService = uriService;
        }

        /// <summary>
        /// Create a card file
        /// </summary>
        /// <param name="formFiles"></param>
        /// <param name="request"></param>
        /// <response code="200">Created a card in the system</response>
        /// <response code="400">Unable to create a card due to validation error</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("card")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> CreateCardFile(IFormFile formFiles, [FromQuery] CardFileRequest request)
        {
            if (formFiles == null)
            {
                _logger.LogWarning("File not be load (null or not correct format)");
                return BadRequest("File can not be load");
            }

            var mappedCardFile = _mapper.Map<CardFileDTO>(request);

            await _cardFileService.AddCardFileAsync(formFiles, mappedCardFile);

            return Ok("Successfully created");
        }

        /// <summary>
        /// Update a card file
        /// </summary>
        /// <param name="cardFileId"></param>
        /// <param name="formFiles"></param>
        /// <param name="request"></param>
        /// <response code="200">Updated a card in the system</response>
        /// <response code="400">Unable to update a card due to validation error</response>
        /// <response code="401">Unauthorized</response>
        [HttpPut("card/{cardFileId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCardFile(
            int cardFileId, 
            IFormFile formFiles, 
            [FromQuery] CardFileRequest request)
        {
            if (formFiles == null)
                return BadRequest("File can not be load");

            var mappedCardFile = _mapper.Map<CardFileDTO>(request);

            await _cardFileService.UpdateCardFileAsync(cardFileId, formFiles, mappedCardFile);

            return Ok("Successfully updated");
        }

        /// <summary>
        /// Delete a card file by id
        /// </summary>
        /// <param name="cardFileId"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Deleted a card in the system</response>
        /// <response code="404">Unable to delete a card due error</response>
        /// <response code="401">Unauthorized</response>
        [HttpDelete("card/{cardFileId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCardFileById(int cardFileId, CancellationToken cancellationToken)
        {
            if (cardFileId <= 0)
                return NotFound();

            await _cardFileService.DeleteByIdAsync(cardFileId, cancellationToken); // TODO: Handle with middleware?

            return Ok("Successfully deleted");
        }

        /// <summary>
        /// Returns all the card files in the system
        /// </summary>
        /// <response code="200">Get all card files</response>
        /// <response code="404">Not Found any card files</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("cards")]
        [AllowAnonymous]
        public IActionResult GetAllCardFiles([FromQuery] PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            var cardFiles = _cardFileService.GetAll(pagination);          
            var mappedCardFiles = _mapper.Map<List<CardFileResponse>>(cardFiles);
            var cardFilesPaginationResponse = new PagedResponse<CardFileResponse>(mappedCardFiles);

            if (cardFiles == null || cardFilesPaginationResponse == null)
                return NotFound();

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
                return Ok(cardFilesPaginationResponse);

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService,pagination, mappedCardFiles);

            return Ok(paginationResponse);
        }

        /// <summary>
        /// Returns a card file by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Get a card file</response>
        /// <response code="404">Not Found any card file</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("cards/{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetCardFileByIdAsync(int id, CancellationToken cancellationToken)
        {
            var cardFile = await _cardFileService.GetByIdAsync(id, cancellationToken);
            var mappedCardFile = _mapper.Map<CardFileResponse>(cardFile);
            var cardFilesPaginationResponse = new Response<CardFileResponse>(mappedCardFile);

            if (cardFile == null || cardFilesPaginationResponse == null)
                return NotFound();

            return Ok(cardFilesPaginationResponse);
        }

        /// <summary>
        /// Returns a card files by date of creation of a cards
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="paginationQuery"></param>
        /// <response code="200">Get all card files</response>
        /// <response code="404">Not Found any card files</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("cards/dateTime/{dateTime}")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetCardFilesByDateOfCreation(
            DateTime dateTime, 
            [FromQuery] PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            var cardFiles = _cardFileService.GetCardsByDateOfCreation(dateTime, pagination);
            var mappedCardFiles = _mapper.Map<List<CardFileResponse>>(cardFiles);
            var cardFilesPaginationResponse = new PagedResponse<CardFileResponse>(mappedCardFiles);

            if (cardFiles == null || cardFilesPaginationResponse == null)
                return NotFound();

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
                return Ok(cardFilesPaginationResponse);

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, mappedCardFiles);

            return Ok(paginationResponse);
        }

        /// <summary>
        /// Returns a card files by language of creation of a cards
        /// </summary>
        /// <param name="language"></param>
        /// <param name="paginationQuery"></param>
        /// <response code="200">Get all card files</response>
        /// <response code="404">Not Found any card files</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("cards/language/{language}")]
        [Authorize(Roles = "Admin,User")]

        public IActionResult GetCardFilesByLanguage(string language, [FromQuery] PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            var cardFiles = _cardFileService.GetCardsByLanguage(language, pagination);
            var mappedCardFiles = _mapper.Map<List<CardFileResponse>>(cardFiles);
            var cardFilesPaginationResponse = new PagedResponse<CardFileResponse>(mappedCardFiles);

            if (cardFiles == null || cardFilesPaginationResponse == null)
                return NotFound();
                
            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
                return Ok(cardFilesPaginationResponse);
            
            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, mappedCardFiles);

            return Ok(paginationResponse);
        }

        /// <summary>
        /// Download file by card id
        /// </summary>
        /// <param name="cardFileId"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Download file</response>
        /// <response code="404">Not Found any file</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("file/{cardFileId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DownloadFile(int cardFileId, CancellationToken cancellationToken)
        {
            var filePath = await _cardFileService.GetFilePathAsync(cardFileId, cancellationToken);

            var bytes = await System.IO.File.ReadAllBytesAsync(filePath);

            return File(bytes, "text/plain", Path.GetFileName(filePath));
        }
    }
}