using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardFile.DAL.Entities
{
    public class CardFileEntitie
    {
        [Key]
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public DateTime DateOfCreation { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }


        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }
    }
}