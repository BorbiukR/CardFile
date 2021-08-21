using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.BLL.Validation
{
    public class CardFileException : Exception
    {
        public CardFileException() { }

        public CardFileException(string message)
            : base(message) { }

        public CardFileException(string message, Exception inner)
            : base(message, inner) { }
    }
}