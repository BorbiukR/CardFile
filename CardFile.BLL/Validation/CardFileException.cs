using System;


namespace CardFile.BLL.Validation
{
    public class CardFileException : Exception
    {
        public CardFileException() { }

        public CardFileException(string message): base(message) { }

        public CardFileException(string message, Exception inner) : base(message, inner) { }
    }
}