namespace CardFile.DAL.Entities
{
    public class FileInfoEntitie
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }

        public CardFileEntitie CardFileEntitie { get; set; }
        public int CardFileEntitieId { get; set; }
    }
}