using AutoMapper;
using CardFile.BLL.DTO;
using CardFile.BLL.Interfaces;
using CardFile.Contracts.Response;
using CardFile.WebAPI.Contracts.Request;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardFile.WebAPI.Controllers
{
    // TODO: переробити формат часу (день, місяць, рік)
    // GetCardFilesByDateOfCreation - не працює коректно 
    [ApiController]
    [Route("api")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CardController : ControllerBase
    {
        private readonly ICardFileService _cardFileService;
        private readonly IMapper _mapper;

        public CardController(ICardFileService cardFileService, IMapper mapper)
        {
            _cardFileService = cardFileService;
            _mapper = mapper;
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCardFile(IFormFile formFiles, [FromQuery] CardFileRequest request)
        {
            if (formFiles == null)
                return BadRequest("file can not be load");

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
        public IActionResult GetAllCardFiles()
        {
            var cardFiles = _cardFileService.GetAll();
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
        [HttpGet("cards/dateTime")]
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
        [HttpGet("cards/language")]
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
        public async Task<IActionResult> DownloadFileById(int cardFileId)
        {
            var file = await _cardFileService.DownloadСardFileById(cardFileId);

            if (file == null) NotFound();

            return Ok(file);
        }
    }
}