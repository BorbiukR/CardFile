using System.ComponentModel.DataAnnotations;

namespace CardFile.WebAPI.Contracts.Request
{
    public class CardFileRequest
    {
        [Required(ErrorMessage = "Language is required")]
        public string Language { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
    }
}