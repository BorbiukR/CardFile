using System;

namespace CardFile.BLL.DTO
{
    public class CardTextFileDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfCreation { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        public byte[] File { get; set; }
        public bool IsDeleated { get; set; }

        public string UserId { get; set; }
    }
}