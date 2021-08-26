using AutoMapper;
using CardFile.BLL.DTO;
using CardFile.BLL.Interfaces;
using CardFile.WebAPI.Contracts.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CardFile.WebAPI.Controllers
{
    [Route("api/card")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardFileService _cardFileService;
        private readonly IMapper _mapper;

        public CardController(ICardFileService cardFileService, IMapper mapper)
        {
            _cardFileService = cardFileService;
            _mapper = mapper;
        }

        [HttpPost("upload")]
        public IActionResult Upload(IFormFile formFiles, [FromQuery]CardFileRequest request)  
        {
            try
            {
                var mappedCardFile = _mapper.Map<CardFileDTO>(request);

                _cardFileService.AddCardFileAsync(formFiles, mappedCardFile);

                return Ok(new { formFiles.FileName, });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}