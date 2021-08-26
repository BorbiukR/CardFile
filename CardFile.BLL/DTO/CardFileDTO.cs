using System;

namespace CardFile.BLL.DTO
{
    public class CardFileDTO
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public DateTime DateOfCreation { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }

        public string UserId { get; set; }
    }
}