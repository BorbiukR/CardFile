using System;
using System.ComponentModel.DataAnnotations;

namespace CardFile.DAL.Entities
{
    public class CardTextFile
    {
        [Key]
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