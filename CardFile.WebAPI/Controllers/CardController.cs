using AutoMapper;
using CardFile.BLL.DTO;
using CardFile.BLL.Interfaces;
using CardFile.WebAPI.Contracts.Request;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CardFile.WebAPI.Controllers
{
    // TODO:  переробити try catch 
    // TODO: розібратися із форматом DataTime 
    [ApiController]
    [Route("api/card")]
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

        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateCardFile(IFormFile formFiles, [FromQuery]CardFileRequest request)  
        {
            try
            {
                var mappedCardFile = _mapper.Map<CardFileDTO>(request);

                _cardFileService.AddCardFileAsync(formFiles, mappedCardFile);

                return Ok(new { formFiles.FileName });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCardFile(int cardFileId, IFormFile formFiles, [FromQuery] CardFileRequest request)
        {
            try
            {
                var mappedCardFile = _mapper.Map<CardFileDTO>(request);

                await _cardFileService.UpdateCardFileAsync(cardFileId, formFiles, mappedCardFile);

                return Ok(new { formFiles.FileName });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCardFileById(int cardFileId)
        {
            try
            {
                await _cardFileService.DeleteByIdAsync(cardFileId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getAll")]
        [AllowAnonymous]
        public IActionResult GetAllCardFiles()
        {
            try
            {
                var cardFiles = _cardFileService.GetAll();

                return Ok(cardFiles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getById")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetCardFileByIdAsync(int id)
        {
            try
            {
                var cardFile = await _cardFileService.GetByIdAsync(id);

                return Ok(cardFile);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getByDateOfCreation")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetCardFilesByDateOfCreation(DateTime dateTime)
        {
            try
            {
                var cardFiles = _cardFileService.GetCardsByDateOfCreation(dateTime);

                return Ok(cardFiles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getByLanguage")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetCardFilesByLanguage(string language)
        {
            try
            {
                var cardFiles = _cardFileService.GetCardsByLanguage(language);

                return Ok(cardFiles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}