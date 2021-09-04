using CardFile.BLL.DTO;
using CardFile.DAL.Entities;
using CardFile.DAL.Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CardFile.Tests
{
    internal class CardFileDTOEqualityComparer : IEqualityComparer<CardFileDTO>
    {
        public bool Equals([AllowNull] CardFileDTO x, [AllowNull] CardFileDTO y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.UserId == y.UserId;
        }

        public int GetHashCode([DisallowNull] CardFileDTO obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class CardFileEntitieEqualityComparer : IEqualityComparer<CardFileEntitie>
    {
        public bool Equals([AllowNull] CardFileEntitie x, [AllowNull] CardFileEntitie y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.UserId == y.UserId;
        }

        public int GetHashCode([DisallowNull] CardFileEntitie obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class RefreshTokenEqualityComparer : IEqualityComparer<RefreshToken>
    {
        public bool Equals([AllowNull] RefreshToken x, [AllowNull] RefreshToken y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Token == y.Token
                && x.CreationDate == y.CreationDate
                && x.ExpiryDate == y.ExpiryDate
                && x.UserId == y.UserId;
        }

        public int GetHashCode([DisallowNull] RefreshToken obj)
        {
            return obj.GetHashCode();
        }
    }
}
