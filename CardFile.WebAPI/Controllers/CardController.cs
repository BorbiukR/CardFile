using AutoMapper;
using CardFile.BLL.DTO;
using CardFile.BLL.Interfaces;
using CardFile.Contracts.Response;
using CardFile.WebAPI.Contracts.Request;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public CardController(ICardFileService cardFileService, IMapper mapper, ILogger<CardController> logger)
        {
            _cardFileService = cardFileService;
            _mapper = mapper;
            _logger = logger;
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

            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors.Select(c => c.ErrorMessage)));

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
        public async Task<IActionResult> UpdateCardFile(int cardFileId, IFormFile formFiles, [FromQuery] CardFileRequest request)
        {
            if (formFiles == null)
                return BadRequest("File can not be load");

            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors.Select(c => c.ErrorMessage)));

            var mappedCardFile = _mapper.Map<CardFileDTO>(request);

            await _cardFileService.UpdateCardFileAsync(cardFileId, formFiles, mappedCardFile);

            return Ok("Successfully updated");
        }

        /// <summary>
        /// Delete a card file by id
        /// </summary>
        /// <param name="cardFileId"></param>
        /// <response code="200">Deleted a card in the system</response>
        /// <response code="404">Unable to delete a card due error</response>
        /// <response code="401">Unauthorized</response>
        [HttpDelete("card/{cardFileId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCardFileById(int cardFileId)
        {
            if (cardFileId <= 0)
                return NotFound();

            await _cardFileService.DeleteByIdAsync(cardFileId);

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
        public IActionResult GetAllCardFiles(CancellationToken cancellationToken)
        {
            var cardFiles = _cardFileService.GetAll(cancellationToken);
            var mappedCardFiles = _mapper.Map<List<CardFileResponse>>(cardFiles);

            if (cardFiles == null || mappedCardFiles == null)
                return NotFound();

            return Ok(mappedCardFiles);
        }

        /// <summary>
        /// Returns a card file by id
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Get a card file</response>
        /// <response code="404">Not Found any card file</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("cards/{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetCardFileByIdAsync(int id)
        {
            var cardFile = await _cardFileService.GetByIdAsync(id);
            var mappedCardFile = _mapper.Map<CardFileResponse>(cardFile);

            if (cardFile == null || mappedCardFile == null)
                return NotFound();

            return Ok(mappedCardFile);
        }

        /// <summary>
        /// Returns a card files by date of creation of a cards
        /// </summary>
        /// <param name="dateTime"></param>
        /// <response code="200">Get all card files</response>
        /// <response code="404">Not Found any card files</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("cards/dateTime/{dateTime}")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetCardFilesByDateOfCreation(DateTime dateTime)
        {
            var cardFiles = _cardFileService.GetCardsByDateOfCreation(dateTime);
            var mappedCardFiles = _mapper.Map<List<CardFileResponse>>(cardFiles);

            if (cardFiles == null || mappedCardFiles == null)
                return NotFound();

            return Ok(mappedCardFiles);
        }

        /// <summary>
        /// Returns a card files by language of creation of a cards
        /// </summary>
        /// <param name="language"></param>
        /// <response code="200">Get all card files</response>
        /// <response code="404">Not Found any card files</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("cards/language/{language}")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetCardFilesByLanguage(string language)
        {
            var cardFiles = _cardFileService.GetCardsByLanguage(language);
            var mappedCardFiles = _mapper.Map<List<CardFileResponse>>(cardFiles);

            if (cardFiles == null || mappedCardFiles == null)
                return NotFound();

            return Ok(mappedCardFiles);
        }

        /// <summary>
        /// Download file by card id
        /// </summary>
        /// <param name="cardFileId"></param>
        /// <response code="200">Download file</response>
        /// <response code="404">Not Found any file</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("file/{cardFileId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DownloadFile(int cardFileId)
        {
            var filePath = await _cardFileService.GetFilePath(cardFileId);

            var bytes = await System.IO.File.ReadAllBytesAsync(filePath);

            return File(bytes, "text/plain", Path.GetFileName(filePath));
        }
    }
}